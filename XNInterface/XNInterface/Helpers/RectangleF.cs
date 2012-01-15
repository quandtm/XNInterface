using System;
using Microsoft.Xna.Framework;

namespace XNInterface.Helpers
{
    public struct RectangleF : IEquatable<RectangleF>
    {
        public float X, Y, Width, Height;

        public Vector2 Location
        {
            get
            {
                return new Vector2(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.X;
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(Width, Height);
            }
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2((X + Width) / 2f, (Y + Height) / 2f);
            }
        }

        public float Rotation;

        public RectangleF(float x, float y, float width, float height)
            : this(x, y, width, height, 0)
        {
        }

        public RectangleF(float x, float y, float width, float height, float rotation)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Rotation = rotation;
        }

        public RectangleF(ref Vector2 pos, ref Vector2 size)
            : this(ref pos, ref size, 0)
        { }

        public RectangleF(ref Vector2 pos, ref Vector2 size, float rotation)
            : this(pos.X, pos.Y, size.X, size.Y, rotation)
        { }

        public RectangleF(Vector2 pos, Vector2 size)
            : this(ref pos, ref size)
        { }

        public RectangleF(Vector2 pos, Vector2 size, float rotation)
            : this(ref pos, ref size, rotation)
        { }

        public RectangleF(SpecialVector2 pos, SpecialVector2 size)
            : this(pos.Final, size.Final)
        { }

        public bool Contains(float x, float y)
        {
            var halfX = Width / 2f;
            var halfY = Height / 2f;
            var adjY = y - (Y + halfY);
            var adjX = x - (X + halfX);
            float cosTheta = (float)Math.Cos(-Rotation);
            float sinTheta = (float)Math.Sin(-Rotation);
            var tempX = adjX;
            adjX = (adjX * cosTheta) - (adjY * sinTheta);
            adjY = (adjY * cosTheta) + (tempX * sinTheta);
            var res = adjX >= -halfX && adjX <= halfX && adjY >= -halfY && adjY <= halfY;
            return res;
        }

        public bool Contains(ref Vector2 point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Contains(Vector2 point)
        {
            return Contains(ref point);
        }

        public bool Equals(RectangleF other)
        {
            return (other.X == X) && (other.X == Y) && (other.Width == Width) && (other.Height == Height);
        }

        public static bool operator ==(RectangleF left, RectangleF right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RectangleF left, RectangleF right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is RectangleF)
                return Equals((RectangleF)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
