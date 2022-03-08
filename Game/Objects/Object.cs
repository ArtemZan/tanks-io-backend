using System.Collections.Generic;
using System;
using System.Linq;

using Box2DX.Dynamics;
using Box2DX.Common;

namespace TanksIO.Game.Objects
{
    //using Math;

    class Object
    {
        public readonly Mesh Mesh;

        public readonly Body Body;

        protected Dictionary<uint, Object> Children;
        public Object Parent;

        public Object(World world, Mesh mesh = null)
        {
            BodyDef bodyDef = new();
            Body = world.CreateBody(bodyDef);
            Body.SetLinearDamping(1);

            if (mesh == null)
            {
                Mesh = new();
            }
            else
            {
                Mesh = mesh;
                Mesh.CreateFixtures(Body);
                Body.SetMassFromShapes();
            }
        }

        public Object(Object obj)
        {
            Mesh = new(obj.Mesh);

            Children.EnsureCapacity(obj.Children.Count);
            foreach (var child in obj.Children)
            {
                Children.Add(child.Key, new Object(child.Value));
            }
        }

        public Vec2 Pos => Body.GetPosition();
        public double Rot => Body.GetAngle();
        public Vec2 Dir => new((float)System.Math.Cos(Rot), (float)System.Math.Sin(Rot));

        public void Add(Object mesh, uint id)
        {
            if (Children == null)
            {
                Children = new();
            }

            mesh.Parent = this;

            if (Children.ContainsKey(id))
            {
                Console.WriteLine("Warning: a mesh with such id already exists. It will be replaced with a new mesh");
            }

            Children.Add(id, mesh);
        }

        public void Add(Object mesh)
        {
            if (Children == null)
            {
                Children = new();
            }

            for (uint i = (uint)Children.Count; i < uint.MaxValue; i++)
            {
                if (!Children.ContainsKey(i))
                {
                    Add(mesh, i);
                    return;
                }
            }

            for (uint i = 0; i < (uint)Children.Count; i++)
            {
                if (!Children.ContainsKey(i))
                {
                    Add(mesh, i);
                    return;
                }
            }

            Console.WriteLine("Error: Mesh children number exceeds maximum allowed amount of " + uint.MaxValue + ". Child is not added");
        }

        public Object GetChildById(uint id)
        {
            Children.TryGetValue(id, out Object res);
            return res;
        }

        public delegate void ChildCallback(uint id, Object mesh);
        public void ForEachChild(ChildCallback callback)
        {
            if (Children == null)
            {
                return;
            }

            foreach ((uint id, Object child) in Children)
            {
                callback(id, child);
            }
        }

        public delegate void ChildCallbackIndexed(uint id, Object obj, int index);
        public void ForEachChild(ChildCallbackIndexed callback)
        {
            int i = 0;
            foreach ((uint id, Object child) in Children)
            {
                callback(id, child, i++);
            }
        }

        /// <summary>
        /// Adds vertices of a mesh to the vertices of this mesh
        /// </summary>
        protected void Merge(Object mesh)
        {
            this.Mesh.Combine(mesh.Mesh);
        }

        /// <summary>
        /// Combines this mesh and children into one mesh and returns it
        /// </summary>
        public Mesh GetCombinedMesh()
        {
            Mesh res = new(Mesh);
            ForEachChild((_, c, _) => res.Combine(c.Mesh));
            return res;
        }

