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
        private GameHandler game = new GameHandler();
        private readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        // megjelen�t�s
        private const int Cell = 22;
        private const int MarginLeft = 10;
        private const int MarginTop = 10;

        // l�t�t�r kiemel�s (sz�nek/penek)
        private readonly Brush visionOverlay = new SolidBrush(Color.FromArgb(70, 100, 149, 237)); // �tl�tsz� k�k (CornflowerBlue-hoz ill�)
        private readonly Pen visionBorder = new Pen(Color.FromArgb(160, 100, 149, 237), 2f);

        private Button startButton;
        private Button resetButton;


        public Form1()
        {
            InitializeComponent();

            // Fix ablakm�ret EL�SZ�R, hogy a gombok k�z�pre ker�ljenek
            ClientSize = new Size(570, 670);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = true;
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true; 
            Text = "Wild West";

            // Start gomb
            startButton = new Button
            {
                Text = "Start",
                Size = new Size(100, 40),
                Location = new Point((ClientSize.Width - 100) / 2, (ClientSize.Height - 40) / 2),
                Visible = true
            };
            startButton.Click += (s, e) =>
            {
                game = new GameHandler();   // itt j�n l�tre a j�t�k
                resetButton.Visible = false;
                timer.Start();              // itt indul a szimul�ci�
                startButton.Visible = false;
                Invalidate();
            };
            Controls.Add(startButton);

            // Reset gomb (csak game over ut�n l�tsz�djon)
            resetButton = new Button
            {
                Text = "Reset",
                Size = new Size(100, 40),
                Location = new Point((ClientSize.Width - 100) / 2, (ClientSize.Height - 40) / 2 + 60),
                Visible = false
            };
            resetButton.Click += (s, e) =>
            {
                game = new GameHandler();   // �j v�ros/j�t�k
                timer.Start();              // �jraind�tjuk az �r�t
                resetButton.Visible = false;
                Invalidate();
            };
            Controls.Add(resetButton);

            // Timer: NEM ind�tjuk el itt!
            timer.Interval = 120;
            timer.Tick += (s, e) =>
            {
                if (game == null) return;     // Start el�tt ne fusson
                game.Tick();
                if (game.GameOver)
                {
                    timer.Stop();             // �ll�tsuk le, hogy ne ketyegjen tov�bb
                    resetButton.Visible = true;
                }
                Invalidate();
            };
            // timer.Start();  // <-- EZT HAGYD KI!
        }


        private void GameOverReset(Button resetBtn)
        {
            resetBtn.Visible = false;
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

                    // st�tusz szekci� fekete sz�nnel
                    int statusY = MarginTop + City.Size * Cell + 8;
                    g.DrawString($"Sheriff HP: {game.City.Sheriff.HP}", Font, Brushes.Black, MarginLeft, statusY);
                    statusY += 20;
                    g.DrawString($"Arany: {game.City.Sheriff.GoldCount}/{City.RequiredGold}", Font, Brushes.Black, MarginLeft, statusY);
                    statusY += 20;
                    g.DrawString($"Bandit�k sz�ma: {game.City.Bandits.Count}", Font, Brushes.Black, MarginLeft, statusY);

                    // t�mad� bandit�k HP list�z�sa
                    var attackers = game.City.AdjacentBanditsToSheriff().ToList();
                    if (attackers.Count > 0)
                    {
                        statusY += 20;
                        var hpList = string.Join(", ", attackers.Select(a => a.HP));
                        g.DrawString($"T�mad� bandit�k HP: {hpList}", Font, Brushes.Black, MarginLeft, statusY);
                    }

                    if (game.GameOver)
                    {
                        string msg = game.StatusLine;
                        using (var bigFont = new Font(Font.FontFamily, 20, FontStyle.Bold))
                        {
                            var size = g.MeasureString(msg, bigFont);
                            g.DrawString(msg, bigFont, Brushes.Black,
                                (ClientSize.Width - size.Width) / 2,
                                (ClientSize.Height - size.Height) / 2);
                        }
                        resetButton.Visible = true;
                    }

                }

            }



            // --- 2) L�t�t�r overlay (3�3) + keret ---
            DrawSheriffVision(g);

            // --- 3) �gensek ---
            DrawAgent(g, city.Sheriff.X, city.Sheriff.Y, Brushes.CornflowerBlue);
            foreach (var b in city.Bandits)
                DrawAgent(g, b.X, b.Y, Brushes.Firebrick);

            // --- 4) St�tuszsor ---
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
