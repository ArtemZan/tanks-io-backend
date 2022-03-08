using Box2DX.Dynamics;

namespace TanksIO.Game.Objects.Bullets
{
    abstract class Bullet : Object
    {
        public string EmitterId;

        public Bullet(World world, string emitterId)
            :base(world)
        {
            EmitterId = emitterId;
            Body.SetBullet(true);
        }
    }
}