using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdgeList = System.Collections.Generic.List<(int node, double weight)>;
using static System.Linq.Enumerable;
using Medallion.Collections;

namespace AOC.Common
{
    public static class Moves
    {

        public static Func<(int x, int y), (int, int)> TurnLeft = tpl => (-tpl.y, tpl.x);
        public static Func<(int x, int y), (int, int)> TurnRight = tpl => (tpl.y, -tpl.x);

        public static Func<(int, int), IEnumerable<((int, int), double)>> PLUS = ((int x, int y) tpl) =>
        {
            var x = tpl.x;
            var y = tpl.y;
            return new[] { (x + 1, y), (x, y + 1), (x - 1, y), (x, y - 1) }.Select(tpl => (tpl, 1.0));
        };

        public static Func<(int, int), IEnumerable<((int, int), double)>> DIAG = ((int x, int y) tpl) =>
        {
            var x = tpl.x;
            var y = tpl.y;
            return new[] { (x + 1, y + 1), (x - 1, y + 1), (x - 1, y - 1), (x + 1, y - 1) }.Select(tpl => (tpl, 1.0));
        };

        public static Func<(int, int), IEnumerable<((int, int), double)>> All8Dirs = ((int x, int y) tpl) =>
        {
            var x = tpl.x;
            var y = tpl.y;
            return new[] { (x + 1, y), (x, y + 1), (x - 1, y), (x, y - 1), (x + 1, y + 1), (x - 1, y + 1), (x - 1, y - 1), (x + 1, y - 1) }.Select(tpl => (tpl, 1.0));
        };
    }

    public class Graph<T>
    {
        public HashSet<T> Nodes { get; set; }
        Func<T, IEnumerable<(T node, double weight)>> GetAdj;
        public static Graph<T> From2dWithMoves(IEnumerable<T> nodes, Func<T, IEnumerable<(T, double)>> moves)
        {
            return new Graph<T>
            {
                Nodes = new HashSet<T>(nodes),
                GetAdj = moves,
            };
        }

        public GraphResult<T> Dijkstra(T start, T finish)
        {
            //var info = Range(0, adjacency.Count).Select(i => (distance: double.PositiveInfinity, prev: i)).ToArray();
            Dictionary<T, double> distOfOpen = new();
            distOfOpen[start] = 0;
            var closed = new HashSet<T>();
            Dictionary<T, ImmutableLinkedList<(T, double)>> pathsRev = new();

            var heap = new Heap<(T node, ImmutableLinkedList<(T, double)> prevLL, double distance)>((a, b) => a.distance.CompareTo(b.distance));
            heap.Push((start, new ImmutableLinkedList<(T, double)>(), 0));

            while (heap.Count > 0)
            {
                var current = heap.Pop();

                if (closed.Contains(current.node)) continue;
                closed.Add(current.node);

                //var prevLL = pathsRev.GetValueOrDefault(current.prev, new ImmutableLinkedList<(T, double)>());
                var currentLL = current.prevLL.Append((current.node, current.distance));
                pathsRev[current.node] = currentLL;

                if (current.node.Equals(finish)) { break; }

                var edges = GetAdj(current.node).Where(v => !closed.Contains(v.node) && Nodes.Contains(v.node));
                foreach(var e in edges)
                {
                    var (v, weight) = e;
                    if (closed.Contains(v)) { continue; }

                    double alt = current.distance + weight;
                    if (distOfOpen.TryGetValue(v, out double prevAlt))
                    {
                        if (alt <= prevAlt) { heap.Push((v, currentLL, alt)); distOfOpen[v] = alt; }
                    }
                    else
                    {
                        heap.Push((v, currentLL, alt)); distOfOpen[v] = alt;
                    }
                }

            }
            return new GraphResult<T>(pathsRev, finish);
        }

    }

    public class GraphResult<T>
    {
        public Dictionary<T, ImmutableLinkedList<(T node, double totalDist)>> Paths { get; private set; }

        private T _finish;

        public GraphResult(Dictionary<T, ImmutableLinkedList<(T, double)>> pathsRev, T finish)
        {
            Paths = pathsRev;
            _finish = finish;
        }

        public List<T> Path()
        {
            return Paths[_finish].Select(_ => _.node).ToList();
        }
        public double Dist()
        {
            return Paths[_finish].First().totalDist;
        }
    }

    //public class Graph
    //{
    //    private readonly List<EdgeList> adjacency;

    //    public Graph(int vertexCount) => adjacency = Range(0, vertexCount).Select(v => new EdgeList()).ToList();

