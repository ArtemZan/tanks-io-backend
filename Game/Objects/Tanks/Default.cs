using Box2DX.Dynamics;
using Box2DX.Common;
using System;

namespace TanksIO.Game.Objects.Tanks
{
    using Box2DX.Collision;
    using Shapes;
    using TanksIO.Game.Math;

    class DefaultTank : Tank
    {
        public DefaultTank(World world)
            :base(world)
        {
            Mesh.Combine(new Rectangle(new(5, 2)));
            Mesh.CreateFixtures(Body);
            Object turret = new Object(world, new Rectangle(new(3, 0.15f)));
            turret.Body.SetPosition(new(2.4f, 0));
            Turret = turret;

            Turret.Body.SetAngularDamping(0.2f);
            Body.SetAngularDamping(1);
        }

        protected override double GetTurretRotationSpeed()
        {
            const double rotSpeedK = 3;
            double rotSpeed;

            double absRot = Turret.Rot;
            Vec2 dir = Turret.Dir;
            float cross = Vec2.Cross(DesiredTurretDir, dir);
            float dot = Vec2.Dot(DesiredTurretDir, dir);
            double angle = System.Math.Acos(dot);

            int rotDir = 1;
            if (cross > 0)
            {
                rotDir = -1;
            }

            float dif = (DesiredTurretDir - dir).Length();
            float relRot = (float)(Turret.Rot - Rot);

            float v = Turret.Body.GetAngularVelocity() - Body.GetAngularVelocity();
            float inertia = (Turret.Body.GetInertia() + Body.GetInertia());
            float angularAcceleration = TurretJoint._maxMotorTorque / inertia;
            float brakingAngle = v * v / angularAcceleration / 2;

            if(v * v < 1e-2 && dif < 0.1)
            {
                TurretJoint.EnableMotor(false);
                rotSpeed = 0;
            }
            else if (System.Math.Abs(angle) <= brakingAngle)
            {
                TurretJoint._maxMotorTorque = (float)(v * v / angle / 2 * inertia);
                rotSpeed = 0;
                //rotSpeed = (float)System.Math.Sqrt(v * v / 2 - System.Math.Abs(angularAcceleration * angle)) * rotDir;
            }
            else
            {
                rotSpeed = rotDir * rotSpeedK;
                TurretJoint._maxMotorTorque = TurretMotorTorque;
                TurretJoint.EnableMotor(true);
            }

            Console.WriteLine(rotSpeed);

            return rotSpeed;
        }
    }
}