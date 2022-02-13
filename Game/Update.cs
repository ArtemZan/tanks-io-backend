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
    abstract record ObjectUpdate(string Id);

    record TransformUpdate : ObjectUpdate
    {
        Mat2x3 Transform;

        public TransformUpdate(string id, Mat2x3 transform)
            : base(id)
        {
            Transform = transform;
        }

        public override string ToString()
        {
            return new JSON().Set("id", Id, true).Set("trfm", Transform).ToString();
        }
    };

    record VerticesUpdate : ObjectUpdate
    {
        Vec2[] Vertices;

        public VerticesUpdate(string id, Vec2[] vertices)
            : base(id)
        {
            Vertices = vertices;
        }

        public override string ToString()
        {
            return new JSON().Set("id", Id, true).Set("vert", new JSON().PushAll(Vertices)).ToString();
        }
    };

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