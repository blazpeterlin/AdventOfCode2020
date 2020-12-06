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

        public static string[] FSplit(this string str, params string[] delimiters)
        {
            return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }
        public static string[] FSplit(this string str, params char[] delimiters)
        {
            return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }


        public static IEnumerable<int> FRng(int min, int max)
        {
            for(int val = min; val<max; val++)
            {
                yield return val;
            }
        }
        public static IEnumerable<long> FRng(long min, long max)
        {
            for (var val = min; val < max; val++)
            {
                yield return val;
            }
        }

        public static IEnumerable<(int fst,int snd)> FRng(this IEnumerable<int> listFst, int min, int max)
        {
            foreach (var fst in listFst)
            {
                for (var snd = min; snd < max; snd++)
                {
                    yield return (fst,snd);
                }
            }
        }

        public static IEnumerable<(long fst, long snd)> FRng(this IEnumerable<long> listFst, long min, long max)
        {
            foreach (var fst in listFst)
            {
                for (var snd = min; snd < max; snd++)
                {
                    yield return (fst, snd);
                }
            }
        }

        public static IEnumerable<(int fst, int snd, int thd)> FRng(this IEnumerable<(int, int)> listFstSnd, int min, int max)
        {
            foreach (var (fst,snd) in listFstSnd)
            {
                for (var thd = min; thd < max; thd++)
                {
                    yield return (fst, snd, thd);
                }
            }
        }

        public static IEnumerable<(long fst, long snd, long thd)> FRng(this IEnumerable<(long, long)> listFstSnd, long min, long max)
        {
            foreach (var (fst, snd) in listFstSnd)
            {
                for (var thd = min; thd < max; thd++)
                {
                    yield return (fst, snd, thd);
                }
            }
        }
    }
}
