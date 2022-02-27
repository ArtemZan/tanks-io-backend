namespace TanksIO.Game.Objects.Bullets
{
    using Shapes;
    class DefaultBullet : Bullet
    {
        public DefaultBullet(string emitterId)
            :base(emitterId)
        {
            Merge(new Rectangle(new(0.25, 0.7)));
        }
    }
}