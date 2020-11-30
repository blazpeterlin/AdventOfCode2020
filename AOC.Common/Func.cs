using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC.Common
{
    public static class Func
    {
        public static U FPipe<T, U>(this T t, Func<T, U> fnc)
        {
            return fnc(t);
        }
        public static IEnumerable<U> FPipeMap<T, U>(this IEnumerable<T> t, Func<T, U> fnc)
        {
            return t.Select(_ => fnc(_));
        }

        public static TState FFold<T, TState>(this IEnumerable<T> list, Func<T, TState, TState> fnc, TState init)
        {
            var res = init;
            foreach(var elt in list)
            {
                res = fnc(elt, res);
            }
            return res;
        }

        public static IEnumerable<TState> FScan<T, TState>(this IEnumerable<T> list, Func<T, TState, TState> fnc, TState init)
        {
            var res = init;
            yield return res;
            foreach (var elt in list)
            {
                res = fnc(elt, res);
                yield return res;
            }
        }

        //public static IEnumerable<T> F
    }
}
