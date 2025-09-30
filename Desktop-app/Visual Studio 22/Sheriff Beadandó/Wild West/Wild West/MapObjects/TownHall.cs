namespace Wild_West
{
    public sealed class TownHall : MapObject
    {
        public bool Active { get; set; } = false;
        public override bool Walkable => Active; // csak aktívan átjárható
    }
}
