namespace TanksIO.Game.Objects.Tanks
{
    using Shapes;
    using TanksIO.Game.Math;

    class DefaultTank : Tank
    {
        public DefaultTank(string id)
            : base(new(6, 6))
        {
            Add(new Rectangle(new(2, 5)));
            Add(new Rectangle(new(0.15, 3)).Transform(new Mat2x3(new(1, 0), new(0, 1), new(0, 2.4))));
        }

        protected override double GetTurretRotationSpeed()
        {
            const double rotSpeedK = 0.003;
            double rotSpeed = 0;

            //turretDir = new Vec2(System.Math.Cos(TurretRotation), System.Math.Sin(TurretRotation));

            int dir = 1;
            if (DesiredTurretDir.Cross(TurretDir) > 0)
            {
                dir = -1;
            }

            double dif = (DesiredTurretDir - TurretDir).Length();

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