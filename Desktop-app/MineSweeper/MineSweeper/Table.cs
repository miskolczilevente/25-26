
using System;
using System.Collections.Generic;

namespace Minesweeper
{
    class Table
    {
        private Field[,] fields;
        private int size;
        private int bombCount;

        public Table(int size, int bombCount)
        {
            this.size = size;
            this.bombCount = bombCount;
            fields = new Field[size, size];
            GenerateTable();
        }

        private void GenerateTable()
        {
            Random rnd = new Random();
            HashSet<(int, int)> bombs = new HashSet<(int, int)>();

            while (bombs.Count < bombCount)
            {
                int x = rnd.Next(size);
                int y = rnd.Next(size);
                bombs.Add((x, y));
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (bombs.Contains((i, j)))
                        fields[i, j] = new BombField();
                    else
                        fields[i, j] = new EmptyField();
                }
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (fields[i, j] is EmptyField)
                    {
                        int bombsAround = CountAdjacentBombs(i, j);
                        if (bombsAround > 0)
                            fields[i, j] = new NumberField(bombsAround);
                    }
                }
            }
        }

        private int CountAdjacentBombs(int x, int y)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    int nx = x + dx, ny = y + dy;
                    if (nx >= 0 && nx < size && ny >= 0 && ny < size)
                    {
                        if (fields[nx, ny] is BombField) count++;
                    }
                }
            }
            return count;
        }

        public bool RevealField(int x, int y)
        {
            if (!IsValid(x, y) || fields[x, y].IsRevealed) return true;

            fields[x, y].Reveal();

            if (fields[x, y] is EmptyField)
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (IsValid(i, j))
                        {
                            RevealField(i, j); 
                        }
                    }
                }
            }

            if (fields[x, y] is BombField)
            {
                return false;
            }

            return true;
        }

        public void ToggleFlag(int x, int y)
        {
            if (IsValid(x, y) && !fields[x, y].IsRevealed)
                fields[x, y].IsFlagged = !fields[x, y].IsFlagged;
        }

        public bool CheckWin()
        {
            int correctlyFlagged = 0, totalBombs = 0;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (fields[i, j] is BombField)
                    {
                        totalBombs++;
                        if (fields[i, j].IsFlagged) correctlyFlagged++;
                    }
                }
            }
            return correctlyFlagged == totalBombs;
        }

        private bool IsValid(int x, int y) => x >= 0 && x < size && y >= 0 && y < size;

        public void PrintTable()
        {
            Console.Clear();
            Console.Write("   ");
            for (int j = 0; j < size; j++) Console.Write($"{j} ");
            Console.WriteLine();

            for (int i = 0; i < size; i++)
            {
                Console.Write($"{i} ");
                if (i < 10) Console.Write(" ");
                for (int j = 0; j < size; j++)
                {
                    Console.Write(fields[i, j].GetSymbol() + " ");
                }
                Console.WriteLine();
            }
        }

        public void RevealAll()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    fields[i, j].Reveal();
                }
            }
        }

    }
}
