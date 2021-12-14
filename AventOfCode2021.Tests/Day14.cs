using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AventOfCode2021.Tests
{
    public class Day14
    {
//        [Theory]
//        [InlineData(1, "NCNBCHB")]
//        [InlineData(2, "NBCCNBBBCBHCB")]
//        [InlineData(3, "NBBBCNCCNBBNBNBBCHBHHBCHB")]
//        [InlineData(4, "NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB")]
//        public void ExampleByResult(int steps, string expectedOutput)
//        {
//            var input = @"NNCB

//CH -> B
//HH -> N
//CB -> H
//NH -> C
//HB -> C
//HC -> B
//HN -> C
//NN -> C
//BH -> H
//NC -> B
//NB -> B
//BN -> B
//BB -> N
//BC -> B
//CC -> N
//CN -> C";

//            var result = PerformInsertions(input, steps);
//            Assert.Equal(expectedOutput, result);
//        }

//        [Theory]
//        [InlineData(5, 97)]
//        [InlineData(10, 3073)]
//        public void ExampleByLength(int steps, int expectedLength)
//        {
//            var input = @"NNCB

//CH -> B
//HH -> N
//CB -> H
//NH -> C
//HB -> C
//HC -> B
//HN -> C
//NN -> C
//BH -> H
//NC -> B
//NB -> B
//BN -> B
//BB -> N
//BC -> B
//CC -> N
//CN -> C";

//            var result = PerformInsertions(input, steps);
//            Assert.Equal(expectedLength, result.Length);
//        }

        [Theory]
        [InlineData(10, 1588)]
        [InlineData(40, 2188189693529)]
        public void ExampleScoring(int steps, long expectedScore)
        {
            var input = @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";

            var result = PerformInsertionsAndScore(input, steps);
            //var score = Score(result);
            Assert.Equal(expectedScore, result);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(40)]
        public void MyInput(int steps)
        {
            var input = @"ONHOOSCKBSVHBNKFKSBK

HO -> B
KB -> O
PV -> B
BV -> C
HK -> N
FK -> H
NV -> C
PF -> K
FV -> B
NH -> P
CO -> N
HV -> P
OH -> H
BC -> H
SP -> C
OK -> F
KH -> N
HB -> V
FP -> N
KP -> O
FB -> O
FH -> F
CN -> K
BP -> P
SF -> O
CK -> K
KN -> O
VK -> C
HP -> N
KK -> V
KO -> C
OO -> P
BH -> B
OC -> O
HC -> V
HS -> O
SH -> V
SO -> C
FS -> N
CH -> O
PC -> O
FC -> S
VO -> H
NS -> H
PH -> C
SS -> F
BN -> B
BF -> F
NC -> F
CS -> F
NN -> O
FF -> P
OF -> H
NF -> O
SC -> F
KC -> F
CP -> H
CF -> K
BS -> S
HN -> K
CB -> P
PB -> V
VP -> C
OS -> C
FN -> B
NB -> V
BB -> C
BK -> V
VF -> V
VC -> O
NO -> K
KF -> P
FO -> C
OB -> K
ON -> S
BO -> V
KV -> H
CC -> O
HF -> N
VS -> S
PN -> P
SK -> F
PO -> V
HH -> F
VV -> N
VH -> N
SV -> S
CV -> B
KS -> K
PS -> V
OV -> S
SB -> V
NP -> K
SN -> C
NK -> O
PK -> F
VN -> P
PP -> K
VB -> C
OP -> P";

            var result = PerformInsertionsAndScore(input, steps);
            //var score = Score(result);

            Console.WriteLine(result);
        }


        private long Score(Dictionary<char, long> input)
        {
            var mostCommon = input.MaxBy(g => g.Value).Value;
            var leastCommon = input.MinBy(g => g.Value).Value;

            return mostCommon - leastCommon;
        }

        private long Score(string input)
        {
            var atomCounts = input.ToCharArray()
                .GroupBy(c => c)
                .Select(g => new { g.Key, Count = g.LongCount() })
                .ToList();

            var mostCommon = atomCounts.MaxBy(g => g.Count);
            var leastCommon = atomCounts.MinBy(g => g.Count);

            return mostCommon.Count - leastCommon.Count;
        }

        private long PerformInsertionsAndScore(string input, int steps)
        {
            (var template, var rules) = Parse(input);

            var countOfPairs = rules.ToDictionary(
                            r => r.Key,
                            r => Regex.Matches(template, r.Key).LongCount());

            Dictionary<string, long> nextPairs = null;

            void Upsert<K>(Dictionary<K, long> dict, K key, long num)
            {
                if (!dict.ContainsKey(key))
                {
                    dict[key] = 0;
                }

                dict[key] += num;
            }

            var countOfElements = template.GroupBy(c => c)
                .ToDictionary(k => k.Key, v => v.LongCount());


            //Pairify(template)
            for (int i = 0; i < steps; i++)
            {
                nextPairs = new Dictionary<string, long>();
                foreach(var pair in countOfPairs)
                {
                    var rule = rules[pair.Key];

                    var newPair1 = $"{pair.Key[0]}{rule}";
                    var newPair2 = $"{rule}{pair.Key[1]}";

                    Upsert(countOfElements, rule, countOfPairs[pair.Key]);

                    Upsert(nextPairs, newPair1, countOfPairs[pair.Key]);
                    Upsert(nextPairs, newPair2, countOfPairs[pair.Key]);
                    //Upsert(countOfPairs, newPair1, countOfPairs[pair.Key]);
                    //Upsert(countOfPairs, newPair2, countOfPairs[pair.Key]);

                    //nextPairs[pair.Key] = 0;
                    //Upsert(nextBuffer, pair.Key, -thisBuffer[pair.Key]);
                }

                //var swap = nextPairs;
                //countOfPairs = nextPairs[];
                //nextBuffer = swap;

                countOfPairs = nextPairs;//.ToDictionary(k => k.Key, v => v.Value);
                //nextPairs.Clear();
            }


            //foreach(var k in nextBuffer)
            //{
            //    if (nextBuffer[k.Key] == 0) nextBuffer.Remove(k.Key);
            //}

            //var len = nextBuffer.Values.Sum();

            var score = Score(countOfElements);




            return score;
        }

        private static (string template, Dictionary<string, char> rules) Parse(string input)
        {
            var pieces = input.Split("\r\n\r\n");
            var template = pieces[0];

            var insertionRules = from row in pieces[1].Split("\r\n")
                                 let operands = row.Split(" -> ")
                                 let lhs = operands[0]
                                 let rhs = operands[1][0]
                                 select (@from: lhs, @to: rhs);

            return (template, insertionRules.ToDictionary(k => k.from, v => v.to));
        }

        private static string[] Pairify(string input)
        {
            var chars = input.ToCharArray();

            //chars[0..1];

            return input.Zip(input.Skip(1))
                .Select(p => $"{p.First}{p.Second}")
                .ToArray();
        }
    }
}
