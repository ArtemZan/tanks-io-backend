namespace TanksIO.Game
{
    using Math;
    using Objects.Tanks;

    class Player
    {
        readonly public string Id;
        public Vec2 Dir;
        public Vec2 Pos;

        public Tank Tank;

        public Player(string id)
        {
            Id = id;
        }

        public static bool operator ==(Player p1, Player p2)
        {
            return p1.Id == p2.Id;
        }

        public static bool operator !=(Player p1, Player p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            return this == (Player)obj;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}