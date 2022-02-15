namespace TanksIO.Game.Objects
{
    abstract class GameObject : Mesh
    {
        public readonly string Id;

        public GameObject(string id)
        {
            Id = id;
        }
    }
}
