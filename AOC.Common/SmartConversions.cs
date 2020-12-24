using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace AOC.Common
{
    public static class SmartConversions
    {
        public static int AsInt(this string str) => int.Parse(str);
        public static long AsLong(this string str) => long.Parse(str);
        public static float AsFloat(this string str) => float.Parse(str);
        public static char AsChar(this string str) => str.Single();
        public static double AsDouble(this string str) => double.Parse(str);
        public static T Id<T>(T t) => t;
        public static string AsString(string str) => str;



        public static int AsInt2(string str) => int.Parse(str);
        public static long AsLong2(string str) => long.Parse(str);
        public static float AsFloat2(string str) => float.Parse(str);
        public static char AsChar2(string str) => str.Single();
        public static double AsDouble2(string str) => double.Parse(str);
        public static string AsString2(string str) => str;

        //public static D NameTokens<D, T1>(this IEnumerable<string> tkns, D d)
        //{
        //    dynamic res = new ExpandoObject();

        //    string[] propertyNames = d.GetType().GetProperties().Select(p => p.Name).ToArray();
        //    foreach (var prop in propertyNames)
        //    {
        //        object propValue = d.GetType().GetProperty(prop).GetValue(d, null);
        //        propValue
        //    }
        //    foreach (var prop in d)
        //    {

        //    }
        //}


        public static (T1 t1,bool ignoreMe) Tuplify<T1>(this IEnumerable<string> tkns, Func<string, T1> convert1)
        {
            return (convert1(tkns.ElementAt(0)),true);
        }
        public static (T1 t1, T2 t2) Tuplify<T1,T2>(this IEnumerable<string> tkns, Func<string, T1> convert1, Func<string, T2> convert2)
        {
            return (convert1(tkns.ElementAt(0)), convert2(tkns.ElementAt(1)));
        }
        public static (T1 t1, T2 t2, T3 t3) Tuplify<T1, T2, T3>(
            this IEnumerable<string> tkns, Func<string, T1> convert1, 
            Func<string, T2> convert2,
            Func<string, T3> convert3)
        {
            return (convert1(tkns.ElementAt(0)), convert2(tkns.ElementAt(1)), convert3(tkns.ElementAt(2)));
        }
        public static (T1 t1, T2 t2, T3 t3, T4 t4) Tuplify<T1, T2, T3, T4>(
            this IEnumerable<string> tkns, 
            Func<string, T1> convert1,
            Func<string, T2> convert2,
            Func<string, T3> convert3,
            Func<string, T4> convert4)
        {
            return (convert1(tkns.ElementAt(0)), convert2(tkns.ElementAt(1)), convert3(tkns.ElementAt(2)), convert4(tkns.ElementAt(3)));
        }
        public static (T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) Tuplify<T1, T2, T3, T4, T5>(
            this IEnumerable<string> tkns,
            Func<string, T1> convert1,
            Func<string, T2> convert2,
            Func<string, T3> convert3,
            Func<string, T4> convert4,
            Func<string, T5> convert5)
        {
            return (convert1(tkns.ElementAt(0)), convert2(tkns.ElementAt(1)), convert3(tkns.ElementAt(2)), convert4(tkns.ElementAt(3)), convert5(tkns.ElementAt(4)));
        }
        public static (T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) Tuplify<T1, T2, T3, T4, T5, T6>(
            this IEnumerable<string> tkns,
            Func<string, T1> convert1,
            Func<string, T2> convert2,
            Func<string, T3> convert3,
            Func<string, T4> convert4,
            Func<string, T5> convert5,
            Func<string, T6> convert6)
        {
            return (convert1(tkns.ElementAt(0)), convert2(tkns.ElementAt(1)), convert3(tkns.ElementAt(2)), convert4(tkns.ElementAt(3)), convert5(tkns.ElementAt(4)), convert6(tkns.ElementAt(5)));
        }
        public static (T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) Tuplify<T1, T2, T3, T4, T5, T6, T7>(
            this IEnumerable<string> tkns,
            Func<string, T1> convert1,
            Func<string, T2> convert2,
            Func<string, T3> convert3,
            Func<string, T4> convert4,
            Func<string, T5> convert5,
            Func<string, T6> convert6,
            Func<string, T7> convert7)
        {
            return (convert1(tkns.ElementAt(0)), convert2(tkns.ElementAt(1)), convert3(tkns.ElementAt(2)), convert4(tkns.ElementAt(3)), convert5(tkns.ElementAt(4)), convert6(tkns.ElementAt(5)), convert7(tkns.ElementAt(6)));
        }
        public static (T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) Tuplify<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IEnumerable<string> tkns,
            Func<string, T1> convert1,
            Func<string, T2> convert2,
            Func<string, T3> convert3,
            Func<string, T4> convert4,
            Func<string, T5> convert5,
            Func<string, T6> convert6,
            Func<string, T7> convert7,
            Func<string, T8> convert8)
        {
            return (convert1(tkns.ElementAt(0)), convert2(tkns.ElementAt(1)), convert3(tkns.ElementAt(2)), convert4(tkns.ElementAt(3)), convert5(tkns.ElementAt(4)), convert6(tkns.ElementAt(5)), convert7(tkns.ElementAt(6)), convert8(tkns.ElementAt(7)));
        }
    }
}
