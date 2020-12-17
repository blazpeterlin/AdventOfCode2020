﻿using System;
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

        public static IEnumerable<T> FUnfoldClass<T>(this T startElt, Func<T, T> fnc) where T:class
        {
            var currElt = startElt;
            while (true)
            {
                var fncOutput = fnc(currElt);
                if (fncOutput == null) { break; }
                yield return fncOutput;
                currElt = fncOutput;
            }
        }
        public static IEnumerable<(T1,T2)?> FUnfold<T1,T2>(this (T1,T2) startElt, Func<(T1,T2)?, (T1,T2)?> fnc)
        {
            var currElt = startElt;
            while (true)
            {
                var fncOutput = fnc(currElt);
                if (fncOutput == null) { break; }
                yield return fncOutput.Value;
                currElt = fncOutput.Value;
            }
        }
        public static IEnumerable<(T1, T2, T3)?> FUnfold<T1, T2, T3>(this (T1, T2, T3) startElt, Func<(T1, T2, T3)?, (T1, T2, T3)?> fnc)
        {
            var currElt = startElt;
            while (true)
            {
                var fncOutput = fnc(currElt);
                if (fncOutput == null) { break; }
                yield return fncOutput.Value;
                currElt = fncOutput.Value;
            }
        }
        public static IEnumerable<(T1, T2, T3, T4)?> FUnfold<T1, T2, T3, T4>(this (T1, T2, T3, T4) startElt, Func<(T1, T2, T3, T4)?, (T1, T2, T3, T4)?> fnc)
        {
            var currElt = startElt;
            while (true)
            {
                var fncOutput = fnc(currElt);
                if (fncOutput == null) { break; }
                yield return fncOutput.Value;
                currElt = fncOutput.Value;
            }
        }

        public static IEnumerable<U> FUnfold<T, U>(this T startElt, Func<T, (U, T)?> fnc)
        {
            var currElt = startElt;
            while(true)
            {
                var fncOutput = fnc(currElt);
                if (fncOutput == null) { break; }
                var (eltOutput, nextElt) = fncOutput.Value;
                yield return eltOutput;
                currElt = nextElt;
            }
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


        public static IEnumerable<int> FRng(int min, int exceptMax)
        {
            for(int val = min; val<exceptMax; val++)
            {
                yield return val;
            }
        }
        public static IEnumerable<long> FRng(long min, long exceptMax)
        {
            for (var val = min; val < exceptMax; val++)
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

        public static IEnumerable<(int fst, int snd, int thd, int fth)> FRng(this IEnumerable<(int, int, int)> listFstSndThd, int min, int max)
        {
            foreach (var (fst, snd, thd) in listFstSndThd)
            {
                for (var fth = min; fth < max; fth++)
                {
                    yield return (fst, snd, thd, fth);
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
        public static IEnumerable<(long fst, long snd, long thd, long fth)> FRng(this IEnumerable<(long, long, long)> listFstSndThd, long min, long max)
        {
            foreach (var (fst, snd, thd) in listFstSndThd)
            {
                for (var fth = min; fth < max; fth++)
                {
                    yield return (fst, snd, thd, fth);
                }
            }
        }






        public static IEnumerable<T1> FTupled<T1>(params T1[] seed)
        {
            return seed;
        }
        public static IEnumerable<(T1 fst, T2 snd)> FTupled<T1, T2>(this IEnumerable<T1> listFst, params T2[] seed)
        {
            foreach (var fst in listFst)
            {
                foreach (var s in seed)
                {
                    yield return (fst, s);
                }
            }
        }
        public static IEnumerable<(T1 fst, T2 snd, T3 thd)> FTupled<T1, T2, T3>(this IEnumerable<(T1, T2)> listFstSnd, params T3[] seed)
        {
            foreach (var (fst, snd) in listFstSnd)
            {
                foreach (var s in seed)
                {
                    yield return (fst, snd, s);
                }
            }
        }
        public static IEnumerable<(T1 fst, T2 snd, T3 thd, T4 frh)> FTupled<T1, T2, T3, T4>(this IEnumerable<(T1, T2, T3)> listFstSndThd, params T4[] seed)
        {
            foreach (var (fst, snd, thd) in listFstSndThd)
            {
                foreach (var s in seed)
                {
                    yield return (fst, snd, thd, s);
                }
            }
        }
        public static IEnumerable<(T1 fst, T2 snd, T3 thd, T4 frh, T5 ffh)> FTupled<T1, T2, T3, T4, T5, T6>(this IEnumerable<(T1, T2, T3, T4)> listFstSndThd, params T5[] seed)
        {
            foreach (var (fst, snd, thd, frh) in listFstSndThd)
            {
                foreach (var s in seed)
                {
                    yield return (fst, snd, thd, frh, s);
                }
            }
        }

    }
}
