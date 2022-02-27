using System.Collections.Generic;
using System.Linq;

namespace TanksIO.Game.Objects
{
    using Math;

    class DynamicMesh : Mesh
    {
        public double Speed;
        public double RotSpeed;

        public DynamicMesh()
        {

        }

        public DynamicMesh(Mesh mesh)
            : base(mesh)
        {

        }

        public override UpdatePayload Update(double dTime)
        {
            double angle = RotSpeed * dTime;
            Rotate(angle);

            Vec2 offset = Dir * Speed * dTime;
            Move(offset);

            bool childUpdated = false;

            if (Children != null)
            {
                foreach ((_, Mesh mesh) in Children)
                {
                    if (mesh.Update(dTime) != null)
                    {
                        childUpdated = true;
                    }
                }
            }

            if (childUpdated)
            {
                return new VerticesUpdatePayload(GetVertices());
            }

            if (System.Math.Abs(angle) < 1e-6 && offset.Length() < 1e-6)
            {
                return null;
            }

            Mat2 rot = Mat2.Rotation(angle);

            Mat2x3 trfm = new(
                rot.I,
                rot.J,
                offset
            );

            return new TransformUpdatePayload(trfm, Pos - offset);
        }
    }
}
