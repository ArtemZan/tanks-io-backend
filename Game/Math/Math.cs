using Newtonsoft.Json;

namespace TanksIO.Game.Math
{
    using Utils;

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

        public static Vec2 operator *(Vec2 vec, double scale)
        {
            return new(vec.X * scale, vec.Y * scale);
        }

        public static Vec2 operator *(double scale, Vec2 vec)
        {
            return new(vec.X * scale, vec.Y * scale);
        }

        public static Vec2 operator +(Vec2 vec1, Vec2 vec2)
        {
            return new(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }
    }

    struct Vec3
    {
        public double X;
        public double Y;
        public double Z;

        public Vec3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vec3(Vec2 vec, double z)
        {
            X = vec.X;
            Y = vec.Y;
            Z = z;
        }

        public Vec3(double x, Vec2 vec)
        {
            X = x;
            Y = vec.X;
            Z = vec.Y;
        }

        public override string ToString()
        {
            return new JSON().Set("x", X).Set("y", Y).Set("z", Z).ToString();
        }
    }

    struct Mat2x3
    {
        public Vec2 I;
        public Vec2 J;
        public Vec2 K;

        public Mat2x3(Vec2 i, Vec2 j, Vec2 k)
        {
            I = i;
            J = j;
            K = k;
        }

        public override string ToString()
        {
            return new JSON().Set("i", I).Set("j", J).Set("k", K).ToString();
        }

        public static Vec2 operator *(Mat2x3 mat, Vec3 vec)
        {
            return vec.X * mat.I + vec.Y * mat.J + vec.Z * mat.K;
        }

        //public static Vec2 operator*=(Mat2x3 mat, out Vec3 vec)
        //    {
        //    vec = new();
        //    }
}


}