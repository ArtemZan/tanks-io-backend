using System.Collections.Generic;

namespace TanksIO.Game.Objects.Tanks
{
    using Math;

    abstract class Tank : Mesh
    {
        //enum UpdateActionType
        //{
        //    RotateTurret,
        //    Rotate,
        //    Move
        //}

        //struct UpdateAction
        //{
        //    UpdateActionType Type;
        //    object payload;
        //}


        protected Vec2 TurretDir = new(0, 1);
        protected double TurretRotation;
        protected Vec2 DesiredTurretDir = new(0, 1);

        /// <summary>
        /// Measured in radians per second
        /// </summary>
        private double _rotationSpeed;

        private Vec2 _pos;
        protected double Rotation;

        //private UpdateAction[] _updatePipeline;
        protected TankVertMap VertMap;
        
        public Tank(TankVertMap vertMap)
        {
            VertMap = vertMap;
        }

        public void Aim(Vec2 dir)
        {
            DesiredTurretDir = dir;
        }

        protected abstract double GetTurretRotationSpeed();

        /// <summary>
        /// Rotates the turret vertices and updates _turretDir and _turretRotation
        /// </summary> 
        private void RotateTurret(double angle)
        {
            TransformHull(Mat2.Rotation(angle));

            TurretRotation += angle;
            TurretDir.X = System.Math.Cos(TurretRotation);
            TurretDir.Y = System.Math.Sin(TurretRotation);
        }

        private void RotateHull(double angle)
        {
            Transform(Mat2.Rotation(angle), 0, VertMap.HullSize, _pos);

            Rotation += angle;
        }

        private void TransformHull(Mat2 transform)
        {
            Transform(transform, VertMap.HullSize, VertMap.HullSize + VertMap.TurretSize, _pos);
        }


        public override UpdatePayload Update(double dTime)
        {
            Mat2x3 transform = new(1);

            bool isUpdated = false;
            bool isTransform = true;

            if(System.Math.Abs(_rotationSpeed) * dTime > 1e-3)
            {
                isUpdated = true;

                double rotAngle = _rotationSpeed * dTime;
                transform = Mat2.Rotation(rotAngle) * transform;
                RotateHull(rotAngle);
            }


            double turretSpeed = GetTurretRotationSpeed();
            if(System.Math.Abs(turretSpeed) * dTime > 1e-4)
            {
                isUpdated = true;
                isTransform = false;
            }
            RotateTurret((turretSpeed + _rotationSpeed) * dTime);

            if(!isUpdated)
            {
                return null;
            }

            if(isTransform)
            {
                return new TransformUpdatePayload(transform);
            }

            return new VerticesUpdatePayload(GetVertices());
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
    }
}