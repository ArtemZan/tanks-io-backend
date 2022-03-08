using Box2DX.Common;

namespace TanksIO.Game.Math
{
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

        public Mat2x3(Mat2 ij, Vec2 k)
        {
            I = ij.I;
            J = ij.J;
            K = k;
        }

        public Mat2x3(float scale = 1)
        {
            I = new(scale, 0);
            J = new(0, scale);
            K = new();
        }

        public override string ToString()
        {
            return new Utils.JSON().Set("i", I.ToJSON()).Set("j", J.ToJSON()).Set("k", K.ToJSON()).ToString();
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