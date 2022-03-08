using System.Collections.Generic;
using Box2DX.Dynamics;
using Box2DX.Common;
using System.Linq;

namespace TanksIO.Game.Objects
{
    //using Math;

    class Mesh
    {
        public readonly List<Vec2> Vertices = new();
        public readonly List<uint> Indices = new();

        private Vec2 _pos = new();
        private float _rot = 0;

        public Vec2 Pos => _pos;
        public float Rot => _rot;


        private List<FixtureDef> _fixtureDefs = new();

        public Mesh()
        {

        }

        public Mesh(Mesh mesh)
        {
            _fixtureDefs.AddRange(mesh._fixtureDefs);
            Vertices.AddRange(mesh.Vertices);
            Indices.AddRange(mesh.Indices);
        }

        public Mesh AddConvex(PolygonDef polygon)
        {
            int trianglesCount = polygon.VertexCount - 2;

            AddPolygonIndices(trianglesCount);

            Vertices.AddRange(polygon.Vertices.SkipLast(Settings.MaxPolygonVertices - polygon.VertexCount));

            _fixtureDefs.Add(polygon);

            return this;
        }

        public Mesh AddCircle(CircleDef circle)
        {
            const int vertCount = 16;
            int trianglesCount = vertCount - 2;

            AddPolygonIndices(trianglesCount);

            Vertices.Capacity += vertCount;
            for(int v = 0; v < vertCount; v++)
            {
                double rotation = (double)v / vertCount * 2 * System.Math.PI;
                Vertices.Add(new Vec2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation)) + circle.LocalPosition);
            }

            _fixtureDefs.Add(circle);

            return this;
        }

        public Mesh Combine(Mesh mesh)
        {
            Indices.AddRange(mesh.Indices.Select(i => (uint)(i + Vertices.Count)));
            Vertices.AddRange(mesh.Vertices);
            _fixtureDefs.AddRange(mesh._fixtureDefs);

            return this;
        }

        private void AddPolygonIndices(int trianglesCount)
        {
            int indicesOffset = Vertices.Count;

            Indices.Capacity += trianglesCount * 3;

            for (int i = 0; i < trianglesCount; i++)
            {
                Indices.Add((uint)indicesOffset); // The pivot point
                Indices.Add((uint)(indicesOffset + i + 1));
                Indices.Add((uint)(indicesOffset + i + 2));
            }
        }

        public void CreateFixtures(Body body)
        {
            foreach (FixtureDef fixtureDef in _fixtureDefs)
            {
                body.CreateFixture(fixtureDef).ComputeMass(out Box2DX.Collision.MassData massData);
            }
            body.SetMassFromShapes();

            _fixtureDefs.Clear();
        }

        public Mesh MoveTo(Vec2 pos)
        {
            return Move(pos - Pos);
        }

        public Mesh Move(Vec2 offset)
        {
            for (int v = 0; v < Vertices.Count; v++)
            {
                Vertices[v] += offset;
            }

            _pos += offset;

            return this;
        }

        public Mesh RotateTo(float rotation)
        {
            return Rotate(rotation - _rot);
        }

        public Mesh Rotate(float angle)
        {
            Mat22 trfm = new(angle);

            for(int v = 0; v < Vertices.Count; v++)
            {
                Vertices[v] = trfm.Solve(Vertices[v] - _pos) + _pos;
            }

            _rot += angle;

            return this;
        }

        public bool ValidateIndices()
        {
            foreach (uint i in Indices)
            {
                if (i >= Vertices.Count)
                    return false;
            }

            return true;
        }
    }
}
