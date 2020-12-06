using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Common
{
    public static class Extensions
    {
        public static U GetOrCreate<T,U>(this Dictionary<T, U> dict, T t, U u)
        {
            if (dict.TryGetValue(t, out var realU)) { return realU; }
            dict[t] = u;
            return u;
        }

        public static List<T> AsTokens<T>(this string s, params string[] splitBy)
        {
            if (splitBy?.Any() != true)
            {
                splitBy = new[] { " ", "\n", "\r", "," };
            }
            return s
                .Split(splitBy, StringSplitOptions.RemoveEmptyEntries)
                .FPipeMap(str => (T)Convert.ChangeType(str, typeof(T)))
                .ToList();
        }
        //public static GridPosition ToPos(this (int x, int y) pos)
        //{
        //    return new GridPosition(pos.x, pos.y); ;
        //}
        //public static (int x, int y) ToTpl(this GridPosition pos)
        //{
        //    return (pos.X, pos.Y);
        //}
    }
}
