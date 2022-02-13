using System.Collections.Generic;

namespace TanksIO.Game.Maps
{
    using Math;
    using Objects.Shapes;

    class Map1 : Scene
    {
        public Map1()
        {
            GameObjects.Add("1", new Rectangle(new Vec2(0.5, 0.5)));
        }
    }
}