    //    public int Count => adjacency.Count;
    //    public bool HasEdge(int s, int e) => adjacency[s].Any(p => p.node == e);
    //    public bool RemoveEdge(int s, int e) => adjacency[s].RemoveAll(p => p.node == e) > 0;

    //    public bool AddEdge(int s, int e, double weight)
    //    {
    //        if (HasEdge(s, e)) return false;
    //        adjacency[s].Add((e, weight));
    //        return true;
    //    }

    //    public (double distance, int prev)[] FindPath(int start)
    //    {
    //        var info = Range(0, adjacency.Count).Select(i => (distance: double.PositiveInfinity, prev: i)).ToArray();
    //        info[start].distance = 0;
    //        var visited = new System.Collections.BitArray(adjacency.Count);

    //        var heap = new Heap<(int node, double distance)>((a, b) => a.distance.CompareTo(b.distance));
    //        heap.Push((start, 0));
    //        while (heap.Count > 0)
    //        {
    //            var current = heap.Pop();
    //            if (visited[current.node]) continue;
    //            var edges = adjacency[current.node];
    //            for (int n = 0; n < edges.Count; n++)
    //            {
    //                int v = edges[n].node;
    //                if (visited[v]) continue;
    //                double alt = info[current.node].distance + edges[n].weight;
    //                if (alt < info[v].distance)
    //                {
    //                    info[v] = (alt, current.node);
    //                    heap.Push((v, alt));
    //                }
    //            }
    //            visited[current.node] = true;
    //        }
    //        return info;
    //    }

    //}

    public class Heap<T>
    {
        private readonly IComparer<T> comparer;
        private readonly List<T> list = new List<T> { default };

        public Heap() : this(default(IComparer<T>)) { }

        public Heap(IComparer<T> comparer)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
        }

        public Heap(Comparison<T> comparison) : this(Comparer<T>.Create(comparison)) { }

        public int Count => list.Count - 1;

        public void Push(T element)
        {
            list.Add(element);
            SiftUp(list.Count - 1);
        }

        public T Pop()
        {
            T result = list[1];
            list[1] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            SiftDown(1);
            return result;
        }

        private static int Parent(int i) => i / 2;
        private static int Left(int i) => i * 2;
        private static int Right(int i) => i * 2 + 1;

        private void SiftUp(int i)
        {
            while (i > 1)
            {
                int parent = Parent(i);
                if (comparer.Compare(list[i], list[parent]) > 0) return;
                (list[parent], list[i]) = (list[i], list[parent]);
                i = parent;
            }
        }

        private void SiftDown(int i)
        {
            for (int left = Left(i); left < list.Count; left = Left(i))
            {
                int smallest = comparer.Compare(list[left], list[i]) <= 0 ? left : i;
                int right = Right(i);
                if (right < list.Count && comparer.Compare(list[right], list[smallest]) <= 0) smallest = right;
                if (smallest == i) return;
                (list[i], list[smallest]) = (list[smallest], list[i]);
                i = smallest;
            }
        }

    }

    //public class Graphs<T, U> where T: TreeNode<U>
    //{
    //    public T DijkstraOrBFS(T start)
    //    {
    //        Dictionary<int, List<T>> queue = new Dictionary<int, List<T>>();
    //        var hStart = start.Heuristic();
    //        queue[hStart] = new[] { start }.ToList();

    //        Dictionary<U, int> distByState = new Dictionary<U, int>();

    //        for(int i = hStart; ; i++)
    //        {
    //            if (!queue.TryGetValue(i, out var listR)) { continue; }

    //            foreach (var r in listR.OrderBy(_ => _.Heuristic()))
    //            {
    //                if (r.IsEndGoal()) { return r; }

    //                if (distByState.ContainsKey(r.State())) { continue; }

    //                foreach(var child in r.Expand().Cast<T>())
    //                {
    //                    int h = child.Heuristic();
    //                    if (h <= 0) { throw new Exception("Should I just return child right here, then?"); }
    //                    if (!queue.ContainsKey(h)) { queue[h] = new List<T>(); }
    //                    queue[h].Add(child);
    //                }
    //            }
    //        }

    //        throw new Exception("Nothing found with DijsktraOrBFS");
    //    }

    //    public T DFS(T start)
    //    {
    //        Stack<T> queue = new Stack<T>();
    //        queue.Push(start);

    //        while(queue.Any())
    //        {
    //            var r= queue.Pop();
    //            if (r.IsEndGoal()) { return r; }
    //            foreach(var child in r.Expand()?.Cast<T>() ?? new List<T>())
    //            {
    //                queue.Push(child);
    //            }
    //        }

    //        throw new Exception("Nothing found with DFS");
    //    }
    //}
}
