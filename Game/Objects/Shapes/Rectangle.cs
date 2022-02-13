namespace TanksIO.Game.Objects.Shapes
{
    using Math;
    class Rectangle : Mesh
    {
        public Rectangle(Vec2 size)
        {
            double x = size.X / 2;
            double y = size.Y / 2;

            Add(new Vec2(-x, -y));
            Add(new Vec2(x, -y));
            Add(new Vec2(x, y));
            Add(new Vec2(x, y));
            Add(new Vec2(-x, -y));
            Add(new Vec2(-x, y));
        }
    }
}