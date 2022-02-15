using System.Collections.Generic;
using System;

namespace TanksIO.Game.Objects
{
    using Math;

    abstract class Mesh
    {
        protected List<Vec2> Vertices = new();

        public Mesh()
        {

        }

        protected void Add(Vec2 vertex)
        {
            Vertices.Add(vertex);
        }

        protected void Add(Mesh mesh)
        {
            Vertices.AddRange(mesh.Vertices);
        }

        public Vec2[] GetVertices() => Vertices.ToArray();

        /// <summary>
        /// Transforms a range of vertices
        /// </summary>
        /// <param name="startIndex"> The first index of the vertices to be transformed </param>
        /// <param name="endIndex"> The FIRST index of the vertices after the startIndex NOT to be transformed </param>
        /// <param name="origin"> The origin of the transformation </param>
        /// <returns> This mesh </returns>
        protected Mesh Transform(Mat2x3 transform, int startIndex, int endIndex, Vec2 origin = new())
        {
            if(startIndex < 0 || endIndex > Vertices.Count)
            {
                Console.WriteLine("Error: indices given to 'Transform' are out of range");
            }

            for (int v = startIndex; v < endIndex; v++)
            {
                Vertices[v] -= origin;
                Vertices[v] = transform * new Vec3(Vertices[v], 1);
                Vertices[v] -= origin;
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
                Vertices[v] -= origin;
            }

            return this;
        }

        public Mesh Transform(Mat2 transform, Vec2 origin = new())
        {
            return Transform(transform, 0, Vertices.Count, origin);
        }
        public Mesh Transform(Mat2x3 transform, Vec2 origin = new())
        {
            return Transform(transform, 0, Vertices.Count, origin);
        }

        public virtual UpdatePayload Update(double dTime) { return null; }
    }
}