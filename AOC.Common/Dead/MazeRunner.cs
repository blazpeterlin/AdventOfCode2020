using Roy_T.AStar;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace AOC.Common
{
    public class MazeRunner
    {
        public class FmrOutput
        {
            /// <summary>
            /// LLLRRRRRFFLLFFRRR
            /// </summary>
            public List<char> TurnLRF { get; set; } = new List<char>();
            /// <summary>
            /// LF8RF9
            /// </summary>
            public string TurnLRNumF { get; set; } = "";
            /// <summary>
            /// ^^^<>vv>>>vv<<^^
            /// </summary>
            public List<char> UpLeftDownBottom { get; set; } = new List<char>();
            /// <summary>
            /// sequence of map chars
            /// </summary>
            public List<char> Char { get; set; } = new List<char>();
            public List<(int x, int y)> Pos { get; set; } = new List<(int x, int y)>();
        }

        public Grid LLCharToGrid(List<List<char>> map, Func<char, bool> isBlock = null)
        {
            var grid = Grid.CreateGridWithLateralConnections(new GridSize((int)map.Count, (int)map[0].Count), new Size(Distance.FromMeters(1),Distance.FromMeters(1)), Velocity.FromMetersPerSecond(1));
            
            for (int j = 0; j < map.Count; j++)
            {
                for (int i = 0; i < map[j].Count; i++)
                {
                    if (isBlock(map[j][i]))
                    {
                        grid.BlockCell(new GridPosition(j, i));
                    }
                    else
                    {
                        grid.UnblockCell(new GridPosition(j, i));
                    }
                }
            }
            return grid;
        }

        //public MazeRunner WithMap(List<List<char>> map)
        //{

        //}












        public FmrOutput RunForward(List<List<char>> chars, (int x, int y) pos, Func<(int x, int y), char, bool> isPosWalkable)
        {
            string path = "";
            string dir = "^";

            var curPos = pos;
            int stepsF = 0;

            var res = new FmrOutput();

            while (true)
            {
                res.Pos.Add(curPos);
                res.Char.Add(chars[curPos.y][curPos.x]);

                int dx = 0, dy = 0;
                if (dir == "^") { dy = -1; }
                if (dir == "v") { dy = 1; }
                if (dir == "<") { dx = -1; }
                if (dir == ">") { dx = 1; }

                (int x, int y) nextPos = (curPos.x + dx, curPos.y + dy);

                bool isOk((int x, int y) p)
                {
                    return p.y >= 0 && p.x >= 0 && p.y < chars.Count && p.x < chars[p.y].Count
                        //&& chars[p.y][p.x] != '.';
                        && isPosWalkable(p, chars[p.y][p.x]);
                }

                if (isOk(nextPos))
                {
                    res.TurnLRF.Add('F');
                    stepsF++;
                    curPos = nextPos;
                }
                else
                {
                    if (stepsF > 0)
                    {
                        res.TurnLRNumF += stepsF;
                    }
                    stepsF = 0;
                    char turn;
                    if (dir == "^" && isOk((curPos.x - 1, curPos.y))) { dir = "<"; turn  = 'L'; }
                    else if (dir == "^" && isOk((curPos.x + 1, curPos.y))) { dir = ">"; turn = 'R'; }
                    else if (dir == "v" && isOk((curPos.x - 1, curPos.y))) { dir = "<"; turn = 'R'; }
                    else if (dir == "v" && isOk((curPos.x + 1, curPos.y))) { dir = ">"; turn = 'L'; }
                    else if (dir == "<" && isOk((curPos.x, curPos.y - 1))) { dir = "^"; turn = 'R'; }
                    else if (dir == "<" && isOk((curPos.x, curPos.y + 1))) { dir = "v"; turn = 'L'; }
                    else if (dir == ">" && isOk((curPos.x, curPos.y - 1))) { dir = "^"; turn = 'L'; }
                    else if (dir == ">" && isOk((curPos.x, curPos.y + 1))) { dir = "v"; turn = 'R'; }
                    else { break; }

                    res.UpLeftDownBottom.Add(dir[0]);

                    res.TurnLRF.Add(turn);
                    res.TurnLRNumF += turn;
                }
            }
            if (stepsF > 0)
            {
                res.TurnLRNumF += stepsF;
            }

            return res;
            //return path.Trim(',');
        }
    }
}
