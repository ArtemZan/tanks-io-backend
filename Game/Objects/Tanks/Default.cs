namespace TanksIO.Game.Objects.Tanks
{
    using Shapes;
    class DefaultTank : Tank
    {
        public DefaultTank(string id)
            : base(new())
        {
            Add(new Rectangle(new(2, 5)));
            Add(new Rectangle(new(0.15, 3)).Transform(new(new(1, 0), new(0, 1), new(0, 2.4))));
        }
    }
}