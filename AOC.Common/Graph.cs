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
        /// <summary>
        /// Counter Clockwise
        /// </summary>
        public static Func<(int x, int y), (int, int)> TurnDegreesCCW(double deg)
        {
            double rad = deg * (Math.PI / 180);
            return tpl =>
                tpl switch
                {
                    (int x, int y) => ((int)Math.Round(Math.Cos(rad) * x - Math.Sin(rad) * y), (int)Math.Round(Math.Sin(rad) * x + Math.Cos(rad) * y))
                };
        }
        /// <summary>
        /// Clockwise
        /// </summary>
        public static Func<(int x, int y), (int, int)> TurnDegreesCW(double deg)
        {
            double rad = (-deg) * (Math.PI / 180);
            return tpl =>
                tpl switch
                {
                    (int x, int y) => ((int)Math.Round(Math.Cos(rad) * x - Math.Sin(rad) * y), (int)Math.Round(Math.Sin(rad) * x + Math.Cos(rad) * y))
                };
        }

        public static Func<(int x, int y), (int, int)> TurnLeft = tpl => (-tpl.y, tpl.x);
        public static Func<(int x, int y), (int, int)> TurnRight = tpl => (tpl.y, -tpl.x);
        public static Func<(int x, int y), (int, int)> TurnForward = tpl => tpl;
        public static Func<(int x, int y), (int, int)> TurnBack = tpl => (-tpl.x, -tpl.y);

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

        public IEnumerable<T> BFS_Simple(T start)
        {
            HashSet<T> hs = new();
            var q = new Queue<T>();
            q.Enqueue(start);
            hs.Add(start);

            while (q.Any())
            {
                var curr = q.Dequeue();
                yield return curr;

                foreach (var adj in GetAdj(curr))
                {
                    var next = adj.node;
                    if (hs.Contains(next)) { continue; }

                    q.Enqueue(next);
                    hs.Add(next);

                    curr = next;
                }
            }
        }

        public IEnumerable<T> DFS_Simple(T start)
        {
            HashSet<T> hs = new();
            var st = new Stack<T>();
            st.Push(start);
            hs.Add(start);

            while (st.Any())
            {
                var curr = st.Pop();
                yield return curr;

                foreach (var adj in GetAdj(curr).Reverse())
                {
                    var next = adj.node;
                    if (hs.Contains(next)) { continue; }

                    st.Push(next);
                    hs.Add(next);

                    curr = next;
                }
            }
        }

        public IEnumerable<(List<T> path, double dist)> DFS(T start, T finish)
        {
            var hs = new HashSet<T> { start };
            var st = new Stack<T>();
            st.Push(start);
            foreach (var res in DFS_Internal(hs, st, 0, finish))
            {
                yield return res;
            }
        }

        private IEnumerable<(List<T> path, double dist)> DFS_Internal(HashSet<T> ignored, Stack<T> currPath, double currDist, T finish)
        {
            var curr = currPath.Peek();
            if (curr.Equals(finish)) { yield return (currPath.ToList(), currDist); }
            else
            {
                foreach (var tpl in GetAdj(curr))
                {
                    var (next, weight) = tpl;
                    if (ignored.Contains(next)) { continue; }

                    ignored.Add(next);
                    currPath.Push(next);
                    foreach (var res in DFS_Internal(ignored, currPath, currDist + weight, finish))
                    {
                        yield return res;
                    }
                    currPath.Pop();
                    ignored.Remove(next);
                }
            }
        }
        //public IEnumerable<(List<T> path, double dist)> BFS(T start, T finish)
        //{
            
        //    st.Push(start);
        //    foreach (var res in DFS_Internal(hs, st, 0, finish))
        //    {
        //        yield return res;
        //    }
        //}

        //private IEnumerable<(List<T> path, double dist)> BFS_Internal(T start, double currDist, T finish)
        //{
        //    var q = new Queue<(T node, double dist)>();
        //    var visited = new HashSet<T>();
        //    q.Enqueue(new List<T> { start });

        //    while (q.Any())
        //    {
        //        var curr = q.Dequeue();
        //        visited.Add(curr);

        //        foreach()
        //    }
        //}

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

        public GraphResult(Dictionary<T, ImmutableLinkedList<(T, double)>> paths, T finish)
        {
            Paths = paths;
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
}
