using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    internal static class Day20
    {
        internal static void Run()
        {
            var input = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###";

            var (outputMap, inputImage) = ParseInput(input);

            var output = ApplyEnhancement(inputImage, outputMap);
            Common.PrintArray(output, conversion: i => i ? "#" : ".");

            output = ApplyEnhancement(output, outputMap);
            Common.PrintArray(output, conversion: i => i ? "#" : ".");

            var numPixels = output.OfType<bool>().Count(o => o);
            Console.WriteLine(numPixels);
        }

        internal static bool[,] ApplyEnhancement(bool[,] inputImage, bool[] algorithmParams)
        {
            var width = inputImage.GetLength(0);
            var height = inputImage.GetLength(1);

            var output = new bool[width, height];

            Common.ManipulateGridCells(inputImage, (i, x, y) =>
            {
                var kernelLookup = GetKernel(inputImage, new Point(x, y));
                var outputValue = algorithmParams[kernelLookup];

                output[x, y] = outputValue;
            });

            return output;
        }

        internal static (bool[] outputMap, bool[,] inputImage) ParseInput(string input)
        {
            var outputMapInput = input.Split("\r\n\r\n")[0];
            var inputImageInput = input.Split("\r\n\r\n")[1];

            var outputMap = outputMapInput
                .ToCharArray()
                .Select(c => c == '#')
                .ToArray();

            var inputImage = Common.ParseGrid(inputImageInput, c => c == '#');

            return (outputMap, inputImage);
        }

        internal static int GetKernel(bool[,] input, Point coordinate)
        {
            int resultantBinary = 0;

            int width = input.GetLength(0);
            int height = input.GetLength(1);
            foreach (var c in GetKernelCoords(coordinate))
            {
                resultantBinary <<= 1;
                if (c.X < 0 || c.X >= width - 1 || c.Y < 0 || c.Y >= height - 1)
                {
                    // Edge of canvas, this is a dark pixel, remains 0
                }
                else
                {
                    var mask = input[c.X, c.Y] ? 1 : 0;
                    resultantBinary |= mask;
                }
            }

            return resultantBinary;
        }

        private static IEnumerable<Point> GetKernelCoords(Point coordinate)
        {
            // I factored out Common.GetNeighbours - but then this is different in that:
            //    1. It requires a value for every neighbour, even at the (virtual) edge of the (infinite but not really) image
            //    2. The binary requires that they be output in a particular order
            //    3. The cell itself is included, it's not just neighbours

            // northwest
            yield return new Point(coordinate.X - 1, coordinate.Y - 1);

            // north
            yield return new Point(coordinate.X, coordinate.Y - 1);

            // northeast
            yield return new Point(coordinate.X + 1, coordinate.Y - 1);

            // west
            yield return new Point(coordinate.X - 1, coordinate.Y);

            // self
            yield return new Point(coordinate.X, coordinate.Y);

            // east
            yield return new Point(coordinate.X + 1, coordinate.Y);

            // southwest
            yield return new Point(coordinate.X - 1, coordinate.Y + 1);

            // south
            yield return new Point(coordinate.X, coordinate.Y + 1);

            // southeast
            yield return new Point(coordinate.X + 1, coordinate.Y + 1);
        }
    }
}
