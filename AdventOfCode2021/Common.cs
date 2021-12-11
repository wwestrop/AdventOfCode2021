using System;
using System.Linq;

namespace AdventOfCode2021
{
    public static class Common
    {
        public static int[,] ParseNumberGrid(string input)
        {
            var rows = input.Split(Environment.NewLine);

            var width = rows[0].Length;        // Assume they're all equal
            var height = rows.Count();

            int[,] result = new int[width, height];

            for (int r = 0; r < height; r++)
            {
                var cols = rows[r].ToCharArray();
                for (int c = 0; c < width; c++)
                {
                    result[c, r] = CharToInt(cols[c]);
                }
            }

            return result;
        }

        private static int CharToInt(char c)
        {
            return c switch
            {
                '0' => 0,
                '1' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                '9' => 9,
            };
        }

        public static void PrintArray<T>(T[,] input,
            Func<T, bool> highlight = null,
            Func<T, string> conversion = null)
        {
            highlight ??= i => false;
            conversion ??= i => i.ToString();

            int width = input.GetLength(0);
            int height = input.GetLength(1);
            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    var me = input[c, r];
                    var mePresentable = conversion(me);
                    Console.BackgroundColor = highlight(me) switch
                    {
                        false => ConsoleColor.Black,
                        true => ConsoleColor.Red,
                    };
                    Console.ForegroundColor = highlight(me) switch
                    {
                        false => ConsoleColor.Gray,
                        true => ConsoleColor.White,
                    };
                    Console.Write(mePresentable);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine();
            }

            for (int i = 0; i < width + 15; i++) Console.Write("-");
            Console.WriteLine();
        }

        public static void ManipulateCells<T>(T[,] input, Func<T, T> manipulation)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);

            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    input[c, r] = manipulation(input[c, r]);
                }
            }
        }
    }
}
