using System.Collections.Generic;
using System.Linq;
using Box2DX.Common;

namespace TanksIO.Game
{
    using TanksIO.Game.Math;
    using TanksIO.Game.Objects;
    using Utils;

    struct PlayerUpdate
    {
        public Vec2 Dir;
        public Vec2 Pos;

        public PlayerUpdate(Vec2 dir, Vec2 pos)
        {
            Dir = dir;
            Pos = pos;
        }

        public override string ToString()
        {
            return new JSON().Set("dir", Dir.ToJSON()).Set("pos", Pos.ToJSON()).ToString();
        }
    }
    record ObjectUpdate(string Id, UpdatePayload payload)
    {
        public override string ToString()
        {
            return new JSON().Set("id", Id, true).Merge(payload.ToJSON()).ToString();
        }
    }

    abstract class UpdatePayload {
        public readonly bool IsTransform;

        public UpdatePayload(bool isTransform)
        {
            IsTransform = isTransform;
        }

        public abstract JSON ToJSON();

        public override string ToString()
        {
            return ToJSON().ToString();
        }
    }

    class TransformUpdatePayload : UpdatePayload
    {
        public Math.Mat2x3 Transform;
        public Vec2 Origin;

        public TransformUpdatePayload(Math.Mat2x3 transform, Vec2 origin)
            :base(true)
        {
            Transform = transform;
            Origin = origin;
        }

        public override JSON ToJSON()
        {
            return new JSON().Set("trfm", Transform).Set("or", Origin.ToJSON());
        }
    }

    class VerticesUpdatePayload : UpdatePayload
    {
        public Mesh Mesh;

        public VerticesUpdatePayload(Mesh mesh)
            :base(false)
        {
            Mesh = mesh;
        }

        public override JSON ToJSON()
        {
            return new JSON().Set("vert", new JSON().PushAll(Mesh.Vertices.Select(v => v.ToJSON()))).Set("ind", new JSON().PushAll(Mesh.Indices));
        }
    }

    class Update
    {
        public PlayerUpdate Player;
        public List<ObjectUpdate> Obj = new();

        public override string ToString()
        {
            return new JSON().Set("obj", new JSON().PushAll(Obj)).Set("player", Player).ToString();
        }
    }
}