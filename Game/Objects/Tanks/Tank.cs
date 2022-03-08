using System;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;
using TanksIO.Game.Math;

namespace TanksIO.Game.Objects.Tanks
{
    /* abstract class Tank : DynamicMesh
    {

        protected Vec2 TurretDir = new(0, 1);
        protected double TurretRotation = System.Math.PI / 2;
        protected Vec2 DesiredTurretDir = new(0, 1);

        protected double Rotation = System.Math.PI / 2;

        //The machine properties
        protected readonly TankVertMap VertMap;
        public readonly double MaxSpeed;

        public Tank(TankVertMap vertMap)
        {
            VertMap = vertMap;
        }

        public void Aim(Vec2 dir)
        {
            DesiredTurretDir = dir;
        }

        public double GetRotation()
        {
            return Rotation;
        }



        protected abstract double GetTurretRotationSpeed();

        /// <summary>
        /// Rotates the turret vertices and updates TurretDir and TurretRotation
        /// </summary> 
        private void RotateTurret(double angle)
        {
            TransformTurret(Mat2.Rotation(angle));

            TurretRotation += angle;
            TurretDir.X = System.Math.Cos(TurretRotation);
            TurretDir.Y = System.Math.Sin(TurretRotation);
        }

        private void RotateHull(double angle)
        {
            Transform(Mat2.Rotation(angle), 0, VertMap.HullSize, Pos);

            Rotation += angle;

            Dir.X = System.Math.Cos(Rotation);
            Dir.Y = System.Math.Sin(Rotation);
        }

        private void TransformTurret(Mat2 transform)
        {
            Transform(transform, VertMap.HullSize, VertMap.HullSize + VertMap.TurretSize, Pos);
        }


        public override UpdatePayload Update(double dTime)
        {
            Mat2x3 transform = new(1);

            bool shouldRotate = System.Math.Abs(RotationSpeed) > 1e-5;
            bool shouldMove = System.Math.Abs(Speed) > 1e-5;

            bool isUpdated = false;
            bool isTransform = true;

            Vec2 startPos = Pos;

            if (shouldRotate)
            {
                isUpdated = true;

                double rotAngle = RotationSpeed * dTime;
                transform = Mat2.Rotation(rotAngle) * transform;
                RotateHull(rotAngle);
            }

            if(shouldMove)
            {
                isUpdated = true;

                Vec2 dPos = Dir * Speed * dTime;

                transform.K += dPos;
                Transform(new Mat2x3(new(1, 0), new(0, 1), dPos), startPos);
                Pos += dPos;
            }


            double turretAbsRotationSpeed = GetTurretRotationSpeed();
            double turretRelRotationSpeed = turretAbsRotationSpeed + RotationSpeed;
            //Console.WriteLine("Aim dir: " + DesiredTurretDir + ". Turret dir: " + TurretDir + ". Rotation speed: " + RotationSpeed);
            if (System.Math.Abs(turretRelRotationSpeed) > 1e-6)
            {
                isUpdated = true;
                isTransform = false;
            }

            if(System.Math.Abs(turretAbsRotationSpeed) > 1e-6)
            {
                RotateTurret(turretAbsRotationSpeed * dTime);
            }




            if (!isUpdated)
            {
                return null;
            }

            if (isTransform)
            {
                return new TransformUpdatePayload(transform, startPos);
            }

            return new VerticesUpdatePayload(GetOwnVertices());
        }
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
    }*/


    abstract class Tank : Object
    {
        private Object _turret;
        protected RevoluteJoint TurretJoint;
        public float Speed = 0;

        public Object Turret
        {
            get => _turret;
            set
            {
                _turret = value;
                RevoluteJointDef jointDef = new();
                jointDef.Initialize(Body, _turret.Body, Body.GetPosition() + TurretOffsetFromCenter);
                jointDef.CollideConnected = false;
                if(MaxTurretRotAngle > 0)
                {
                    jointDef.EnableLimit = true;
                    jointDef.UpperAngle = MaxTurretRotAngle;
                    jointDef.LowerAngle = -MaxTurretRotAngle;
                }

                jointDef.MaxMotorTorque = TurretMotorTorque;
                jointDef.EnableMotor = true;

                TurretJoint = (RevoluteJoint)Body.GetWorld().CreateJoint(jointDef);

                Add(value);
            }
        }

        protected Vec2 DesiredTurretDir = new(1, 0);

        //The machine properties
        public readonly float MaxSpeed;
        public readonly float BarrelLength;
        public readonly Vec2 TurretOffsetFromCenter;
        public readonly float MaxTurretRotAngle = -1;
        public readonly float TurretMotorTorque = 10000;

        public Tank(World world)
            : base(world)
        {

        }

        public void Aim(Vec2 dir)
        {
            DesiredTurretDir = dir;
        }


        protected abstract double GetTurretRotationSpeed();


        public override UpdatePayload Update(double dTime)
        {
            TurretJoint.MotorSpeed = (float)GetTurretRotationSpeed();

            Body.ApplyForce(Dir * Speed * 10000, Pos);

            //Console.WriteLine(Body.GetPosition().ToJSON() + " " + Mesh.Pos.ToJSON());
            //Console.WriteLine(TurretJoint.MotorSpeed + " " + Turret.Rot + " " + Turret.Mesh.Rot);

            return base.Update(dTime);

            /* 
            Mat2x3 transform = new(1);

            bool shouldRotate = System.Math.Abs(RotationSpeed) > 1e-5;
            bool shouldMove = System.Math.Abs(Speed) > 1e-5;

            bool isUpdated = false;
            bool isTransform = true;

            Vec2 startPos = Pos;

            if (shouldRotate)
            {
                isUpdated = true;

                double rotAngle = RotationSpeed * dTime;
                transform = Mat2.Rotation(rotAngle) * transform;
                RotateHull(rotAngle);
            }

            if (shouldMove)
            {
                isUpdated = true;

                Vec2 dPos = Dir * Speed * dTime;

                transform.K += dPos;
                Transform(new Mat2x3(new(1, 0), new(0, 1), dPos), startPos);
                Pos += dPos;
            }


            double turretAbsRotationSpeed = GetTurretRotationSpeed();
            double turretRelRotationSpeed = turretAbsRotationSpeed + RotationSpeed;
            //Console.WriteLine("Aim dir: " + DesiredTurretDir + ". Turret dir: " + TurretDir + ". Rotation speed: " + RotationSpeed);
            if (System.Math.Abs(turretRelRotationSpeed) > 1e-6)
            {
                isUpdated = true;
                isTransform = false;
            }

            if (System.Math.Abs(turretAbsRotationSpeed) > 1e-6)
            {
                RotateTurret(turretAbsRotationSpeed * dTime);
            }




            if (!isUpdated)
            {
                return null;
            }

            if (isTransform)
            {
                return new TransformUpdatePayload(transform, startPos);
            }

            return new VerticesUpdatePayload(GetOwnVertices());
            */
        }
    }
}