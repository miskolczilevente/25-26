namespace AIChess
{
    public partial class Form1 : Form
    {
        private const int tileSize = 60;
        private const int gridSize = 8;
        private Table table;
        private Puppet selectedPiece = null;

        // 🔥 új változó a körökhöz
        private bool whiteTurn = true; // fehér kezd

        public Form1()
        {
            InitializeComponent();
            table = new Table();
            this.ClientSize = new Size(gridSize * tileSize, gridSize * tileSize);
            this.MouseClick += Form1_MouseClick;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / tileSize;
            int y = e.Y / tileSize;

            if (selectedPiece == null)
            {
                // csak a soron következő szín bábuja választható
                selectedPiece = table.Pieces.FirstOrDefault(
                    p => p.X == x && p.Y == y && p.IsWhite == whiteTurn
                );
            }
            else
            {
                if (selectedPiece.IsValidMove(x, y, table))
                {
                    var target = table.Pieces.FirstOrDefault(p => p.X == x && p.Y == y);
                    if (target != null && target.IsWhite != selectedPiece.IsWhite)
                        table.Pieces.Remove(target);

                    selectedPiece.X = x;
                    selectedPiece.Y = y;

                    // 🔥 kör váltás
                    whiteTurn = !whiteTurn;
                }

                selectedPiece = null;
            }

            Invalidate();
        }
    }
}
