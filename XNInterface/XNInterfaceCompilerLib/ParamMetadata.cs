using System;
using System.Reflection;

namespace XNInterfaceCompilerLib
{
    public class ParamMetadata
    {
        public string XMLName;
        public bool Optional;
        public Type DataType;
        public string ImporterOverride;

        public FieldInfo FieldInfo;
        public PropertyInfo PropInfo;
        public bool IsProperty;
    }
}
