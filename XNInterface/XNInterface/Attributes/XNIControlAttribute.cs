using System;

namespace XNInterface.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class XNIControlAttribute : Attribute
    {
        public readonly string Name;
        public readonly int MaxChildren;
        public readonly string ImporterTypeName;

        public XNIControlAttribute(string name, int maxChildren, string importerTypeName)
        {
            Name = name.ToLower();
            MaxChildren = maxChildren;
            ImporterTypeName = importerTypeName.ToLower();
        }

        public XNIControlAttribute(string name, int maxChildren)
            : this(name, maxChildren, "")
        { }
    }
}
