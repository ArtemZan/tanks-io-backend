namespace TanksIO.Game.Objects.Tanks
{
    using Shapes;
    using TanksIO.Game.Math;

    class DefaultTank : Tank
    {
        public DefaultTank()
        {
            Merge(new Rectangle(new(5, 2)));
            DynamicMesh turret = new(new Rectangle(new(3, 0.15)).Transform(new Mat2x3(new(1, 0), new(0, 1), new(2.4, 0))));
            Turret = turret;
            Add(turret);
        }

        protected override double GetTurretRotationSpeed()
        {
            const double rotSpeedK = 0.003;
            double rotSpeed = 0;

            double absRot = Turret.Rot + Rot;
            Vec2 absDir = new(System.Math.Cos(absRot), System.Math.Sin(absRot));

            int dir = 1;
            if (DesiredTurretDir.Cross(absDir) > 0)
            {
                dir = -1;
            }

            double dif = (DesiredTurretDir - absDir).Length();

            const double brakingDistance = 0.5;

            if (dif < brakingDistance)
            {
                if (dif > 1e-3)
                {
                    rotSpeed = dif * dir * rotSpeedK / brakingDistance;
                }
            }
            else
            {
                rotSpeed = dir * rotSpeedK;
            }

            return rotSpeed;
        }
    }
}