using System;
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

            var allYPossibilities = GetAllYVelocitiesWhichLandAcceptably(yrange);

            // We can't simply take the largest arc, as we must find a combination
            // of X that also lands on the target in the same step as the Y
            foreach (var y in allYPossibilities)
            {
                foreach (var x in allXPossibilities)
                {
                    var allSteps = Math.Min(y.stepPositions.Length, x.stepPositions.Length);
                    for (int s = 0; s < allSteps; s++)
                    {
                        if (LandsWithinRange(yrange, y.stepPositions[s])
                            && LandsWithinRange(xrange, x.stepPositions[s]))
                        {
                            // This is the first output.
                            // But oh my, three nested loops!!!
                            // I ****WONDER**** what puzzle part 2 will be, maybe a much bigger input perhaps?????

                            Console.WriteLine($"Velocity with highest arc is {x.initialVelocity},{y.initialVelocity}");
                            return;
                        }
                    }
                }
            }
        }

        private static (int initialVelocity, int[] stepPositions)[] GetAllYVelocitiesWhichLandAcceptably((int min, int max) landingZone)
        {
            var allYPossibilities = GetAllYStepPositionsForAllVelocities(landingZone.min);

            return allYPossibilities
                // Skip those which undershoot
                .SkipWhile(p => !p.stepPositions.Any(c => LandsWithinRange(landingZone, c)))
                // Take in the middle
                .TakeWhile(p => p.stepPositions.Any(c => LandsWithinRange(landingZone, c)))
                // End when we overshoot

                // The one with the highest arc is the last one (since we searched UP,
                // as the bottom is bounded by not totally passing the target)
                .OrderByDescending(r => r.initialVelocity)
                .ToArray();
        }

        private static (int initialVelocity, int[] stepPositions)[] GetAllXVelocitiesWhichLandAcceptably((int min, int max) landingZone)
        {
            var allXPossibilities = GetAllXStepPositionsForAllVelocities();

            return allXPossibilities
                // Skip those which undershoot
                .SkipWhile(p => !p.stepPositions.Any(c => LandsWithinRange(landingZone, c)))
                // Take in the middle
                .TakeWhile(p => p.stepPositions.Any(c => LandsWithinRange(landingZone, c)))
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

        private static IEnumerable<(int initialVelocity, int[] stepPositions)> GetAllYStepPositionsForAllVelocities(int minimumInitialVelocity)
        {
            var yVelocity = minimumInitialVelocity;
            while (true)
            {
                var positions = GetAllYStepPositionsByVelocity(yVelocity);
                yield return (yVelocity, positions);
                yVelocity++;
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

        private static int[] GetAllYStepPositionsByVelocity(int initialVelocity)
        {
            var debuggableMaxValue = 30;    // int.MaxValue

            var allYStepsAtThisVelocity = from step in Enumerable.Range(0, debuggableMaxValue)
                                          let pos = GetYPosition(initialVelocity, step)
                                          select pos;

            return allYStepsAtThisVelocity.ToArray();
        }

        private static bool LandsWithinRange((int min, int max) acceptableXRange, int landingPoint)
            => landingPoint >= acceptableXRange.min && landingPoint <= acceptableXRange.max;

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
