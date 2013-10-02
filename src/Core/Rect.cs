namespace XNInterface.Core
{
    public sealed class Rect
    {
        public Vec2 Position { get; set; } // Will always be relative to parent
        public Vec2 Size { get; set; }

        public Rect()
            : this(0, 0, 0, 0)
        {
        }

        public Rect(float x, float y, float width, float height)
            : this(new Vec2(x, y), new Vec2(width, height))
        {
        }

        public Rect(Vec2 position, Vec2 size)
        {
            Position = position;
            Size = size;
        }

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