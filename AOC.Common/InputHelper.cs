using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace AOC.Common
{
    public class InputHelper
    {
        private Func<string, string> SettingModifyLines = SmartConversions.Id;
        private Func<string, string> SettingModifyTxt = SmartConversions.Id;


        public InputHelper ModifyLines(Func<string, string> ml)
        {
            SettingModifyLines = ml;
            return this;
        }
        public InputHelper ModifyTxt(Func<string, string> mt)
        {
            SettingModifyTxt = mt;
            return this;
        }
        public string CalcedTxt { get; set; }

        public bool P { get; private set; }
        public static InputHelper LoadInputP(int year)
        {
            string fname = "input.txt";
            FilePath = CalcFilePath(fname);
            string fpath = FilePath;
            string res;
            if (!File.Exists(fpath)) { File.WriteAllText(fpath, ""); res = ""; } else { res = File.ReadAllText(fpath); }

            if (res.Length == 0)
            {
                var baseAddress = new Uri("https://adventofcode.com");
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(baseAddress, new Cookie("session", Consts.SessionId));

                var httpc = new HttpClient(
                    new HttpClientHandler
                    {
                        CookieContainer = cookieContainer,
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    })
                {
                    BaseAddress = baseAddress,
                };
                var httpRes = httpc.GetAsync($"{year}/day/{Day()}/input").Result;
                httpRes.EnsureSuccessStatusCode();
                res = httpRes.Content.ReadAsStringAsync().Result;
                File.WriteAllText(fpath, res);
            }
            return new InputHelper() { P = true };
        }
        public static InputHelper LoadInputT(int year)
        {
            string fname = "input-t.txt";
            FilePath = CalcFilePath(fname);
            string fpath = FilePath;
            string res;
            if (!File.Exists(fpath)) { File.WriteAllText(fpath, ""); res = ""; } else { res = File.ReadAllText(fpath); }

            return new InputHelper();
        }

        private static string CalcFilePath(string fname)
        {
            string fpath =
                Path.Combine(
                    Assembly.GetExecutingAssembly().Location
                    .FPipe(Path.GetDirectoryName)
                    .FPipe(Path.GetDirectoryName)
                    .FPipe(Path.GetDirectoryName)
                    .FPipe(Path.GetDirectoryName),
                    fname);
            return fpath;
        }

        //public static InputHelper LoadInput(int year)
        //{
        //    string fpath = FilePath();
        //    string res;
        //    if (!File.Exists(fpath)) { File.WriteAllText(fpath, ""); res = ""; } else { res = File.ReadAllText(fpath); }

        //    if (res.Length == 0)
        //    {
        //        var baseAddress = new Uri("https://adventofcode.com");
        //        var cookieContainer = new CookieContainer();
        //        cookieContainer.Add(baseAddress, new Cookie("session", Consts.SessionId));

        //        var httpc = new HttpClient(
        //            new HttpClientHandler
        //            {
        //                CookieContainer = cookieContainer,
        //                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        //            })
        //        {
        //            BaseAddress = baseAddress,
        //        };
        //        var httpRes = httpc.GetAsync($"{year}/day/{Day()}/input").Result;
        //        httpRes.EnsureSuccessStatusCode();
        //        res = httpRes.Content.ReadAsStringAsync().Result;
        //        File.WriteAllText(fpath, res);
        //    }
        //    return new InputHelper();
        //}

        public static int Day()
        {
            string entryName = Assembly.GetEntryAssembly().GetName().Name;
            int day = int.Parse(entryName[^2..]);
            return day;
        }

        public static string FilePath { get; private set; }

        public IEnumerable<string> FileInputModifiedLines
        {
            get
            {
                var txt = File.ReadAllText(FilePath);
                var res = 
                    txt
                    .Replace("\r", "")
                    .FPipe(SettingModifyTxt)
                    .FPipe(txt => txt.Split("\n"))
                    .Select(SettingModifyLines)
                    .Where(ln => ln != null)
                    .ToList();
                if (res.Last() == "") { res.RemoveAt(res.Count - 1); }
                return res;
            }
        }
        public string FileInputModifiedText
        {
            get
            {
                return FileInputModifiedLines
                    .FPipe(_ => string.Join("\n", _));
            }
        }

        public string AsText()
        {
            return FileInputModifiedText;
        }

        public List<T> AsTokens<T>(params string[] splitBy)
        {
            return FileInputModifiedText.AsTokens<T>(splitBy);
        }

        public List<List<char>> AsCharListOfLists()
        {
            return 
                FileInputModifiedLines
                .Select(ln => ln.ToCharArray().ToList()).ToList();
        }



        public List<string> AsLines()
        {
            return
                FileInputModifiedLines
                .ToList();
        }
    }
}
