using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public static class Day17
    {
        internal static void Run()
        {
            var input = "target area: x=20..30, y=-10..-5";

            input = input.Replace("target area: x=", "").Replace(" y=", "");

            var acceptableRanges = from axes in input.Split(",")
                                   let extremes = axes.Split("..").Select(int.Parse)
                                   let min = extremes.Min()
                                   let max = extremes.Max()
                                   select (min, max);

            var xrange = acceptableRanges.First();
            var yrange = acceptableRanges.Last();


            var allXPossibilities = GetAllXVelocitiesWhichLandAcceptably(xrange);

            // Now find the maximum value of y for any of these x, which will land in
            // the y target area. (and it can go negative, which could be a PITA)

            // If y target is below the origin, then pointing down is a possibility
            // basically the lowest we can point is the furthest distance down

            // TODO not sure of the -2, but hey, maths
            var lowestPossibleYVelocity = yrange.min - 2;
            while (true)
            {
                lowestPossibleYVelocity++;
            }
        }

        private static (int initialVelocity, int[] stepPositions)[] GetAllXVelocitiesWhichLandAcceptably((int min, int max) landingZone)
        {
            var allXPossibilities = GetAllXStepPositionsForAllVelocities();

            return allXPossibilities
                // Skip those which undershoot
                .SkipWhile(p => !p.stepPositions.Any(c => xInRange(landingZone, c)))
                // Take in the middle
                .TakeWhile(p => p.stepPositions.Any(c => xInRange(landingZone, c)))
                // End when we overshoot
                .ToArray();
        }

        private static IEnumerable<(int initialVelocity, int[] stepPositions)> GetAllXStepPositionsForAllVelocities()
        {
            var xVelocity = 0;
            while (true)
            {
                var positions = GetAllXStepPositionsByVelocity(xVelocity);
                yield return (xVelocity, positions);
                xVelocity++;
            }
        }

        private static int[] GetAllXStepPositionsByVelocity(int initialVelocity)
        {
            // Because of the deceleration, x tends to 0 so this is finite set of possibilities
            var theMaxStep = initialVelocity;

            var allXStepsAtThisVelocity = from step in Enumerable.Range(0, theMaxStep)
                                          let pos = GetXPosition(initialVelocity, step)
                                          select pos;

            return allXStepsAtThisVelocity.ToArray();
        }

        private static bool xInRange((int min, int max) acceptableXRange, int x)
            => x >= acceptableXRange.min && x <= acceptableXRange.max;

        // It's also highly exponential since it keeps getting called
        // C# doesn't know there's no side effect and can memoise
        // Recursive. They're obvs gonna screw me over with that one tho
        public static int GetXPosition(int initialXVelocity, int stepNum)
        {
            if (stepNum == 0)
            {
                return initialXVelocity;
            }
            else if (stepNum >= initialXVelocity)
            {
                return GetXPosition(initialXVelocity, initialXVelocity - 1);
            }
            else
            {
                return GetXPosition(initialXVelocity, stepNum - 1) + initialXVelocity - stepNum;
            }
        }

        public static int GetYPosition(int initialYVelocity, int stepNum)
        {
            if (stepNum == 0)
            {
                return initialYVelocity;
            }
            else
            {
                return GetYPosition(initialYVelocity, stepNum - 1) + initialYVelocity - stepNum;
            }
        }
    }
}
