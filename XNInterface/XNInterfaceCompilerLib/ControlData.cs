using System.Collections.Generic;

namespace XNInterfaceCompilerLib
{
    public class ControlData
    {
        public List<ControlData> Children;
        public Dictionary<string, string> Attributes;

        public string Name = "";
    }
}
