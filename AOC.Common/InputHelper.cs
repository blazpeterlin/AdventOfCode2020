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

        public static InputHelper LoadInput(int year)
        {
            string fpath = FilePath();
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
            return new InputHelper();
        }

        public static int Day()
        {
            string entryName = Assembly.GetEntryAssembly().GetName().Name;
            int day = int.Parse(entryName[^2..]);
            return day;
        }

        public static string FilePath()
        {
#if DEBUGT
            //string filename = "" + num + "t";
            string filename = "input-t.txt";
#else
            //string filename = "" + Day() + ".txt";
            string filename = "input.txt";
#endif

            string fpath =
                Path.Combine(
                    Assembly.GetExecutingAssembly().Location
                    .FPipe(Path.GetDirectoryName)
                    .FPipe(Path.GetDirectoryName)
                    .FPipe(Path.GetDirectoryName)
                    .FPipe(Path.GetDirectoryName),
                    filename);
            return fpath;

        }

        public string AsText()
        {
            return File.ReadAllText(FilePath());
        }

        public List<T> AsTokens<T>()
        {
            return
                File.ReadAllText(FilePath())
                .Split(new[] { " ", "\n", "\r", "," }, StringSplitOptions.RemoveEmptyEntries)
                .FPipeMap(str => (T)Convert.ChangeType(str, typeof(T)))
                .ToList();
        }

        public List<List<char>> AsCharListOfLists()
        {
            return File.ReadAllLines(FilePath()).Select(ln => ln.ToCharArray().ToList()).ToList();
        }



        public List<string> AsLines()
        {
            return File.ReadAllLines(FilePath()).ToList();
        }
    }
}
