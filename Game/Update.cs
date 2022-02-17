using System.Collections.Generic;
using System.Linq;

namespace TanksIO.Game
{
    using Math;
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
            return new JSON().Set("dir", Dir).Set("pos", Pos).ToString();
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
        public abstract JSON ToJSON();

        public override string ToString()
        {
            return ToJSON().ToString();
        }
    }

    class TransformUpdatePayload : UpdatePayload
    {
        public Mat2x3 Transform;
        public Vec2 Origin;

        public TransformUpdatePayload(Mat2x3 transform, Vec2 origin)
        {
            Transform = transform;
            Origin = origin;
        }

        public override JSON ToJSON()
        {
            return new JSON().Set("trfm", Transform).Set("or", Origin);
        }
    }

    class VerticesUpdatePayload : UpdatePayload
    {
        public Vec2[] Vertices;

        public VerticesUpdatePayload(Vec2[] vertices)
        {
            Vertices = vertices;
        }

        public override JSON ToJSON()
        {
            return new JSON().Set("vert", new JSON().PushAll(Vertices));
        }
    }

    class Update
    {
        public PlayerUpdate Player;
        public List<ObjectUpdate> Obj = new();

        public override string ToString()
        {
            return new JSON().Set("player", Player).Set("obj", new JSON().PushAll(Obj)).ToString();
        }
    }
}