using System.Collections.Generic;

namespace TanksIO.Game.Objects.Tanks
{
    using Math;

    abstract class Tank : Mesh
    {
        private Vec2 _turretDir;
        protected TankVertMap VertMap;
        
        public Tank(TankVertMap vertMap)
        {
            VertMap = vertMap;
        }

        public void SetTurretDir()
        {

        }

        // And so on ...
    }

    struct TankVertMap
    {
        public int HullSize;
        public int TurretSize;

        public TankVertMap(int hullSize, int turretSize)
        {
            HullSize = hullSize;
            TurretSize = turretSize;
        }
    }
}