namespace TanksIO.Game.Objects
{
    class GameObject : Mesh
    {
        public readonly string Id;

        public GameObject(string id)
        {
            Id = id;
        }
    }
}
