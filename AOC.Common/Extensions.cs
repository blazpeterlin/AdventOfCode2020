using System;
using System.Collections.Generic;

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
