using System;
using System.Collections.Generic;
using System.Linq;
using Wild_West.Items;
using Wild_West.MapObjects;

namespace Wild_West.Characters
{
    public sealed class Sheriff : Character
    {
        public int GoldCount { get; private set; }
        public List<(int x, int y)> KnownGolds { get; } = new();

        // Bandita-emlékezet: utolsó ismert pozíció + mikor látta
        // A World.Tick alapján frissítjük és időkorlát után kidobjuk.
        public List<(int x, int y, int seenTick)> KnownBandits { get; } = new();

        public bool IsFleeing => HP <= 40;

        public bool[,] GetMyDiscoveredMap() => Discovered;


        public Sheriff(int size)
        {
            DamageRange = (20, 35);
            Discovered = new bool[size, size];
        }

        public override void Step(City world, IList<Bandit> bandits, Sheriff _)
        {
            // 1) Látókör frissítés + arany/ bandita emlékek
            foreach (var (vx, vy) in world.GetVisionSquare(X, Y, 1))
            {
                Discovered[vx, vy] = true;

                // arany megjegyzése
                if (world.Grid[vx, vy] is GoldNugget && !KnownGolds.Contains((vx, vy)))
                    KnownGolds.Add((vx, vy));

                // banditák megjegyzése (utolsó ismert pozíció + tick)
                if (bandits.Any(b => b.X == vx && b.Y == vy))
                {
                    int idx = KnownBandits.FindIndex(k => k.x == vx && k.y == vy);
                    if (idx >= 0) KnownBandits[idx] = (vx, vy, world.Tick);
                    else KnownBandits.Add((vx, vy, world.Tick));
                }
            }

            // lejárt bandita-nyomok törlése (pl. 60 tick után)
            KnownBandits.RemoveAll(k => world.Tick - k.seenTick > 60);

            // 2) Pickup
            if (world.Grid[X, Y] is Item it) Pickup(world, it);

            // 3/a) Ha bármelyik bandita LÁT (3×3), akkor ne folytassuk a normál célokat.
            // Ha nagy a rizikó (kevés HP), meneküljünk whiskey felé, különben csak "készüljünk" (nem váltunk új célra).
            bool beingChased = bandits.Any(b => Math.Abs(b.X - X) <= 1 && Math.Abs(b.Y - Y) <= 1);
            if (beingChased && !bandits.Any(b => Math.Abs(b.X - X) + Math.Abs(b.Y - Y) == 1))
            {
                if (IsFleeing)
                {
                    MoveTowards(world, world.FindNearestKnown<Whiskey>(this) ?? world.FindAnySafe(this));
                    return;
                }
                // ha nem kritikus a HP, itt egyszerűen nem váltunk új célra (folytatódik a későbbi célprioritás),
                // de a "harc" eset (szomszédos bandita) úgyis azonnal kezelve lesz lejjebb.
            }


            // 3) Harc – ha mellettünk bandita van
            var adjacentBandits = bandits.Where(b => Math.Abs(b.X - X) + Math.Abs(b.Y - Y) == 1).ToList();
            if (adjacentBandits.Count > 0)
            {
                if (IsFleeing)
                {
                    MoveTowards(world, world.FindNearestKnown<Whiskey>(this) ?? world.FindAnySafe(this));
                }
                else
                {
                    // Üsd az elsőt (egyszerre egyet ütünk)
                    Combat(adjacentBandits[0]);
                }
                return;
            }

            // *** ÚJ: ha nincs ismert arany és a pályán sincs már lerakott arany,
            // akkor a hiányzó arany banditánál van -> vadászat ***
            if (GoldCount < City.RequiredGold && KnownGolds.Count == 0 && !world.IsAnyGoldOnGround())
            {
                var visible = bandits.FirstOrDefault(b => Math.Abs(b.X - X) <= 1 && Math.Abs(b.Y - Y) <= 1);
                if (visible != null)
                {
                    MoveTowards(world, (visible.X, visible.Y));
                    return;
                }

                if (KnownBandits.Count > 0)
                {
                    var target = KnownBandits.OrderByDescending(k => k.seenTick).First();
                    MoveTowards(world, (target.x, target.y));
                    return;
                }

                var huntExplore = world.FindNearestUnknown(this);
                MoveTowards(world, huntExplore);
                return;
            }


            // 4) Célok prioritása
            // 4/a) Ha megvan az összes arany és a városháza aktív -> kijárat
            if (GoldCount >= City.RequiredGold && world.TownHall.Active)
            {
                MoveTowards(world, (world.TownHall.X, world.TownHall.Y));
                return;
            }

            // 4/b) Van ismert arany -> oda
            if (KnownGolds.Count > 0)
            {
                MoveTowards(world, KnownGolds[0]);
                return;
            }

            // 4/c) Ha mindent feltártunk és nincs arany, menjünk a legfrissebb bandita-nyomra
            if (FullyDiscovered(world) && KnownBandits.Count > 0)
            {
                var target = KnownBandits.OrderByDescending(k => k.seenTick).First();
                MoveTowards(world, (target.x, target.y));
                return;
            }

            // 4/d) Alap: felfedezés (legközelebbi ismeretlen)
            var unknown = world.FindNearestUnknown(this);
            MoveTowards(world, unknown);
        }

        public void Pickup(City world, Item item)
        {
            if (item is GoldNugget)
            {
                AddGold(world, 1);
                world.Grid[X, Y] = new Empty { X = X, Y = Y };
                KnownGolds.RemoveAll(p => p.x == X && p.y == Y);
            }
            else if (item is Whiskey)
            {
                HP = Math.Min(100, HP + 50);
                world.Grid[X, Y] = new Empty { X = X, Y = Y };
                world.RespawnWhiskey();
            }
        }

        // Loot vagy pickup esetén egységes kezelés
        public void AddGold(City world, int amount)
        {
            GoldCount += amount;
            if (GoldCount >= City.RequiredGold)
                world.TownHall.Active = true;
        }

        // --- Mozgás BFS-sel + anti-stuck ---
        private int stuckTicks = 0;
        private int lastX, lastY;

        private void MoveTowards(City world, (int x, int y)? target)
        {
            if (lastX == X && lastY == Y) stuckTicks++; else stuckTicks = 0;
            lastX = X; lastY = Y;

            if (target == null) { MoveRandom(world); return; }
            var (tx, ty) = target.Value;
            if (tx == X && ty == Y) return;

            if (world.TryGetNextStepTowards(X, Y, tx, ty, out var step))
            {
                X = step.x; Y = step.y;
            }
            else
            {
                MoveRandom(world);
            }

            if (stuckTicks >= 20)
            {
                MoveRandom(world);
                stuckTicks = 0;
            }
        }

        private static int Dist(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

        private void MoveRandom(City world)
        {
            var dirs = Neighbours4(X, Y).Where(p => world.IsWalkable(p.x, p.y)).ToList();
            if (dirs.Count == 0) return;
            var (nx, ny) = dirs[Rng.Next(dirs.Count)];
            X = nx; Y = ny;
        }

        private bool FullyDiscovered(City world)
        {
            for (int y = 0; y < City.Size; y++)
                for (int x = 0; x < City.Size; x++)
                    if (!Discovered[x, y]) return false;
            return true;
        }
    }
}
