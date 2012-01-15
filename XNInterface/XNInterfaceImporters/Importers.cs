using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using XNInterface.Controls;
using XNInterface.Helpers;

namespace XNInterfaceImporters
{
    public class Importers
    {
        public object Parse(string typeName, string attribValue, bool throwOnError, ContentProcessorContext context)
        {
            switch (typeName)
            {
                case "Int32":
                    return ParseInt(attribValue, throwOnError);

                case "Single":
                    return ParseFloat(attribValue, throwOnError);

                case "SpecialVector2":
                    return ParseSpecialVector2(attribValue, throwOnError);

                case "String":
                    return attribValue;

                case "Color":
                    return ParseColor(attribValue, throwOnError);

                case "Texture2D":
                    return ParseTexture2D(attribValue, throwOnError, context);

                case "HorizontalAlignment":
                    return ParseEnum<HorizontalAlignment>(attribValue, throwOnError);

                case "VerticalAlignment":
                    return ParseEnum<VerticalAlignment>(attribValue, throwOnError);

                case "Font":
                    return ParseFont(attribValue, throwOnError, context);

                case "FormatItems":
                    return ParseFormatItems(attribValue, throwOnError);

                case "StackPanelOrientation":
                    return ParseEnum<StackPanelOrientation>(attribValue, throwOnError);

                case "ControlVisibility":
                    return ParseEnum<ControlVisibility>(attribValue, throwOnError);

                case "Boolean":
                    return ParseBool(attribValue, throwOnError);

                default:
                    if (throwOnError)
                        throw new Exception("Could not find parser for type \'" + typeName + "\'");
                    else
                        context.Logger.LogImportantMessage("ERROR: Could not find parser for optional type \'" + typeName + "\'");
                    return null;
            }
        }

        private object ParseBool(string attribValue, bool throwOnError)
        {
            bool b;
            if (bool.TryParse(attribValue, out b))
                return b;
            else
            {
                if (throwOnError)
                    throw new Exception("Could not parse \"" + attribValue + "\" as a boolean.");
                else
                    return true;
            }
        }

        private object ParseFormatItems(string attribValue, bool throwOnError)
        {
            return FormatItem.ParseFormatString(attribValue);
        }

        private object ParseFont(string attribValue, bool throwOnError, ContentProcessorContext context)
        {
            try
            {
                return Loaders.BuildFontExternal(context, attribValue);
            }
            catch
            {
                if (throwOnError)
                    throw;
                return null;
            }
        }

        private T ParseEnum<T>(string attribValue, bool throwOnError) where T : struct
        {
            T val;
            if (Enum.TryParse<T>(attribValue, true, out val))
                return val;
            else if (throwOnError)
                throw new Exception("Could not parse \'" + attribValue + "\' into the following enumeration: " + typeof(T).Name);
            else
                return default(T);
        }

        private string ParseTexture2D(string attribValue, bool throwOnError, ContentProcessorContext context)
        {
            try
            {
                return Loaders.BuildTextureExternal(context, attribValue);
            }
            catch
            {
                if (throwOnError)
                    throw;
                return null;
            }
        }

        public int ParseInt(string value, bool throwOnError)
        {
            int val = 0;
            if (!int.TryParse(value, out val) && throwOnError)
                throw new Exception("Failed to parse \'" + value + "\' as a 32 bit integer.");
            return val;
        }

        public float ParseFloat(string value, bool throwOnError)
        {
            float val = 0;
            if (!float.TryParse(value, out val) && throwOnError)
                throw new Exception("Failed to parse \'" + value + "\' as a Single.");
            return val;
        }

        public Color ParseColor(string value, bool throwOnError)
        {
            Color col = Color.Transparent;
            if (value.StartsWith("#"))
            {
                // #RRGGBB[AA] format
                value = value.Remove(0, 1);
                if (value.Length == 6 || value.Length == 8)
                {
                    // RRGGBB
                    var sred = value.Substring(0, 2);
                    var sgreen = value.Substring(2, 2);
                    var sblue = value.Substring(4, 2);
                    col.R = byte.Parse(sred, NumberStyles.HexNumber);
                    col.G = byte.Parse(sgreen, NumberStyles.HexNumber);
                    col.B = byte.Parse(sblue, NumberStyles.HexNumber);

                    if (value.Length == 8)
                    {
                        // RRGGBBAA
                        var salpha = value.Substring(6, 2);
                        col.A = byte.Parse(salpha, NumberStyles.HexNumber);
                    }
                    else
                        col.A = 0xFF;
                }
                else if (throwOnError)
                    throw new Exception("Failed to parse color value \'#" + value + "\'. Should be in #RRGGBB or #RRGGBBAA form.");
            }
            else
            {
                // RRR,GGG,BBB[,AAA] format
                var parts = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3 || parts.Length == 4)
                {
                    col.R = byte.Parse(parts[0]);
                    col.G = byte.Parse(parts[1]);
                    col.B = byte.Parse(parts[2]);

                    if (parts.Length == 4)
                        col.A = byte.Parse(parts[3]);
                }
                else if (throwOnError)
                    throw new Exception("Failed to parse color value \'" + value + "\'. Should be in RRR,GGG,BBB or RRR,GGG,BBB,AAA form.");
            }
            return col;
        }

        public SpecialVector2 ParseSpecialVector2(string value, bool throwOnError)
        {
            var split = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            SpecialVector2 output = new SpecialVector2(0, 0, SpecialVector2Type.Inherit);
            if (split.Length == 1)
            {
                var part = split[0];
                if (part.Equals("inherit", StringComparison.InvariantCultureIgnoreCase))
                {
                    return output;
                }
                else if (part.EndsWith("%"))
                {
                    part = part.TrimEnd('%');
                    output.SpecialType = SpecialVector2Type.Percentage;
                }
                else
                    output.SpecialType = SpecialVector2Type.Absolute;
                float val;
                if (float.TryParse(part, out val))
                    output.InputX = output.InputY = val;
                else if (throwOnError)
                    throw new Exception("Failed to parse \'" + value + "\'. Expected a floating point number.");
            }
            else if (split.Length == 2)
            {
                var p1 = split[0];
                var p2 = split[1];
                if (p1.EndsWith("%"))
                {
                    output.SpecialType = SpecialVector2Type.Percentage;
                    p1 = p1.TrimEnd('%');
                    p2 = p2.TrimEnd('%');
                }
                else
                    output.SpecialType = SpecialVector2Type.Absolute;
                float p1val, p2val;
                if (float.TryParse(p1, out p1val) && float.TryParse(p2, out p2val))
                {
                    output.InputX = p1val;
                    output.InputY = p2val;
                }
                else if (throwOnError)
                    throw new Exception("Failed to parse \'" + value + "\'. Values must be floating point numbers and either both absolute, or both percentages.");
            }
            else
            {
                if (throwOnError)
                    throw new Exception(value + " is the incorrect format. Expected one/two numbers split by a comma, they can also be percentages. (But not mixed)");
            }

            if (output.SpecialType == SpecialVector2Type.Percentage)
            {
                output.InputX /= 100f;
                output.InputY /= 100f;
            }
            return output;
        }
    }
}
