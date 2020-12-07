using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MoreLinq;

namespace AOC.Common
{
    public static class Helpers
    { 
        public static IEnumerable<(int x, int y, T t)> WalkIdx_ThroughRows<T>(this IEnumerable<IEnumerable<T>> arr)
        {
            List<int> a;
            if (arr.Any())
            {
                int h = arr.Count();
                int w = arr.ElementAt(0).Count();
                for (int j = 0; j < h; j++)
                {
                    var row = arr.ElementAt(j);
                    for (int i = 0; i < w; i++)
                    {
                        yield return (i, j, row.ElementAt(i));
                    }
                }
            }
        }

        //public static IEnumerable<(int x, int y, T t)> WalkIdx_ThroughColumns<T>(this IEnumerable<IEnumerable<T>> arr)
        //{
        //    List<int> a;
        //    if (arr.Any())
        //    {
        //        int h = arr.Count();
        //        int w = arr.ElementAt(0).Count();
        //        for (int i = 0; i < w; i++)
        //        {
        //            var row = arr.ElementAt(j);
        //            for (int j = 0; j < h; j++)
        //            {
        //                yield return (i, j, row.ElementAt(i));
        //            }
        //        }
        //    }
        //}


        public const char CHAR_BLOCK = '█';
        public static List<List<T>> Permutations<T>(this IEnumerable<T> allAvailable, int length, bool repeatable)
        {
            int currentIdx = 0;

            List<List<T>> result = new List<List<T>>() { new List<T>() };

            while (currentIdx < length)
            {
                var r2 = new List<List<T>>();
                foreach (var r in result)
                {
                    foreach(T t in allAvailable)
                    {
                        var l2 = r.ToList();
                        l2.Add(t);
                        if (repeatable || l2.Distinct().Count() == l2.Count())
                        {
                            r2.Add(l2);
                        }
                    }
                }
                result = r2;

                currentIdx++;
            }
            return result;
        }


        public static List<List<int>> Permutations(int minPhase, int maxPhase, int totalNumbers)
        {
            int length = totalNumbers;
            int currentIdx = 0;

            List<List<int>> result = new List<List<int>>() { new List<int>() };

            while (currentIdx < length)
            {
                var r2 = new List<List<int>>();
                foreach (var r in result)
                {
                    for (int phase = minPhase; phase <= maxPhase; phase++)
                    {
                        var l2 = r.ToList();
                        l2.Add(phase);
                        if (l2.Distinct().Count() == l2.Count())
                        {
                            r2.Add(l2);
                        }
                    }
                }
                result = r2;

                currentIdx++;
            }
            return result;
        }

        public static (int x, int y) FindPos(this List<List<char>> map, Func<char, bool> p)
        {
            for(int j = 0; j < map.Count; j++)
            {
                for (int i = 0; i < map[j].Count; i++)
                {
                    if (p(map[j][i]))
                    {
                        return (i, j);
                    }
                }
            }
            throw new Exception();
        }
        public static List<(int x, int y)> FindPosList (this List<List<char>> map, Func<char, bool> p)
        {
            List<(int x, int y)> result = new List<(int x, int y)>();
            for (int j = 0; j < map.Count; j++)
            {
                for (int i = 0; i < map[j].Count; i++)
                {
                    if (p(map[j][i]))
                    {
                        result.Add((i, j));
                        //return (i, j);
                    }
                }
            }
            return result;
        }

        public static void PrintArrayBoolean(bool[,] printer, Func<bool, string> fncPrint)
        {

            for (int j = 0; j < printer.GetLength(1); j++)
            {
                for (int i = 0; i < printer.GetLength(0); i++)
                {
                    Console.Write(fncPrint(printer[i, j]));
                }
                Console.WriteLine();
            }
        }

        public static void PrintArrayGeneric<T>(this T[,] printer)
        {
            for (int j = 0; j < printer.GetLength(1); j++)
            {
                for (int i = 0; i < printer.GetLength(0); i++)
                {
                    Console.Write(printer[i, j].ToString());
                }
                Console.WriteLine();
            }
        }

        public static void PrintArrayGeneric<T>(this T[][] printer)
        {
            for (int j = 0; j < printer.Length; j++)
            {
                for (int i = 0; i < printer[j].Length; i++)
                {
                    Console.Write(printer[j][i].ToString());
                }
                Console.WriteLine();
            }
        }

        public static void PrintArrayGeneric<T>(this List<List<T>> printer)
        {
            for (int j = 0; j < printer.Count; j++)
            {
                for (int i = 0; i < printer[j].Count; i++)
                {
                    Console.Write(printer[j][i].ToString());
                }
                Console.WriteLine();
            }
        }

        public static Dictionary<(int x, int y), T> ToValByPos<T>(this List<List<T>> map)
        {
            var dict = new Dictionary<(int x, int y), T>();
            for (int j = 0; j < map.Count; j++)
            {
                for (int i = 0; i < map[j].Count; i++)
                {
                    dict[(i, j)] = map[j][i];
                }
            }
            return dict;
        }

