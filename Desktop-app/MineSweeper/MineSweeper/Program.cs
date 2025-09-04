using System;

namespace Minesweeper
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Add meg a pálya méretét (pl. 8): ");
            int size = int.Parse(Console.ReadLine());
            Console.Write("Add meg a bombák számát: ");
            int bombs = int.Parse(Console.ReadLine());

            MinesweeperGame game = new MinesweeperGame(size, bombs);
            game.StartGame();
        }
    }
}
