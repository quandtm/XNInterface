using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace XNInterfaceCompilerLib
{
    [ContentImporter(".xml", DisplayName = "XNInterface Importer", DefaultProcessor = "XNIProcessor")]
    public class XNIImporter : ContentImporter<ImportData>
    {
        public override ImportData Import(string filename, ContentImporterContext context)
        {
            ImportData data = new ImportData();
            XDocument doc = XDocument.Load(filename);

            var root = doc.Root;
            data.RootControl = new ControlData();
            data.RootControl.Name = root.Name.LocalName.ToLower();
            data.RootControl.Attributes = LoadAttributes(root);
            data.RootControl.Children = LoadChildren(root);

            return data;
        }

        private List<ControlData> LoadChildren(XElement root)
        {
            List<ControlData> controls = new List<ControlData>();

            var children = root.Elements();
            foreach (var child in children)
            {
                var data = new ControlData();
                data.Name = child.Name.LocalName.ToLower();
                data.Attributes = LoadAttributes(child);
                data.Children = LoadChildren(child);
                controls.Add(data);
            }

            return controls;
        }

        private Dictionary<string, string> LoadAttributes(XElement root)
        {
            Dictionary<string, string> attribs = new Dictionary<string, string>();

            var at = root.Attributes();
            foreach (var attrib in at)
            {
                if (!string.IsNullOrWhiteSpace(attrib.Value))
                    attribs.Add(attrib.Name.LocalName.ToLower(), attrib.Value);
            }

            return attribs;
        }
    }
}
