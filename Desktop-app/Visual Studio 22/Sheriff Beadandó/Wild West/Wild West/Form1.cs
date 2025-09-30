using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Wild_West.MapObjects;
using Wild_West.Items;

namespace Wild_West
{
    public partial class Form1 : Form
    {
        private readonly GameHandler game = new GameHandler();
        private readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        // megjelen�t�s
        private const int Cell = 22;
        private const int MarginLeft = 10;
        private const int MarginTop = 10;

        // l�t�t�r kiemel�s (sz�nek/penek)
        private readonly Brush visionOverlay = new SolidBrush(Color.FromArgb(70, 100, 149, 237)); // �tl�tsz� k�k (CornflowerBlue-hoz ill�)
        private readonly Pen visionBorder = new Pen(Color.FromArgb(160, 100, 149, 237), 2f);

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            timer.Interval = 120;
            timer.Tick += (s, e) => { game.Tick(); Invalidate(); };
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            var city = game.City;

            // --- 1) R�cs �s csemp�k ---
            for (int y = 0; y < City.Size; y++)
            {
                for (int x = 0; x < City.Size; x++)
                {
                    bool known = city.Sheriff.Discovered[x, y];
                    var rect = CellRect(x, y);

                    // alap h�tt�r: felfedezett vs ismeretlen
                    g.FillRectangle(known ? Brushes.Beige : Brushes.DarkSlateGray, rect);

                    var comp = city.Grid[x, y];

                    // fal/barrik�d
                    if (comp is Barricade) g.FillRectangle(Brushes.SaddleBrown, rect);

                    // townhall
                    else if (comp is TownHall th) g.FillRectangle(th.Active ? Brushes.Gold : Brushes.Gray, rect);

                    // itemek csak, ha a mez� felfedezett
                    else if (known && comp is GoldNugget) g.FillRectangle(Brushes.Yellow, rect);
                    else if (known && comp is Whiskey) g.FillRectangle(Brushes.Orange, rect);

                    // r�csvonal
                    g.DrawRectangle(Pens.Black, rect);
                }
            }

            // --- 2) L�t�t�r overlay (3�3) + keret ---
            DrawSheriffVision(g);

            // --- 3) �gensek ---
            DrawAgent(g, city.Sheriff.X, city.Sheriff.Y, Brushes.CornflowerBlue);
            foreach (var b in city.Bandits)
                DrawAgent(g, b.X, b.Y, Brushes.Firebrick);

            // --- 4) St�tuszsor ---
            g.DrawString(game.StatusLine, Font, Brushes.White, MarginLeft, MarginTop + City.Size * Cell + 8);
        }

        private void DrawSheriffVision(Graphics g)
        {
            var city = game.City;
            int sx = city.Sheriff.X;
            int sy = city.Sheriff.Y;

            // 3�3-on bel�li mez�k f�lig �ttetsz� k�k overlay
            for (int y = Math.Max(0, sy - 1); y <= Math.Min(City.Size - 1, sy + 1); y++)
            {
                for (int x = Math.Max(0, sx - 1); x <= Math.Min(City.Size - 1, sx + 1); x++)
                {
                    var r = CellRect(x, y);
                    g.FillRectangle(visionOverlay, r);
                }
            }

            // K�ls� keret a teljes 3�3 blokk k�r� (ha a p�lya sz�le miatt kisebb, ahhoz igaz�tjuk)
            var topLeft = CellRect(Math.Max(0, sx - 1), Math.Max(0, sy - 1));
            var bottomRight = CellRect(Math.Min(City.Size - 1, sx + 1), Math.Min(City.Size - 1, sy + 1));

            var outerRect = Rectangle.FromLTRB(
                topLeft.Left,
                topLeft.Top,
                bottomRight.Right,
                bottomRight.Bottom
            );

            g.DrawRectangle(visionBorder, outerRect);
        }

        private Rectangle CellRect(int x, int y)
        {
            return new Rectangle(MarginLeft + x * Cell, MarginTop + y * Cell, Cell - 1, Cell - 1);
        }

        private void DrawAgent(Graphics g, int x, int y, Brush brush)
        {
            var rect = new Rectangle(
                MarginLeft + x * Cell + 4,
                MarginTop + y * Cell + 4,
                Cell - 9,
                Cell - 9
            );
            g.FillEllipse(brush, rect);
            g.DrawEllipse(Pens.Black, rect);
        }
    }
}
