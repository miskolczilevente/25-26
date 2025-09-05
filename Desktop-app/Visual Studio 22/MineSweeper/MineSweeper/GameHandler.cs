
using System;

namespace Minesweeper
{
    abstract class GameHandler
    {
        protected Table table;
        public abstract void StartGame();
    }

    class MinesweeperGame : GameHandler
    {
        private int size, bombs;

        //public MinesweeperGame(int size, int bombs)
        public MinesweeperGame()
        {
            this.size = size;
            this.bombs = bombs;
            table = new Table(size, bombs);
        }

        public override void StartGame()
        {

            Console.Write("Add meg a pálya méretét (pl. 8): ");
            size = int.Parse(Console.ReadLine());
            Console.Write("Add meg a bombák számát: ");
            bombs = int.Parse(Console.ReadLine());

            if (size > 10) 
                size = 10;

            if (bombs > (size * size - 1))
                bombs = size * size - 1;

            table = new Table(size, bombs);

            bool running = true;
            while (running)
            {
                table.PrintTable();
                Console.WriteLine("Parancsok: r x y = reveal, f x y = flag, q = quit");
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input == null) continue;

                var parts = input.Split(' ');
                if (parts[0] == "q") break;

                if (parts.Length < 3) continue;
                int x = int.Parse(parts[1]);
                int y = int.Parse(parts[2]);

                if (parts[0] == "r")
                {
                    bool safe = table.RevealField(x, y);
                    if (!safe)
                    {
                        table.RevealAll();           
                        table.PrintTable();
                        Console.WriteLine("💥 VESZTETTÉL! Bombára léptél!");
                        running = false;
                    }
                }
                else if (parts[0] == "f")
                {
                    table.ToggleFlag(x, y);
                    if (table.CheckWin())
                    {
                        table.RevealAll();           
                        table.PrintTable();
                        Console.WriteLine("🎉 NYERTÉL! Minden bombát megjelöltél!");
                        running = false;
                    }
                }

            }



            Console.WriteLine("Szeretnél új játékot? (i/n)");
            string restart = Console.ReadLine();
            if (restart.ToLower() == "i")
            {
                Program.Main(null);
            }
        }

        
    }
}
