using AdventOfCode2021;
using System.Collections.Generic;
using Xunit;
using Path = System.Collections.Generic.LinkedList<string>;
using Graph = System.Collections.Generic.IReadOnlyCollection<AventOfCode2021.Tests.Day12.GraphEdge>;
using System.Linq;
using System;

namespace AventOfCode2021.Tests
{
    public class Day12
    {
        // AoC likes to trick us. It won't surprise me if we get a cycle of big caves

        [Fact]
        public void SmallSample()
        {
            var input = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";

            var edges = input.Parse(r => new GraphEdge(r));

            var allPaths = FindPaths(edges);

            Assert.Equal(10, allPaths.Count);
        }

        [Fact]
        public void LargerSample()
        {
            var input = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";

            var edges = input.Parse(r => new GraphEdge(r));

            var allPaths = FindPaths(edges);
                //.Select(p => string.Join(",", p))
                //.OrderBy(s => s)
                //.ToArray();

            Assert.Equal(19, allPaths.Count);
        }

        [Fact]
        public void EvenLargerSample()
        {
            var input = @"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW";

            var edges = input.Parse(r => new GraphEdge(r));

            var allPaths = FindPaths(edges);
                //.Select(p => string.Join(",", p))
                //.OrderBy(s => s)
                //.ToArray();

            Assert.Equal(226, allPaths.Count);
        }

        [Fact]
        public void MyInput()
        {
            var input = @"ey-dv
AL-ms
ey-lx
zw-YT
hm-zw
start-YT
start-ms
dv-YT
hm-ms
end-ey
AL-ey
end-hm
rh-hm
dv-ms
AL-dv
ey-SP
hm-lx
dv-start
end-lx
zw-AL
hm-AL
lx-zw
ey-zw
zw-dv
YT-ms";

            var edges = input.Parse(r => new GraphEdge(r));

            var allPaths = FindPaths(edges);

            Console.WriteLine(allPaths.Count);
        }

        private static IReadOnlyCollection<Path> FindPaths(Graph graph)
        {
            var pathResults = new List<Path>();
            Find(graph, "start", new HashSet<string>(), new Path(), pathResults);

            return pathResults;
        }

        private static void Find(
            Graph graph,
            string fromVertex,
            HashSet<string> seen,
            Path pathSoFar,
            List<Path> completedPaths)
        {
            pathSoFar.AddLast(fromVertex);

            if (fromVertex.IsSmallCave())
            {
                seen.Add(fromVertex);
            }

            if (fromVertex.IsEnd())
            {
                // we've got to the end, though it's possible this path doesn't visit every cave
                completedPaths.Add(pathSoFar);

                seen.Remove(fromVertex);
                return;
            }

            // graph is undirected
            var outwardEdges = graph.Where(e => e.from == fromVertex || e.to == fromVertex);

            foreach (var e in outwardEdges)
            {
                if (e.from != fromVertex && !seen.Contains(e.from))
                {
                    // the method indicates this LinkList ctor will *COPY* the linklist, not exactly what I wanted
                    Find(graph, e.from, seen, new Path(pathSoFar), completedPaths);
                }
                else if (e.to != fromVertex && !seen.Contains(e.to))
                {
                    // the method indicates this LinkList ctor will *COPY* the linklist, not exactly what I wanted
                    Find(graph, e.to, seen, new Path(pathSoFar), completedPaths);
                }
            }

            seen.Remove(fromVertex);
        }

        public record GraphEdge()
        {
            public readonly string from;
            public readonly string to;

            public GraphEdge(string inputRow) : this()
            {
                var vertices = inputRow.Split("-");
                from = vertices[0];
                to = vertices[1];
            }
        }
    }

    public static class Day12Extensions
    {
        public static bool IsSmallCave(this string cave)
            => cave.ToLowerInvariant() == cave && !cave.IsEnd();

        public static bool IsStart(this string cave)
            => cave == "start";

        public static bool IsEnd(this string cave)
            => cave == "end";

        // Can going back to the start be part of a valid path?
        // Any cycles are going to be a big pain as there are infinitely many variants of them
        public static bool CanRevisit(this string cave)
            => !cave.IsSmallCave() || cave.IsStart();
    }
}
