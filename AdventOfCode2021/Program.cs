using System;
using System.Linq;

namespace AdventOfCode2021
{
    public class Program
    {
        public static void Main()
        {
            Day2.Run();
            Day1();
        }

        private static void Day1()
        {
            var depths = new[] {
                199,
                200,
                208,
                210,
                200,
                207,
                240,
                269,
                260,
                263,
            };

            var numIncrements = depths
                .Zip(depths.Skip(1))
                .Count(z => z.Second > z.First);

            Console.WriteLine(numIncrements);


            var slidingWindows = depths
                .Take(depths.Length - 2)
                .Select((_, i) => depths[i] + depths[i + 1] + depths[i + 2]);

            numIncrements = slidingWindows
                .Zip(slidingWindows.Skip(1))
                .Count(z => z.Second > z.First);

            Console.WriteLine(numIncrements);
        }
    }
}
