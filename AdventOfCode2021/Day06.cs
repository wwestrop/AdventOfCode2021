using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    internal static class Day06
    {
        internal static void Run()
        {
            var input = "3,4,3,1,2";
            input = "1,4,2,4,5,3,5,2,2,5,2,1,2,4,5,2,3,5,4,3,3,1,2,3,2,1,4,4,2,1,1,4,1,4,4,4,1,4,2,4,3,3,3,3,1,1,5,4,2,5,2,4,2,2,3,1,2,5,2,4,1,5,3,5,1,4,5,3,1,4,5,2,4,5,3,1,2,5,1,2,2,1,5,5,1,1,1,4,2,5,4,3,3,1,3,4,1,1,2,2,2,5,4,4,3,2,1,1,1,1,2,5,1,3,2,1,4,4,2,1,4,5,2,5,5,3,3,1,3,2,2,3,4,1,3,1,5,4,2,5,2,4,1,5,1,4,5,1,2,4,4,1,4,1,4,4,2,2,5,4,1,3,1,3,3,1,5,1,5,5,5,1,3,1,2,1,4,5,4,4,1,3,3,1,4,1,2,1,3,2,1,5,5,3,3,1,3,5,1,5,3,5,3,1,1,1,1,4,4,3,5,5,1,1,2,2,5,5,3,2,5,2,3,4,4,1,1,2,2,4,3,5,5,1,1,5,4,3,1,3,1,2,4,4,4,4,1,4,3,4,1,3,5,5,5,1,3,5,4,3,1,3,5,4,4,3,4,2,1,1,3,1,1,2,4,1,4,1,1,1,5,5,1,3,4,1,1,5,4,4,2,2,1,3,4,4,2,2,2,3";

            var lanternFishGroups = input.Split(",").Select(int.Parse).GroupBy(f => f).ToDictionary(k => k.Key, v => (ulong)v.Count());

            for (int i = 0; i <= 8; i++)
            {
                if (!lanternFishGroups.ContainsKey(i))
                {
                    lanternFishGroups[i] = 0;
                }
            }

            const int days = 256;
            for(int i = 0; i < days; i++)
            {
                var incrementsThisRound = Enumerable.Range(0, 9).ToDictionary(k => k, v => (ulong)0);

                for(int f = 0; f < lanternFishGroups.Count; f++)
                {
                    var numFishInBucket = lanternFishGroups[f];
                    if (f == 0)
                    {
                        incrementsThisRound[8] += numFishInBucket;
                        incrementsThisRound[6] += numFishInBucket;
                    }
                    else
                    {
                        incrementsThisRound[f - 1] += numFishInBucket;
                    }

                    incrementsThisRound[f] -= numFishInBucket;
                }

                foreach(var kvp in incrementsThisRound)
                {
                    lanternFishGroups[kvp.Key] += kvp.Value;
                }
            }

            var total = GetGrandTotal(lanternFishGroups);
            Console.WriteLine(total);
        }

        private static ulong GetGrandTotal(Dictionary<int, ulong> fishBuckets)
        {
            ulong total = 0;

            for(int i = 0; i < fishBuckets.Count; i++)
            {
                total += fishBuckets[i];
            }

            return total;
        }
    }
}
