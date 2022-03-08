using System.Collections.Generic;

namespace TanksIO.Game.Maps
{
    using Objects;
    using Objects.Shapes;

    class Map1 : Scene
    {
        public Map1()
            : base(new(100, 100))
        {
            Mesh obstMesh = new Rectangle(new(1, 5));

            Object obst = new(_world, obstMesh);

            obst.Mesh.MoveTo(new(-5, 0));

            obst.Body.SetPosition(new(-5, 0));

            GameObjects.Add("1", obst);
        }
    }
}