using System;
using System.Collections.Generic;
using System.Linq;
using Wild_West.Characters;
using Wild_West.Items;
using Wild_West.MapObjects;

namespace Wild_West
{
    public sealed class City
    {
        public const int Size = 25;
        public const int RequiredGold = 5;

        public TownComponent[,] Grid { get; } = new TownComponent[Size, Size];

        public Sheriff Sheriff { get; private set; } = null!;
        public List<Bandit> Bandits { get; } = new();
        public TownHall TownHall { get; private set; } = null!;

        // globális tick a memóriákhoz (meglévő logikához)
        public int Tick { get; internal set; }

        private static readonly Random Rng = new Random();

        public City()
        {
            for (int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                    Grid[x, y] = new Empty { X = x, Y = y };
        }

        public void Generate()
        {
            PlaceRandomWalkableEmpty(out int sx, out int sy);
            Sheriff = new Sheriff(Size) { X = sx, Y = sy };

            PlaceRandomWalkableEmpty(out int hx, out int hy);
            TownHall = new TownHall { X = hx, Y = hy };
            Grid[hx, hy] = TownHall;

            for (int i = 0; i < RequiredGold; i++)
            {
                PlaceRandomEmpty(out int gx, out int gy);
                Grid[gx, gy] = new GoldNugget { X = gx, Y = gy };
            }

            for (int i = 0; i < 3; i++)
            {
                PlaceRandomEmpty(out int wx, out int wy);
                Grid[wx, wy] = new Whiskey { X = wx, Y = wy };
            }

            // Barricade-ok (nyugodtan finomhangolhatod a számot)
            for (int i = 0; i < 40; i++)
            {
                PlaceRandomEmpty(out int bx, out int by);
                Grid[bx, by] = new Barricade { X = bx, Y = by };
            }

            // Banditák
            for (int i = 0; i < 4; i++)
            {
                PlaceRandomWalkableEmpty(out int bx, out int by);
                Bandits.Add(new Bandit(Size) { X = bx, Y = by });
            }

            // --- ÚJ: okos konnektivitás-garancia „faragással” ---
            EnsureConnectivityEnhanced();
        }

        // =====================================================================
        // KONNEKTIVITÁS FARAGÁSSAL (ESSZENCIÁLIS CÉLOKHOZ ÚTVONAL VÁGÁSA)
        // =====================================================================

        /// <summary>
        /// Garancia: seriff eléri az ÖSSZES GoldNugget-et, a TownHall-t, és MINDEN banditát.
        /// Ha valamelyik cél nem elérhető, a legkevesebb barikád eltávolításával „folyosót vágunk”.
        /// </summary>
        private void EnsureConnectivityEnhanced()
        {
            // 1) célok listázása
            var golds = PositionsOf<GoldNugget>().ToList();
            var bandits = Bandits.Select(b => (b.X, b.Y)).ToList();
            var hall = (TownHall.X, TownHall.Y);

            // 2) faragás a célok felé
            // Aranyrögök
            foreach (var g in golds)
                EnsureReachableByCarving((Sheriff.X, Sheriff.Y), g);

            // TownHall (még inaktív is elérhető legyen)
            EnsureReachableByCarving((Sheriff.X, Sheriff.Y), hall);

            // Banditák (hogy „ne legyenek elzárva” a seriffről)
            foreach (var b in bandits)
                EnsureReachableByCarving((Sheriff.X, Sheriff.Y), b);
        }

        /// <summary>
        /// Ha a cél nem elérhető a starttól, kiválasztjuk azt az útvonalat, ami a
        /// LEGLKEVESEBB barikád eltávolításával elérhetővé teszi, és azokat a barikádokat eltávolítjuk.
        /// </summary>
        private void EnsureReachableByCarving((int sx, int sy) start, (int tx, int ty) target)
        {
            if (IsReachable(start, target)) return;

            var carved = CarvePathTo(start, target);
            // ha carved = 0, akkor nem találtunk semmilyen utat (nagyon extrém), de ez 25x25-nél gyakorlatilag nem fordul elő.
        }

        /// <summary>
        /// Visszaadja, hogy elérhető-e a target a starttól CSAK Walkable mezőkön (Barricade és inaktív Hall fal).
        /// </summary>
        private bool IsReachable((int x, int y) start, (int x, int y) target)
        {
            var (sx, sy) = start;
            var (tx, ty) = target;
            if (!IsInside(tx, ty)) return false;

            var seen = new bool[Size, Size];
            var q = new Queue<(int x, int y)>();
            q.Enqueue((sx, sy));
            seen[sx, sy] = true;

            while (q.Count > 0)
            {
                var (x, y) = q.Dequeue();
                if (x == tx && y == ty) return true;

                foreach (var (nx, ny) in Neigh4(x, y))
                {
                    if (!IsWalkable(nx, ny) || seen[nx, ny]) continue;
                    seen[nx, ny] = true;
                    q.Enqueue((nx, ny));
                }
            }
            return false;
        }

        /// <summary>
        /// „Legolcsóbb” (legkevesebb barikád) útvonalat keres a start → target között.
        /// Empty mező költsége 0, Barricade költsége 1. A visszavezetett útvonalon
        /// MINDEN barikádot Empty-vé alakít. Visszatér: hány barikádot távolított el.
        /// </summary>
        private int CarvePathTo((int x, int y) start, (int x, int y) target)
        {
            var (sx, sy) = start;
            var (tx, ty) = target;

            // 0–1 BFS (deque), mert csak 0 és 1 költségű élek vannak
            var dist = new int[Size, Size];
            var prev = new (int x, int y)?[Size, Size];
            for (int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                    dist[x, y] = int.MaxValue;

            var dq = new LinkedList<(int x, int y)>();
            dist[sx, sy] = 0;
            dq.AddFirst((sx, sy));

            while (dq.Count > 0)
            {
                var (x, y) = dq.First.Value;
                dq.RemoveFirst();

                if (x == tx && y == ty) break;

                foreach (var (nx, ny) in Neigh4(x, y))
                {
                    if (!IsInside(nx, ny)) continue;

                    int w = (Grid[nx, ny] is Barricade || (Grid[nx, ny] is TownHall th && !th.Active)) ? 1 : 0;
                    int nd = dist[x, y] + w;

                    if (nd < dist[nx, ny])
                    {
                        dist[nx, ny] = nd;
                        prev[nx, ny] = (x, y);
                        if (w == 0) dq.AddFirst((nx, ny)); else dq.AddLast((nx, ny));
                    }
                }
            }

            if (dist[tx, ty] == int.MaxValue)
                return 0; // nincs út sem faragással (nagyon valószínűtlen)

            // visszafejtés és faragás
            int removed = 0;
            var cur = (tx, ty);
            while (prev[cur.tx, cur.ty] != null)
            {
                var p = prev[cur.tx, cur.ty]!.Value;
                // ha a cur mező barikád vagy inaktív Hall, „átjárhatóvá” tesszük
                if (Grid[cur.tx, cur.ty] is Barricade)
                {
                    Grid[cur.tx, cur.ty] = new Empty { X = cur.tx, Y = cur.ty };
                    removed++;
                }
                else if (Grid[cur.tx, cur.ty] is TownHall hall && !hall.Active)
                {
                    // TownHall-at nem aktiváljuk itt; csak megengedtük az „útként” – ide nem nyúlunk.
                }

                cur = p;
            }
            return removed;
        }

        private IEnumerable<(int x, int y)> PositionsOf<T>() where T : TownComponent
        {
            for (int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                    if (Grid[x, y] is T) yield return (x, y);
        }

        // =====================================================================
        // KÖZMŰVEK / SEGÉDEK
        // =====================================================================

        public bool IsInside(int x, int y) => x >= 0 && y >= 0 && x < Size && y < Size;
        public bool IsWalkable(int x, int y) => IsInside(x, y) && Grid[x, y].Walkable;

        public IEnumerable<(int x, int y)> GetVisionSquare(int cx, int cy, int r)
        {
            for (int y = Math.Max(0, cy - r); y <= Math.Min(Size - 1, cy + r); y++)
                for (int x = Math.Max(0, cx - r); x <= Math.Min(Size - 1, cx + r); x++)
                    yield return (x, y);
        }

        public void RespawnWhiskey()
        {
            PlaceRandomEmpty(out int x, out int y);
            Grid[x, y] = new Whiskey { X = x, Y = y };
        }

        public (int x, int y)? FindNearestUnknown(Characters.Sheriff s)
        {
            var q = new Queue<(int x, int y)>();
            var seen = new bool[Size, Size];
            q.Enqueue((s.X, s.Y));
            seen[s.X, s.Y] = true;

            while (q.Count > 0)
            {
                var (x, y) = q.Dequeue();
                if (!s.Discovered[x, y]) return (x, y);

                foreach (var (nx, ny) in Neigh4(x, y))
                {
                    if (!IsWalkable(nx, ny) || seen[nx, ny]) continue;
                    seen[nx, ny] = true; q.Enqueue((nx, ny));
                }
            }
            return null;
        }

        public (int x, int y)? FindNearestKnown<T>(Characters.Sheriff s) where T : TownComponent
        {
            (int x, int y)? best = null;
            int bestD = int.MaxValue;

            for (int y = 0; y < Size; y++)
                for (int x = 0; x < Size; x++)
                    if (s.Discovered[x, y] && Grid[x, y] is T)
                    {
                        int d = Dist(s.X, s.Y, x, y);
                        if (d < bestD) { bestD = d; best = (x, y); }
                    }
            return best;
        }

        public (int x, int y) FindAnySafe(Characters.Sheriff s) => (s.X, s.Y);

        public IEnumerable<Characters.Bandit> AdjacentBanditsToSheriff()
        {
            foreach (var b in Bandits)
                if (Math.Abs(b.X - Sheriff.X) + Math.Abs(b.Y - Sheriff.Y) == 1)
                    yield return b;
        }

        public bool TryGetNextStepTowards(int sx, int sy, int tx, int ty, out (int x, int y) next)
        {
            next = (sx, sy);
            if (sx == tx && sy == ty) return false;

            var prev = new (int x, int y)?[Size, Size];
            var q = new Queue<(int x, int y)>();
            var seen = new bool[Size, Size];

            q.Enqueue((sx, sy));
            seen[sx, sy] = true;

            while (q.Count > 0)
            {
                var (x, y) = q.Dequeue();
                foreach (var (nx, ny) in Neigh4(x, y))
                {
                    if (!IsWalkable(nx, ny) || seen[nx, ny]) continue;
                    seen[nx, ny] = true;
                    prev[nx, ny] = (x, y);

                    if (nx == tx && ny == ty)
                    {
                        var cur = (nx, ny);
                        while (prev[cur.nx, cur.ny] != null && prev[cur.nx, cur.ny]!.Value != (sx, sy))
                            cur = prev[cur.nx, cur.ny]!.Value;

                        next = cur;
                        return true;
                    }
                    q.Enqueue((nx, ny));
                }
            }
            return false;
        }

        private static int Dist(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

        private IEnumerable<(int x, int y)> Neigh4(int x, int y)
        {
            yield return (x + 1, y);
            yield return (x - 1, y);
            yield return (x, y + 1);
            yield return (x, y - 1);
        }

        private void PlaceRandomEmpty(out int x, out int y)
        {
            do { x = Rng.Next(Size); y = Rng.Next(Size); }
            while (!(Grid[x, y] is Empty));
        }

        private void PlaceRandomWalkableEmpty(out int x, out int y)
        {
            do { x = Rng.Next(Size); y = Rng.Next(Size); }
            while (!(Grid[x, y] is Empty) || !Grid[x, y].Walkable);
        }
    }
    public sealed class GameHandler
    {
        public City City { get; } = new City();
        public bool GameOver { get; private set; }
        public string StatusLine { get; private set; } = "";

        public GameHandler() => City.Generate();

        public void Tick()
        {
            if (GameOver) return;

            City.Tick++; // globális tick

            // Sheriff lép
            City.Sheriff.Step(City, City.Bandits, City.Sheriff);

            // Banditák lépnek, és halál/loot kezelés
            foreach (var b in City.Bandits.ToList())
            {
                b.Step(City, City.Bandits, City.Sheriff);

                if (b.HP <= 0)
                {
                    if (b.CarryGold > 0)
                    {
                        City.Sheriff.AddGold(City, b.CarryGold);
                    }
                    City.Bandits.Remove(b);
                }
            }

            // Vége feltételek
            if (City.Sheriff.HP <= 0)
            {
                GameOver = true; StatusLine = "A seriff elesett. GAME OVER."; return;
            }

            if (City.TownHall.Active &&
                City.Sheriff.X == City.TownHall.X && City.Sheriff.Y == City.TownHall.Y)
            {
                GameOver = true; StatusLine = "A seriff kijutott! GYŐZELEM!"; return;
            }

            // státusz
            StatusLine = $"HP: {City.Sheriff.HP} | Arany: {City.Sheriff.GoldCount}/{City.RequiredGold} | Banditák: {City.Bandits.Count}";
            var attackers = City.AdjacentBanditsToSheriff().ToList();
            if (attackers.Count > 0)
            {
                var hpList = string.Join(", ", attackers.Select(a => a.HP));
                StatusLine += $" | Támadó banditák HP: [{hpList}]";
            }
        }
    }

}
