using AdventOfCode2021;
using Xunit;

namespace AventOfCode2021.Tests
{
    public class Day17Tests
    {
        [Theory]
        [InlineData(7, 0, 7)]
        [InlineData(7, 1, 13)]
        [InlineData(7, 2, 18)]
        [InlineData(7, 3, 22)]
        [InlineData(7, 4, 25)]
        [InlineData(7, 5, 27)]

        [InlineData(6, 0, 6)]
        [InlineData(6, 1, 11)]
        [InlineData(6, 2, 15)]
        [InlineData(6, 3, 18)]
        [InlineData(6, 4, 20)]
        [InlineData(6, 5, 21)]
        [InlineData(6, 6, 21)]
        [InlineData(6, 7, 21)]
        [InlineData(6, 8, 21)]

        [InlineData(17, 0, 17)]      //
        [InlineData(17, 1, 33)]      // These pass right over the target
        [InlineData(17, 2, 48)]      //
        [InlineData(17, 3, 62)]      //
        public void GetX(int initialXVelocity, int stepNum, int expectedPosition)
        {
            var pos = Day17.GetXPosition(initialXVelocity, stepNum);
            Assert.Equal(expectedPosition, pos);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, -1)]
        [InlineData(0, 2, -3)]
        [InlineData(0, 3, -6)]

        [InlineData(2, 0, 2)]
        [InlineData(2, 1, 3)]
        [InlineData(2, 2, 3)]
        [InlineData(2, 3, 2)]
        [InlineData(2, 4, 0)]
        [InlineData(2, 5, -3)]
        [InlineData(2, 6, -7)]

        [InlineData(3, 0, 3)]
        [InlineData(3, 1, 5)]
        [InlineData(3, 2, 6)]
        [InlineData(3, 3, 6)]
        [InlineData(3, 4, 5)]
        [InlineData(3, 5, 3)]
        [InlineData(3, 6, 0)]
        [InlineData(3, 7, -4)]
        [InlineData(3, 8, -9)]

        [InlineData(-4, 0, -4)]
        [InlineData(-4, 1, -9)]
        [InlineData(-4, 2, -15)]
        [InlineData(-4, 3, -22)]
        public void GetY(int initialYVelocity, int stepNum, int expectedPosition)
        {
            var pos = Day17.GetYPosition(initialYVelocity, stepNum);
            Assert.Equal(expectedPosition, pos);
        }
    }
}
