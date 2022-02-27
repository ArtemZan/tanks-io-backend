using System.Collections.Generic;
using System;

namespace TanksIO.Game.Objects
{
    using Math;

    abstract class Mesh
    {
        protected List<Vec2> Vertices = new();
        private Vec2 _position = new();
        private double _rotation = 0;

        protected Dictionary<uint, Mesh> Children;
        public Mesh Parent;

        public Mesh()
        {

        }

        public Mesh(Mesh mesh)
        {
            Vertices = mesh.Vertices.GetRange(0, mesh.Vertices.Count);
        }

        public Vec2 Pos => _position;
        public double Rot => _rotation;
        public Vec2 Dir => new(System.Math.Cos(_rotation), System.Math.Sin(_rotation));

        protected void Add(Vec2 vertex)
        {
            Vertices.Add(vertex);
        }

        public void Add(Mesh mesh, uint id)
        {
            mesh.Parent = this;

            if(Children.ContainsKey(id))
            {
                Console.WriteLine("Warning: a mesh with such id already exists. It will be replaced with a new mesh");
            }

            Children.Add(id, mesh);
        }

        public void Add(Mesh mesh)
        {
            for (uint i = (uint)Children.Count; i < uint.MaxValue; i++)
            {
                if (!Children.ContainsKey(i))
                {
                    Add(mesh, i);
                    return;
                }
            }

            for(uint i = 0; i < (uint)Children.Count; i++)
            {
                if (!Children.ContainsKey(i))
                {
                    Add(mesh, i);
                    return;
                }
            }

            Console.WriteLine("Error: Mesh children number exceeds maximum allowed amount of " + uint.MaxValue + ". Child is not added");
        }

        public Mesh GetChildById(uint id)
        {
            Children.TryGetValue(id, out Mesh res);
            return res;
        }

        /// <summary>
        /// Adds vertices of a mesh to the vertices of this mesh
        /// </summary>
        protected void Merge(Mesh mesh)
        {
            Vertices.AddRange(mesh.Vertices);
        }

        public Vec2[] GetOwnVertices() => Vertices.ToArray();

        /// <summary>
        /// Transforms a range of vertices
        /// </summary>
        /// <param name="startIndex"> The first index of the vertices to be transformed </param>
        /// <param name="endIndex"> The FIRST index of the vertices after the startIndex NOT to be transformed </param>
        /// <param name="origin"> The origin of the transformation </param>
        /// <returns> This mesh </returns>
        protected Mesh Transform(Mat2x3 transform, int startIndex, int endIndex, Vec2 origin = new())
        {
            if (startIndex < 0 || endIndex > Vertices.Count)
            {
                Console.WriteLine("Error: indices given to 'Transform' are out of range");
            }

            for (int v = startIndex; v < endIndex; v++)
            {
                Vertices[v] -= origin;
                Vertices[v] = transform * new Vec3(Vertices[v], 1);
                Vertices[v] += origin;
            }

            return this;
        }

        /// <summary>
        /// Transforms a range of vertices
        /// </summary>
        /// <param name="startIndex"> The first index of the vertices to be transformed </param>
        /// <param name="endIndex"> The FIRST index of the vertices after the startIndex NOT to be transformed </param>
        /// <param name="origin"> The origin of the transformation </param>
        /// <returns> This mesh </returns>
        protected Mesh Transform(Mat2 transform, int startIndex, int endIndex, Vec2 origin = new())
        {
            if (startIndex < 0 || endIndex > Vertices.Count)
            {
                Console.WriteLine("Error: indices given to 'Transform' are out of range");
            }

            for (int v = startIndex; v < endIndex; v++)
            {
                Vertices[v] -= origin;
                Vertices[v] = transform * Vertices[v];
                Vertices[v] += origin;
            }

            return this;
        }

        /// <summary>
        /// Transorfms the mesh and returns it
        /// </summary> 
        public Mesh Transform(Mat2 transform, Vec2 origin = new())
        {
            return Transform(transform, 0, Vertices.Count, origin);
        }

        /// <summary>
        /// Transorfms the mesh and returns it
        /// </summary>
        public Mesh Transform(Mat2x3 transform, Vec2 origin = new())
        {
            return Transform(transform, 0, Vertices.Count, origin);
        }

        public Mesh Move(Vec2 offset)
        {
            for (int v = 0; v < Vertices.Count; v++)
            {
                Vertices[v] += offset;
            }

            _position += offset;

            return this;
        }

        public Mesh Rotate(double angle, Vec2 origin)
        {
            Mat2 mat = Mat2.Rotation(angle);

            for (int v = 0; v < Vertices.Count; v++)
            {
                Vertices[v] = mat * (Vertices[v] - origin) + origin;
            }

            return this;
        }

        public Mesh Rotate(double angle)
        {
            return Rotate(angle, Parent == null ? new() : Parent.Pos);
        }

        public virtual UpdatePayload Update(double dTime) { return null; }
    }
}