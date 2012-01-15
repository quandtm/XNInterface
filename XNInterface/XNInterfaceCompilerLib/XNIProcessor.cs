using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Content.Pipeline;
using XNInterface.Attributes;
using XNInterface.Controls;
using XNInterfaceImporters;

namespace XNInterfaceCompilerLib
{
    [ContentProcessor(DisplayName = "XNInterface Processor")]
    public class XNIProcessor : ContentProcessor<ImportData, BaseControl>
    {
        private Dictionary<string, ControlMetadata> _metaData;
        private Importers attribImporter;

        public XNIProcessor()
            : base()
        {
            _metaData = new Dictionary<string, ControlMetadata>();
            attribImporter = new Importers();
            LoadAllControlMetadata();
        }

        private void LoadAllControlMetadata()
        {
            List<Assembly> assemblies = new List<Assembly>();
            assemblies.Add(Assembly.GetAssembly(typeof(BaseControl)));
            if (File.Exists("xninterface_assemblylist.txt"))
            {
                var lines = File.ReadAllLines("xninterface_assemblylist.txt");
                foreach (var line in lines)
                {
                    if (File.Exists(line))
                    {
                        try
                        {
                            assemblies.Add(Assembly.LoadFrom(Path.GetFullPath(line)));
                        }
                        catch (Exception ex)
                        {
                            File.AppendAllText("error.txt", ex.Message + "\n");
                        }
                    }
                }
            }

            foreach (var a in assemblies)
                LoadControlMetadata(a);
        }

        private void LoadControlMetadata(Assembly assembly)
        {
            IEnumerable<Type> types = null;
            try
            {
                types = assembly.GetTypes().Where(t => t.BaseType == typeof(BaseControl));
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var e in ex.LoaderExceptions)
                {
                    File.AppendAllText("error.txt", e.Message + "\n");
                }
            }

            var importType = typeof(Importers);

            foreach (var type in types)
            {
                var typeAttribs = Attribute.GetCustomAttributes(type);
                ControlMetadata meta = null;
                foreach (var attrib in typeAttribs)
                {
                    if (attrib is XNIControlAttribute)
                    {
                        var a = (XNIControlAttribute)attrib;
                        if (!_metaData.ContainsKey(a.Name))
                        {
                            meta = new ControlMetadata();
                            meta.Name = a.Name;
                            meta.MaxChildren = a.MaxChildren;
                            meta.Type = type;
                            meta.Parameters = new List<ParamMetadata>();
                            _metaData.Add(meta.Name, meta);
                            break;
                        }
                    }
                }

                if (meta == null)
                    continue;

                var fields = type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                var props = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
                foreach (var f in fields)
                {
                    var attribs = f.GetCustomAttributes(typeof(XNIParamAttribute), false);
                    if (attribs != null && attribs.Length >= 1)
                    {
                        var a = (XNIParamAttribute)attribs[0];
                        var p = new ParamMetadata();
                        p.XMLName = a.XMLName ?? f.Name.ToLower();
                        p.Optional = a.Optional;
                        p.DataType = f.FieldType;
                        p.ImporterOverride = a.ImporterOverride;
                        p.IsProperty = false;
                        p.FieldInfo = f;
                        meta.Parameters.Add(p);
                    }
                }

                foreach (var prop in props)
                {
                    var attribs = prop.GetCustomAttributes(typeof(XNIParamAttribute), false);
                    if (attribs != null && attribs.Length >= 1)
                    {
                        var a = (XNIParamAttribute)attribs[0];
                        var p = new ParamMetadata();
                        p.XMLName = a.XMLName ?? prop.Name.ToLower();
                        p.Optional = a.Optional;
                        p.DataType = prop.PropertyType;
                        p.ImporterOverride = a.ImporterOverride;
                        p.IsProperty = true;
                        p.PropInfo = prop;
                        meta.Parameters.Add(p);
                    }
                }
            }
        }

        public override BaseControl Process(ImportData input, ContentProcessorContext context)
        {
            return LoadControl(input.RootControl, context);
        }

        private BaseControl LoadControl(ControlData controlData, ContentProcessorContext context)
        {
            ControlMetadata meta;
            if (_metaData.TryGetValue(controlData.Name, out meta))
            {
                if (meta.MaxChildren >= 0 && meta.MaxChildren < controlData.Children.Count)
                    throw new Exception(string.Format("{0} can only have {1} children.", meta.Name, meta.MaxChildren));

                BaseControl control = (BaseControl)Activator.CreateInstance(meta.Type);

                foreach (var param in meta.Parameters)
                {
                    string attrib = "";
                    object val = null;
                    if (controlData.Attributes.TryGetValue(param.XMLName, out attrib))
                        val = attribImporter.Parse(string.IsNullOrWhiteSpace(param.ImporterOverride) ? param.DataType.Name : param.ImporterOverride, attrib, !param.Optional, context);
                    else if (!param.Optional)
                        throw new Exception("Required attribute \'" + param.XMLName + "\' was not found in the XML for the \'" + meta.Name + "\' control.");
                    else
                        val = GetDefault(param.DataType);

                    if (param.IsProperty)
                        param.PropInfo.SetValue(control, val, null);
                    else
                        param.FieldInfo.SetValue(control, val);
                }

                foreach (var child in controlData.Children)
                    control.AddChild(LoadControl(child, context));

                return control;
            }
            else
            {
                throw new Exception(string.Format("Control \"{0}\" does not exist.", controlData.Name));
            }
        }

        private object GetDefault(Type type)
        {
            if (type == typeof(bool))
                return true;
            else
                return null;
        }
    }
}