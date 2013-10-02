namespace XNInterface.Core
{
    public sealed class Vec2
    {
        public enum Vec2ValueType : short
        {
            Absolute,
            Percentage
        }

        public float X { get; set; }
        public float Y { get; set; }
        public Vec2ValueType XType { get; set; }
        public Vec2ValueType YType { get; set; }

        public float RealX { get; private set; }
        public float RealY { get; private set; }

        public Vec2()
            : this(0, 0)
        {
        }

        public Vec2(float absX, float absY)
            : this(absX, Vec2ValueType.Absolute, absY, Vec2ValueType.Percentage)
        {
        }

        public Vec2(float x, Vec2ValueType xtype, float y, Vec2ValueType ytype)
        {
            XType = xtype;
            YType = ytype;
            X = x;
            Y = y;
        }

        public void Calculate(float parentWidth, float parentHeight)
        {
            if (XType == Vec2ValueType.Absolute)
                RealX = X;
            else
                RealX = parentWidth * X;

            if (YType == Vec2ValueType.Absolute)
                RealY = Y;
            else
                RealY = parentHeight * Y;
        }

        public void Calculate(Vec2 owner)
        {
            Calculate(owner.RealX, owner.RealY);
        }
    }
}