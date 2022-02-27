using System.Collections.Generic;
using System.Linq;

namespace TanksIO.Game.Objects
{
    using Math;

    abstract class DynamicMesh : Mesh
    {
        public double Speed;
        public double RotSpeed;

        public override UpdatePayload Update(double dTime)
        {
            double angle = RotSpeed * dTime;
            Rotate(angle);

            Vec2 offset = Dir * Speed * dTime;
            Move(offset);

            bool childUpdated = false;

            foreach ((_, Mesh mesh) in Children)
            {
                mesh.Rotate(angle);
                mesh.Move(offset);

                if (mesh.Update(dTime) != null)
                {
                    childUpdated = true;
                }
            }

            if (childUpdated)
            {
                return new VerticesUpdatePayload(GetOwnVertices());
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
