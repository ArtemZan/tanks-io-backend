using Box2DX.Common;

namespace TanksIO.Game.Math
{
    class Edge
    {
        public Vec2 Start;
        public Vec2 End;

        public Vec2 GetVec()
        {
            return Start - End;
        }

        public double GetLength()
        {
            return GetVec().Length();
        }
    }
}