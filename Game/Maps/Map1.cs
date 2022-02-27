using System.Collections.Generic;

namespace TanksIO.Game.Maps
{
    using Math;
    using Objects.Shapes;

    class Map1 : Scene
    {
        public Map1()
        {
            GameObjects.Add("1", new Rectangle(new Vec2(1, 5)).Transform(new Mat2x3(new(1, 0), new(0, 1), new(-3, 0))));
        }
    }
}