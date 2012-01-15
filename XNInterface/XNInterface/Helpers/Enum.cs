#if WINDOWS_PHONE
using System;
using System.Linq;
using System.Reflection;

namespace XNInterface.Helpers
{
    public class Enum<T>
    {
        public static string[] GetNames()
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new ArgumentException("Type '" + type.Name + "' is not an Enum.");

            return (from field in type.GetFields(BindingFlags.Public | BindingFlags.Static)
                    where field.IsLiteral
                    select field.Name).ToArray();
        }
    }
}
#endif