        /*

        /// <summary>
        /// Transforms a range of vertices
        /// </summary>
        /// <param name="startIndex"> The first index of the vertices to be transformed </param>
        /// <param name="endIndex"> The FIRST index of the vertices after the startIndex NOT to be transformed </param>
        /// <param name="origin"> The origin of the transformation </param>
        /// <returns> This mesh </returns>
        protected Object Transform(Math.Mat2x3 transform, int startIndex, int endIndex, Vec2 origin = new())
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
        protected Object Transform(Math.Mat2 transform, int startIndex, int endIndex, Vec2 origin = new())
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
        public Object Transform(Mat2 transform, Vec2 origin = new())
        {
            return Transform(transform, 0, Vertices.Count, origin);
        }

        /// <summary>
        /// Transorfms the mesh and returns it
        /// </summary>
        public Object Transform(Mat2x3 transform, Vec2 origin = new())
        {
            return Transform(transform, 0, Vertices.Count, origin);
        }

        /// <summary>
        /// Moves the mesh with all children
        /// </summary>
        public Object Move(Vec2 offset)
        {
            MoveVertices(offset);

            _position += offset;

            return this;
        }

        /// <summary>
        /// Moves the mesh itself (without children)
        /// </summary>
        public Object MoveSelf(Vec2 offset)
        {
            MoveOwnVertices(offset);

            _position += offset;

            return this;
        }


        /// <summary>
        /// Moves the mesh itself (without children) without updating '_position' ('Pos')
        /// </summary>
        private void MoveOwnVertices(Vec2 offset)
        {
            for (int v = 0; v < Vertices.Count; v++)
            {
                Vertices[v] += offset;
            }
        }

        /// <summary>
        /// Moves the mesh with all children without updating '_position' ('Pos')
        /// </summary>
        private void MoveVertices(Vec2 offset)
        {
            MoveOwnVertices(offset);

            if (Children != null)
            {
                foreach ((_, Object mesh) in Children)
                {
                    mesh.MoveVertices(offset);
                }
            }
        }

        public Object Rotate(double angle, Vec2 origin)
        {
            RotateVertices(angle, origin);

            _rotation += angle;

            return this;
        }

        public Object Rotate(double angle)
        {
            return Rotate(angle, Pos + (Parent == null ? new() : Parent.Pos));
        }

        public Object RotateSelf(double angle, Vec2 origin)
        {
            RotateOwnVertices(angle, origin);

            _rotation += angle;

            return this;
        }

        private void RotateOwnVertices(double angle, Vec2 origin)
        {
            Mat2 mat = Mat2.Rotation(angle);

            for (int v = 0; v < Vertices.Count; v++)
            {
                Vertices[v] = mat * (Vertices[v] - origin) + origin;
            }
        }

        // To be optimised: the rotation matrix is created for every children
        private void RotateVertices(double angle, Vec2 origin)
        {
            RotateOwnVertices(angle, origin);
            
            if (Children != null)
            {
                foreach ((_, Object mesh) in Children)
                {
                    mesh.RotateVertices(angle, origin);
                }
            }
        }

        */

        /// <summary>
        /// Updates mesh and returns information about the update
        /// </summary>
        /// <param name="dTime">Time since last update in seconds. For the first update should be 0</param>
        /// <returns></returns>
        public virtual UpdatePayload Update(double dTime)
        {
            float rot = -Body.GetAngle() - Mesh.Rot;
            Vec2 offset = Body.GetPosition() - Mesh.Pos;

            Mesh.Rotate(rot);
            Mesh.Move(offset);

            Math.Mat2x3 trfm = new(
                Math.Mat2.Rotation(rot),
                offset
                );

            bool didUpdate = (rot > 1e-6 || rot < -1e-6) || offset.Length() > 1e-6;

            bool isSimpleTransform = true;

            ForEachChild((_, child) =>
            {
                UpdatePayload update = child.Update(dTime);

                if (update == null)
                {
                    // Child didn't move
                    if (didUpdate)
                    {
                        // But parent moved, thus not simple transform
                        isSimpleTransform = false;
                    }

                    return;
                }

                if (!update.IsTransform)
                {
                    isSimpleTransform = false;
                    return;
                }

                Math.Mat2x3 childTrfm = ((TransformUpdatePayload)update).Transform;

                if ((trfm.I - childTrfm.I).Length() > 1e-6 || (trfm.J - childTrfm.J).Length() > 1e-6 || (trfm.K - childTrfm.K).Length() > 1e-6)
                    isSimpleTransform = false;
            });

            if (isSimpleTransform)
            {
                if (didUpdate)
                {
                    return new TransformUpdatePayload(trfm, Body.GetPosition() - offset);
                }

                return null;
            }

            return new VerticesUpdatePayload(GetCombinedMesh());
        }
    }
}