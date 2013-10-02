using System;

namespace XNInterface.Core
{
    public sealed class Vec2 : IEquatable<Vec2>
    {
        public enum Vec2ValueType : short
        {
            Absolute,
            Percentage
        }

        private float _x, _y;

        public float X
        {
            get { return _x; }
            set
            {
                if (XType == Vec2ValueType.Percentage)
                {
                    if (value > 1)
                    {
                        _x = 1;
                        return;
                    }
                    if (value < 0)
                    {
                        _x = 0;
                        return;
                    }
                }
                _x = value;
            }
        }
        public float Y
        {
            get { return _y; }
            set
            {
                if (YType == Vec2ValueType.Percentage)
                {
                    if (value > 1)
                    {
                        _y = 1;
                        return;
                    }
                    if (value < 0)
                    {
                        _y = 0;
                        return;
                    }
                }
                _y = value;
            }
        }
        public Vec2ValueType XType { get; set; }
        public Vec2ValueType YType { get; set; }

        public float RealX { get; private set; }
        public float RealY { get; private set; }

        public Vec2()
            : this(0, 0)
        {
        }

        public Vec2(float absX, float absY)
            : this(absX, Vec2ValueType.Absolute, absY, Vec2ValueType.Absolute)
        {
        }

        public Vec2(float x, Vec2ValueType xtype, float y, Vec2ValueType ytype)
        {
            XType = xtype;
            YType = ytype;
            X = x;
            Y = y;
            RealX = X;
            RealY = Y;
        }

        public void Calculate(float parentWidth, float parentHeight)
        {
            if (parentWidth <= 0 || XType == Vec2ValueType.Absolute)
                RealX = X;
            else
                RealX = parentWidth * X;

            if (parentHeight <= 0 || YType == Vec2ValueType.Absolute)
                RealY = Y;
            else
                RealY = parentHeight * Y;
        }

        public void Calculate(Vec2 owner)
        {
            Calculate(owner.RealX, owner.RealY);
        }

        public bool Equals(Vec2 other)
        {
            bool xEqual = other.XType == XType && other.X == X;
            bool yEqual = other.YType == YType && other.Y == Y;
            return xEqual && yEqual;
        }

        public static bool operator ==(Vec2 a, Vec2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vec2 a, Vec2 b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vec2)
                return ((Vec2)obj).Equals(this);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("RealX={0} RealY={1}", RealX, RealY);
        }
    }
}