using System;
using System.Collections.Generic;

namespace XNInterfaceCompilerLib
{
    public class ControlMetadata
    {
        public Type Type;
        public string Name;
        public int MaxChildren;

        public List<ParamMetadata> Parameters;
    }
}
