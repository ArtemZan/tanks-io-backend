namespace TanksIO.Game.Objects.Bullets
{
    abstract class Bullet : DynamicMesh
    {
        public string EmitterId;

        public Bullet(string emitterId)
        {
            EmitterId = emitterId;
        }
    }
}