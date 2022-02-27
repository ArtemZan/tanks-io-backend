namespace TanksIO.Game.Objects
{
    abstract class GameObject : Mesh
    {
        public readonly string Id;

        public GameObject(string id)
        {
            Id = id;
        }

        public GameObject(string id, Mesh mesh)
            :base(mesh)
        {
            Id = id;
        }
    }
}
