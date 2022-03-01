namespace TanksIO.Game.Objects.Bullets
{
    using Shapes;
    class DefaultBullet : Bullet
    {
        public DefaultBullet(string emitterId)
            :base(emitterId)
        {
            Speed = 50;
            Merge(new Rectangle(new(0.7, 0.25)));
        }
    }
}