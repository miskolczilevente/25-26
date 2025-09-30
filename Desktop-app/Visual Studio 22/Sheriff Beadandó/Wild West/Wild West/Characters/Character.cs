using System;
using System.Collections.Generic;

namespace Wild_West.Characters
{
    public abstract class Character : TownComponent
    {
        protected static readonly Random Rng = new Random();

        public int HP { get; protected set; } = 100;
        public (int min, int max) DamageRange { get; protected set; }
        public bool[,] Discovered { get; protected set; } = default!; // inicializálják a leszármazottak

        public abstract void Step(City world, IList<Bandit> bandits, Sheriff sheriff);

        public virtual void Combat(Character other)
        {
            int dmg = Rng.Next(DamageRange.min, DamageRange.max + 1);
            other.HP = Math.Max(0, other.HP - dmg);
        }

        protected static IEnumerable<(int x, int y)> Neighbours4(int x, int y)
        {
            yield return (x + 1, y);
            yield return (x - 1, y);
            yield return (x, y + 1);
            yield return (x, y - 1);
        }
    }
}
