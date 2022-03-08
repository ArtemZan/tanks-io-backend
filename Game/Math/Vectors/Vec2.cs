using TanksIO.Utils;
using Box2DX.Common;

namespace TanksIO.Game.Math
{
    static class Vec2Ext
    {
        public static float Cos(this Vec2 vec1, Vec2 vec2)
        {
            return Vec2.Dot(vec1, vec2) / vec1.Length() / vec2.Length();
        }

        public static float Sin(this Vec2 vec1, Vec2 vec2)
        {
            return Vec2.Cross(vec1, vec2) / vec1.Length() / vec2.Length();
        }

        public static JSON ToJSON(this Vec2 vec)
        {
            return new JSON().Set("x", vec.X).Set("y", vec.Y);
        }
    }

    /*
    struct Vec2
    {
        public double X;
        public double Y;

        public Vec2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return new JSON().Set("x", X).Set("y", Y).ToString();
        }

        public double Length()
        {
            return System.Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        /// Gives the Z component of the cross product of the two vectors as if they were 3-dimensional with z = 0
        /// </summary>
        public double Cross(Vec2 vec)
        {
            return X * vec.Y - Y * vec.X;
        }

        /// <summary>
        /// Gives the sine of the angle between the two vectors
        /// </summary>
        public double Sin(Vec2 vec)
        {
            return Cross(vec) / Length() / vec.Length();
        }

        /// <summary>
        /// Dot product
        /// </summary>
        public double Dot(Vec2 vec)
        {
            return X * vec.X + Y * vec.Y;
        }

        /// <summary>
        /// Gives the cosine of the angle between the two vectors
        /// </summary>
        public double Cos(Vec2 vec)
        {
            return Dot(vec) / Length() / vec.Length();
        }

        public void Normalize()
        {
            this /= Length();
        }

        public Vec2 Normalized()
        {
            Vec2 res = this;
            res.Normalize();
            return res;
        }

        public static Vec2 operator *(Vec2 vec, double scale)
        {
            return new(vec.X * scale, vec.Y * scale);
        }

        public static Vec2 operator *(double scale, Vec2 vec)
        {
            return new(vec.X * scale, vec.Y * scale);
        }

        public static Vec2 operator /(Vec2 vec, double scale)
        {
            return new(vec.X / scale, vec.Y / scale);
        }

        public static Vec2 operator +(Vec2 vec1, Vec2 vec2)
        {
            return new(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public static Vec2 operator -(Vec2 vec1, Vec2 vec2)
        {
            return new(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }
    }
    */
}