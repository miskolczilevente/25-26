using System;
using System.Collections.Generic;
using System.Linq;
using Wild_West.Items;
using Wild_West.MapObjects;

namespace Wild_West.Characters
{
    public sealed class Bandit : Character
    {
        // A bandita aranyat cipelhet (felszedi a rögöt, nem hagyja a földön)
        public int CarryGold { get; private set; }

        public Bandit(int size)
        {
            DamageRange = (4, 15);
            Discovered = new bool[size, size];
        }

        public override void Step(City world, IList<Bandit> bandits, Sheriff sheriff)
        {
            // 70% eséllyel nem lép (lassabb)
            if (Rng.NextDouble() < 0.7) return;

            // Buta látótér: minden tickben újratöltjük a 3×3-at
            Array.Clear(Discovered, 0, Discovered.Length);
            foreach (var (vx, vy) in world.GetVisionSquare(X, Y, 1))
                Discovered[vx, vy] = true;

            // Ha aranyon áll, zsebre teszi
            if (world.Grid[X, Y] is GoldNugget)
            {
                CarryGold++;
                world.Grid[X, Y] = new Empty { X = X, Y = Y };
            }

            // Ha a seriff mellett áll, támad
            if (Math.Abs(sheriff.X - X) + Math.Abs(sheriff.Y - Y) == 1)
            {
                Combat(sheriff);
                return;
            }

            // Ha látja a seriffet a 3×3-ban, követi
            bool seesSheriff = Math.Abs(sheriff.X - X) <= 1 && Math.Abs(sheriff.Y - Y) <= 1;
            if (seesSheriff)
            {
                MoveTowards(world, (sheriff.X, sheriff.Y));
                return;
            }

            // Különben csatangol
            MoveRandom(world);
        }

        private void MoveTowards(City world, (int x, int y) target)
        {
            // Egylépéses közelítés – egyszerűsített; opcionálisan használhatod a City BFS-ét is
            var (tx, ty) = target;
            int bestX = X, bestY = Y, bestDist = Dist(X, Y, tx, ty);

            foreach (var (nx, ny) in Neighbours4(X, Y))
            {
                if (!world.IsWalkable(nx, ny)) continue;
                int d = Dist(nx, ny, tx, ty);
                if (d < bestDist) { bestDist = d; bestX = nx; bestY = ny; }
            }
            X = bestX; Y = bestY;
        }

        private static int Dist(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

        private void MoveRandom(City world)
        {
            var dirs = Neighbours4(X, Y).Where(p => world.IsWalkable(p.x, p.y)).ToList();
            if (dirs.Count == 0) return;
            var (nx, ny) = dirs[Rng.Next(dirs.Count)];
            X = nx; Y = ny;
        }
    }
}
