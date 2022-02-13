using System.Collections.Generic;

namespace TanksIO.Game.Objects
{
    using Math;

    abstract class Mesh
    {
        private List<Vec2> _vertices = new();

        public Mesh()
        {
            
        }

        protected void Add(Vec2 vertex)
        {
            _vertices.Add(vertex);
        }

        protected void Add(Mesh mesh)
        {
            _vertices.AddRange(mesh._vertices);
        }

        public Vec2[] GetVertices() => _vertices.ToArray();

        public Mesh Transform(Mat2x3 transform)
        {
            for(int v = 0; v < _vertices.Count; v++)
            {
                _vertices[v] = transform * new Vec3(_vertices[v], 1);
            }

            return this;
        }
    }
}