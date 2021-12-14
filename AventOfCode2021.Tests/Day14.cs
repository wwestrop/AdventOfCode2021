﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AventOfCode2021.Tests
{
    public class Day14
    {
        [Theory]
        [InlineData(1, "NCNBCHB")]
        [InlineData(2, "NBCCNBBBCBHCB")]
        [InlineData(3, "NBBBCNCCNBBNBNBBCHBHHBCHB")]
        [InlineData(4, "NBBNBNBBCCNBCNCCNBBNBBNBBBNBBNBBCBHCBHHNHCBBCBHCB")]
        public void ExampleByResult(int steps, string expectedOutput)
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

            var result = PerformInsertions(input, steps);
            Assert.Equal(expectedOutput, result);
        }

        [Theory]
        [InlineData(5, 97)]
        [InlineData(10, 3073)]
        public void ExampleByLength(int steps, int expectedLength)
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

            var result = PerformInsertions(input, steps);
            Assert.Equal(expectedLength, result.Length);
        }

        [Fact]
        public void ExampleScoring()
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

            var result = PerformInsertions(input, steps: 10);
            var score = Score(result);
            Assert.Equal(1588, score);
        }

        [Fact]
        public void MyInput()
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

            var result = PerformInsertions(input, steps: 10);
            var score = Score(result);

            Console.WriteLine(score);
        }

        private int Score(string input)
        {
            var atomCounts = input.ToCharArray()
                .GroupBy(c => c)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToList();

            var mostCommon = atomCounts.MaxBy(g => g.Count);
            var leastCommon = atomCounts.MinBy(g => g.Count);

            return mostCommon.Count - leastCommon.Count;
        }

        private string PerformInsertions(string input, int steps)
        {
            (var template, var rules) = Parse(input);

            StringBuilder sb = null;

            for (int i = 0; i < steps; i++)
            {
                sb = new StringBuilder();
                var templatePairs = Pairify(template);

                foreach (var t in templatePairs)
                {
                    var rule = rules.Single(r => r.from == t);

                    sb.Append(t.Substring(0, 1));
                    sb.Append(rule.to);
                }

                sb.Append(templatePairs.Last().Substring(1, 1));
                template = sb.ToString();
            }
            
            return template;
        }

        private static (string template, (string from, string to)[] rules) Parse(string input)
        {
            var pieces = input.Split("\r\n\r\n");
            var template = pieces[0];

            var insertionRules = from row in pieces[1].Split("\r\n")
                                 let operands = row.Split(" -> ")
                                 let lhs = operands[0]
                                 let rhs = operands[1]
                                 select (@from: lhs, @to: rhs);

            return (template, insertionRules.ToArray());
        }

        private static string[] Pairify(string input)
        {
            var chars = input.ToCharArray();

            return chars.Zip(chars.Skip(1))
                .Select(p => $"{p.First}{p.Second}")
                .ToArray();
        }
    }
}
