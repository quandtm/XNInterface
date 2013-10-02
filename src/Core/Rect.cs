namespace XNInterface.Core
{
    public sealed class Rect
    {
        public Vec2 Position { get; set; } // Will always be relative to parent
        public Vec2 Size { get; set; }

        public void Calculate(float parentWidth, float parentHeight)
        {
            Position.Calculate(parentWidth, parentHeight);
            Size.Calculate(parentWidth, parentHeight);
        }

        public void Calculate(Vec2 parentSize)
        {
            Position.Calculate(parentSize);
            Size.Calculate(parentSize);
        }

        public void Calculate(Rect parent)
        {
            Calculate(parent.Size);
        }
    }
}