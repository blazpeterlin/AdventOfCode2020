using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Environment;

namespace AOC.Common
{
    public class Patterns
    {
        public List<string> Rows { get; set; } = new List<string>();
        public List<string> Cols { get; set; } = new List<string>();
        private int w;
        private int h;
        public Patterns(List<List<char>> map)
        {
            if (map.Count == 0) { w = 0; h = 0; return; }
            h = map.Count;
            w = map[0].Count;
            

            for (int j = 0; j < h; j++)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < w; i++)
                {
                    sb.Append(map[j][i]);
                }
                Rows.Add(sb.ToString());
            }

            for (int i = 0; i < w; i++)
            {
                var sb = new StringBuilder();
                for (int j = 0; j < h; j++)
                {
                    sb.Append(map[j][i]);
                }
                Cols.Add(sb.ToString());
            }
        }

        public List<(int rowIdx, Capture capt)> FindPatternInRows(string regex)
        {
            var res = new List<(int rowIdx, Capture capt)>();
            for (int i = 0; i < Rows.Count; i++)
            {
                var row = Rows[i];
                var capts = Regex.Matches(row, regex).Cast<Match>().SelectMany(m =>m.Captures.Cast<Capture>()).ToList();
                foreach(var capt in capts)
                {
                    res.Add((i, capt));
                }
            }
            return res;
        }

        public List<(int colIdx, Capture capt)> FindPatternInCols(string regex)
        {
            var res = new List<(int colIdx, Capture capt)>();
            for (int i = 0; i < Cols.Count; i++)
            {
                var col = Cols[i];
                var capts = Regex.Matches(col, regex).Cast<Match>().SelectMany(m => m.Captures.Cast<Capture>()).ToList();
                foreach (var capt in capts)
                {
                    res.Add((i, capt));
                }
            }
            return res;
        }

        public List<(int rowIdxStart, int colIdxStart)> FindPatternInArea(string regexArea)
        {
            string[] rowRegex = regexArea.Split(NewLine, StringSplitOptions.RemoveEmptyEntries);
            var numRR = rowRegex.Length;
            Dictionary<(int row, int col), int> countByPos = new Dictionary<(int row, int col), int>();
            for (int i = 0; i < Rows.Count; i++)
            {
                var row = Rows[i];
                for(int j = 0; j < rowRegex.Length; j++)
                {
                    if (i - j < 0) { continue; }
                    var rr = rowRegex[j];
                    var capts = Regex.Matches(row, rr).Cast<Match>().SelectMany(m => m.Captures.Cast<Capture>()).ToList();
                    foreach (var capt in capts)
                    {
                        var key = (capt.Index, i - j);
                        if (!countByPos.ContainsKey(key)) { countByPos[key] = 0; }
                        countByPos[key]++;
                    }
                }
            }

            var res = new List<(int rowIdxStart, int colIdxStart)>();
            foreach (var kvp in countByPos)
            {
                if (kvp.Value == numRR)
                {
                    res.Add(kvp.Key);
                }
            }

            return res;
        }

        //public void X()
        //{
        //    foreach(string row in rows)
        //    {

        //    }
        //}
    }
}
