using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021
{
    public static class Common
    {
        public static T[,] ParseGrid<T>(string input, Func<char, T> inputConverter)
        {
            var rows = input.Split(Environment.NewLine);

            var width = rows[0].Length;        // Assume they're all equal
            var height = rows.Count();

            T[,] result = new T[width, height];

            for (int r = 0; r < height; r++)
            {
                var cols = rows[r].ToCharArray();
                for (int c = 0; c < width; c++)
                {
                    result[c, r] = inputConverter(cols[c]);
                }
            }

            return result;
        }

        public static int[,] ParseNumberGrid(string input)
            => ParseGrid(input, CharToInt);

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

        public static void ManipulateGridCells<T>(T[,] input, Func<T, T> manipulation)
        {
            Action <T[,], int, int> thunkedManipulation
                = (T[,] i, int x, int y) => i[x, y] = manipulation(i[x, y]);

            ManipulateGridCells(input, thunkedManipulation);
        }

        public static void ManipulateGridCells<T>(T[,] input, Action<T[,], int, int> manipulation)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);

            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    manipulation(input, c, r);
                }
            }
        }

        public static IEnumerable<Point> GetNeighbourCoordinates<T>(T[,] input, Point coordinate)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);

            if (coordinate.Y != 0)
            {
                // north
                yield return new Point(coordinate.X, coordinate.Y - 1);
            }
            if (coordinate.Y != height - 1)
            {
                // south
                yield return new Point(coordinate.X, coordinate.Y + 1);
            }
            if (coordinate.X != 0)
            {
                // west
                yield return new Point(coordinate.X - 1, coordinate.Y);
            }
            if (coordinate.X != width - 1)
            {
                // east
                yield return new Point(coordinate.X + 1, coordinate.Y);
            }
        }

        public static IEnumerable<Point> GetNeighbourCoordinatesPlusDiagonals<T>(T[,] input, Point coordinate)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);

            foreach (var c in GetNeighbourCoordinates(input, coordinate))
            {
                yield return c;
            }


            if (coordinate.X != 0 && coordinate.Y != 0)
            {
                // northwest
                yield return new Point(coordinate.X - 1, coordinate.Y - 1);
            }

            if (coordinate.X != width - 1 && coordinate.Y != 0)
            {
                // northeast
                yield return new Point(coordinate.X + 1, coordinate.Y - 1);
            }

            if (coordinate.X != 0 && coordinate.Y != height - 1)
            {
                // southwest
                yield return new Point(coordinate.X - 1, coordinate.Y + 1);
            }

            if (coordinate.X != width - 1 && coordinate.Y != height - 1)
            {
                // southeast
                yield return new Point(coordinate.X + 1, coordinate.Y + 1);
            }
        }
    }
}