        public static Dictionary<T, List<(int x, int y)>> ToPosListByVal<T>(this List<List<T>> map)
        {
            var dict = new Dictionary<T, List<(int x, int y)>>();
            for (int j = 0; j < map.Count; j++)
            {
                for (int i = 0; i < map[j].Count; i++)
                {
                    var val = map[j][i];
                    if (!dict.ContainsKey(val)) { dict[val] = new List<(int x, int y)>(); }
                    dict[val].Add((i, j));
                }
            }
            return dict;
        }

        public static bool CoordsOK(int _x, int _y, int LX, int LY)
        {
            if (_x < 0 || _y < 0 || _x >= LX || _y >= LY) { return false; }
            return true;
        }
        public static int GCD(int x, int y)
        {
            return y == 0 ? x : GCD(y, x % y);
        }
        public static long GCD(long x, long y)
        {
            return y == 0 ? x : GCD(y, x % y);
        }



        public static IEnumerable<T> Parse2list<T>(string s)
        {
            var ieT = new string(s.Select(c => char.IsDigit(c) || c == '-' || c == '+' ? c : ' ').ToArray()).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (typeof(T) == typeof(int)) { return (IEnumerable<T>)ieT.Select(_ => int.Parse(_)); }
            if (typeof(T) == typeof(long)) { return (IEnumerable<T>)ieT.Select(_ => long.Parse(_)); }
            if (typeof(T) == typeof(float)) { return (IEnumerable<T>)ieT.Select(_ => float.Parse(_)); }
            if (typeof(T) == typeof(double)) { return (IEnumerable<T>)ieT.Select(_ => double.Parse(_)); }
            if (typeof(T) == typeof(long)) { return (IEnumerable<T>)ieT.Select(_ => long.Parse(_)); }
            return null;
        }

        public static T[] Parse2arr<T>(string s)
        {
            return Parse2list<T>(s).ToArray();
        }

        private static Dictionary<(int, int), U> FlipCoords<U>(this Dictionary<(int c1, int c2), U> screen)
        {
            return screen.Select(kvp => ((kvp.Key.c2, kvp.Key.c1), kvp.Value)).ToDictionary(tpl => tpl.Item1, tpl => tpl.Item2);
        }
        private static Dictionary<(long, long), U> FlipCoords<U>(this Dictionary<(long c1, long c2), U> screen)
        {
            return screen.Select(kvp => ((kvp.Key.c2, kvp.Key.c1), kvp.Value)).ToDictionary(tpl => tpl.Item1, tpl => tpl.Item2);
        }

        public static void PrintScreenDictYX<U>(this Dictionary<(int y, int x), U> screen, U defaultVal, Func<U, string> printFunc = null)
        {
            PrintScreenDictXY(screen.FlipCoords(), defaultVal, printFunc);
        }
        public static void PrintScreenDictYX<U>(this Dictionary<(long y, long x), U> screen, U defaultVal, Func<U, string> printFunc = null)
        {
            PrintScreenDictXY(screen.FlipCoords(), defaultVal, printFunc);
        }

        public static void PrintScreenDictXY<U>(this Dictionary<(int x, int y), U> screen, U defaultVal, Func<U, string> printFunc = null)
        {
            if (printFunc == null) { printFunc = (U u) => { return u?.ToString() ?? " "; }; };

            var maxY = screen.Keys.Max(_ => _.y);
            var maxX = screen.Keys.Max(_ => _.x);
            var minY = screen.Keys.Min(_ => _.y);
            var minX = screen.Keys.Min(_ => _.x);



            for (var j = minY; j <= maxY; j++)
            {
                for (var i = minX; i <= maxX; i++)
                {
                    U val = screen.GetValueOrDefault((i, j), defaultVal);
                    Console.Write(printFunc(val));
                }
                Console.WriteLine();
            }
        }

        public static void PrintScreenDictXY<U>(this Dictionary<(long x, long y), U> screen, U defaultVal, Func<U, string> printFunc = null)
        {
            if (printFunc == null) { printFunc = (U u) => { return u?.ToString() ?? " "; } ; };

            var maxY = screen.Keys.Max(_ => _.y);
            var maxX = screen.Keys.Max(_ => _.x);
            var minY = screen.Keys.Min(_ => _.y);
            var minX = screen.Keys.Min(_ => _.x);



            for (var j = minY; j <= maxY; j++)
            {
                for (var i = minX; i <= maxX; i++)
                {
                    U val = screen.GetValueOrDefault((i, j), defaultVal);
                    Console.Write(printFunc(val));
                }
                Console.WriteLine();
            }
        }

        public static int ManhattanDist(int start_X, int start_Y, int x, int y)
        {
            return Math.Abs(start_X - x) + Math.Abs(start_Y - y);
        }

        /// <summary>
        /// Sets clipboard to value.
        /// </summary>
        /// <param name="value">String to set the clipboard to.</param>
        public static void SetClipboard(string value)
        {
            if (value == null)
                throw new ArgumentNullException("Attempt to set clipboard with null");

            Process clipboardExecutable = new Process();
            clipboardExecutable.StartInfo = new ProcessStartInfo // Creates the process
            {
                RedirectStandardInput = true,
                FileName = @"clip",
            };
            clipboardExecutable.Start();

            clipboardExecutable.StandardInput.Write(value); // CLIP uses STDIN as input.
            // When we are done writing all the string, close it so clip doesn't wait and get stuck
            clipboardExecutable.StandardInput.Close();

            return;
        }
    }
}
