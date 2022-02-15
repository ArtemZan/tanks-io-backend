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

    struct Mat2
    {
        public Vec2 I;
        public Vec2 J;

        /// <summary>
        /// Creates scale matrix
        /// </summary>
        public Mat2(double scale = 1)
        {
            I = new(scale, 0);
            J = new(0, scale);
        }

        public Mat2(Vec2 i, Vec2 j)
        {
            I = i;
            J = j;
        }

        public static Mat2 Rotation(Vec2 vec1, Vec2 vec2)
        {
            double cos = vec1.Cos(vec2);
            double sin = vec1.Sin(vec2);

            return new(
                new Vec2(cos, sin),
                new Vec2(-sin, cos)
            );
        }

        public static Mat2 Rotation(double angle)
        {
            double cos = System.Math.Cos(angle);
            double sin = System.Math.Sin(angle);
            
            return new(
                new Vec2(cos, sin),
                new Vec2(-sin, cos)
            );
        }

        public static Vec2 operator *(Mat2 mat, Vec2 vec)
        {
            return vec.X * mat.I + vec.Y * mat.J;
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

        public Mat2x3(double scale = 1)
        {
            I = new(scale, 0);
            J = new(0, scale);
            K = new();
        }

        public override string ToString()
        {
            return new JSON().Set("i", I).Set("j", J).Set("k", K).ToString();
        }

        public static Vec2 operator *(Mat2x3 mat, Vec3 vec)
        {
            return vec.X * mat.I + vec.Y * mat.J + vec.Z * mat.K;
        }

        public static Mat2x3 operator *(Mat2 mat1, Mat2x3 mat2)
        {
            return new(mat1 * mat2.I, mat1 * mat2.J, mat1 * mat2.K);
        }

        //public static Vec2 operator*=(Mat2x3 mat, out Vec3 vec)
        //    {
        //    vec = new();
        //    }
}


}