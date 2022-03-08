using Box2DX.Dynamics;

namespace TanksIO.Game.Objects.Bullets
{
    using Shapes;
    class DefaultBullet : Bullet
    {
        public DefaultBullet(World world, string emitterId)
            :base(world, emitterId)
        {
            Body.SetLinearVelocity(Dir * 50);
            Mesh.Combine(new Rectangle(new(0.7f, 0.25f)));
        }
    }
}