namespace TanksIO.Game.Objects.Bullets
{
    abstract class Bullet : Mesh
    {
        public string EmitterId;

        public Bullet(string emitterId)
        {
            EmitterId = emitterId;
        }
    }
}