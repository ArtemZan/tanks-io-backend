using Box2DX.Dynamics;
using Box2DX.Common;

namespace TanksIO.Game.Objects.Shapes
{
    class Rectangle : Mesh
    {
        public Rectangle(Vec2 size, float density = 1000)
        {
            float x = size.X / 2;
            float y = size.Y / 2;

            PolygonDef rect = new();
            rect.SetAsBox(x, y);
            rect.Density = density;

            AddConvex(rect);
        }
    }
}