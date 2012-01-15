using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace XNInterface.Helpers
{
    public enum SpecialVector2Type : short
    {
        Absolute,
        Percentage,
        Inherit
    }

    public enum HorizontalAlignment : short
    {
        Left,
        Center,
        Right
    }

    public enum VerticalAlignment : short
    {
        Top,
        Middle,
        Bottom
    }

    public class SpecialVector2
    {
        public float InputX;
        public float InputY;

        public SpecialVector2Type SpecialType;

        [ContentSerializerIgnore]
        public Vector2 Final;

        public SpecialVector2(float x, float y, SpecialVector2Type type)
        {
            InputX = x;
            InputY = y;
            SpecialType = type;
            Final = new Vector2();
        }

        public SpecialVector2()
            : this(0, 0, SpecialVector2Type.Inherit)
        { }

        public void CalculateSize(float refWidth, float refHeight)
        {
            switch (SpecialType)
            {
                case SpecialVector2Type.Absolute:
                    Final.X = InputX;
                    Final.Y = InputY;
                    break;

                case SpecialVector2Type.Percentage:
                    Final.X = InputX * refWidth;
                    Final.Y = InputY * refHeight;
                    break;

                case SpecialVector2Type.Inherit:
                    Final.X = refWidth;
                    Final.Y = refHeight;
                    break;
            }
        }

        public void CalculateSize(Vector2 refSize)
        {
            CalculateSize(ref refSize);
        }

        public void CalculateSize(ref Vector2 refSize)
        {
            CalculateSize(refSize.X, refSize.X);
        }

        public void CalculatePosition(float refX, float refY, float refWidth, float refHeight, float thisWidth, float thisHeight, HorizontalAlignment hAlign, VerticalAlignment vAlign)
        {
            switch (SpecialType)
            {
                case SpecialVector2Type.Absolute:
                    Final.X = refX + InputX;
                    Final.Y = refY + InputY;
                    break;

                case SpecialVector2Type.Inherit:
                    float x = refX;
                    float y = refY;
                    switch (hAlign)
                    {
                        case HorizontalAlignment.Center:
                            x += (refWidth / 2f) - (thisWidth / 2f);
                            break;

                        case HorizontalAlignment.Right:
                            x += refWidth - thisWidth;
                            break;
                    }

                    switch (vAlign)
                    {
                        case VerticalAlignment.Middle:
                            y += (refHeight / 2f) - (thisHeight / 2f);
                            break;

                        case VerticalAlignment.Bottom:
                            y += refHeight - thisHeight;
                            break;
                    }
                    Final.X = x;
                    Final.Y = y;
                    break;

                case SpecialVector2Type.Percentage:
                    Final.X = refX + (InputX * refWidth);
                    Final.Y = refY + (InputY * refHeight);
                    break;
            }
        }
    }
}
