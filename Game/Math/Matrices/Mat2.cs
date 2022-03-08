using Box2DX.Common;

namespace TanksIO.Game.Math
{
    

    struct Mat2
    {
        public Vec2 I;
        public Vec2 J;

        /// <summary>
        /// Creates scale matrix
        /// </summary>
        public Mat2(float scale = 1)
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
            float cos = vec1.Cos(vec2);
            float sin = vec1.Sin(vec2);

            return new(
                new Vec2(cos, sin),
                new Vec2(-sin, cos)
            );
        }

        public static Mat2 Rotation(double angle)
        {
            float cos = (float)System.Math.Cos(angle);
            float sin = (float)System.Math.Sin(angle);

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
}