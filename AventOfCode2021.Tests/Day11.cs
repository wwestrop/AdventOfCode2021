using AdventOfCode2021;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace AventOfCode2021.Tests
{
    public class Day11
    {
        private int ExampleRunner(string input, int steps)
        {
            var octopususes = Common.ParseNumberGrid(input);
            var numFlashes = 0;

            Common.PrintArray(octopususes);

            for (int step = 0; step < steps; step++)
            {
                SimulateFlashes(octopususes);

                numFlashes += octopususes.OfType<int>().Count(c => c == -1);

                ResetFlashedCells(octopususes);

                Common.PrintArray(octopususes, highlight: c => c == 0);
            }

            return numFlashes;
        }

        public int GetSynchronisedStepNum(string input)
        {
            var octopususes = Common.ParseNumberGrid(input);

            for (int step = 0; ; step++)
            {
                SimulateFlashes(octopususes);

                ResetFlashedCells(octopususes);

                if (octopususes.OfType<int>().All(c => c ==0))
                {
                    return step + 1;
                }
            }
        }

        [Theory]
        [InlineData(1, 9)]
        [InlineData(2, 9)]
        public void SmallerExample(int steps, int expectedFlashes)
        {
            var input = @"11111
19991
19191
19991
11111";

            var numFlashes = ExampleRunner(input, steps);
            Assert.Equal(expectedFlashes, numFlashes);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(10, 204)]
        [InlineData(100, 1656)]
        public void LargerExample(int steps, int expectedFlashes)
        {
            var input = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";

            var numFlashes = ExampleRunner(input, steps);
            Assert.Equal(expectedFlashes, numFlashes);

            var synchronisedStepNum = GetSynchronisedStepNum(input);
            Assert.Equal(195, synchronisedStepNum);
        }

        [Theory]
        [InlineData(100)]
        public void MyPuzzleInput(int steps)
        {
            var input = @"1326253315
3427728113
5751612542
6543868322
4422526221
2234325647
1773174887
7281321674
6562513118
4824541522";

            var numFlashes = ExampleRunner(input, steps);
            Console.WriteLine(numFlashes);

            var synchronisedStepNum = GetSynchronisedStepNum(input);
            Console.WriteLine(synchronisedStepNum);
        }

        private static void ResetFlashedCells(int[,] input)
            => Common.ManipulateGridCells(input, c => c == -1 ? 0 : c);

        private static void IncrementEnergyAt(int[,] input, Point coordiante)
        {
            if (input[coordiante.X, coordiante.Y] != -1)
            {
                input[coordiante.X, coordiante.Y]++;
            }
            
            if (input[coordiante.X, coordiante.Y] > 9)
            {
                input[coordiante.X, coordiante.Y] = -1;

                // Propogate out to the neigbour cells
                foreach (var neighbour in Common.GetNeighbourCoordinatesPlusDiagonals(input, coordiante))
                {
                    IncrementEnergyAt(input, neighbour);
                }
            }
        }

        private void SimulateFlashes(int[,] input)
            => Common.ManipulateGridCells(
                input,
                (i, x, y) => IncrementEnergyAt(i, new Point(x, y)));
    }
}
