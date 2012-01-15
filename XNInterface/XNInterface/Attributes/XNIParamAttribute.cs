using System;

namespace XNInterface.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class XNIParamAttribute : Attribute
    {
        public string XMLName;
        public bool Optional;
        public string ImporterOverride;

        public XNIParamAttribute(string xml, bool optional, string importerOverride)
        {
            if (xml != null)
                XMLName = xml.ToLower();
            Optional = optional;
            ImporterOverride = importerOverride;
        }

        public XNIParamAttribute(string xml)
            : this(xml, true, null)
        { }

        public XNIParamAttribute(string xml, bool optional)
            : this(xml, optional, null)
        { }

        public XNIParamAttribute(string xml, string importerOverride)
            : this(xml, true, importerOverride)
        { }

        public XNIParamAttribute()
            : this(null, true, null)
        { }
    }
}
