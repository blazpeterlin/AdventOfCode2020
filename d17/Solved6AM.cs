using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOC.Common;
using MoreLinq;
using static System.Environment;
using static AOC.Common.SmartConversions;
using static AOC.Common.Func;

namespace d17
{
    class Solved6AM
    {
        //// Reminders:

        //var txt = ih.AsText();
        //var tkns = ih.AsTokens<int>();
        //var chch = ih.AsCharListOfLists();

        // AsInt
        // Graph<(int, int)>.From2dWithMoves(pts, Moves.PLUS).Dijkstra((0,0),(3,1)).Path();
        // ih.ModifyLines(ln => { if (ln == "") return "-"; else return ln; });

        //.Select(ln => ln.FSplit("-", " ", ":"))
        //.Select(tkns => tkns.Tuplify(int.Parse, Enumerable.First, x => x)) // int, char, string
        //.Where(t => t.t1 < t.t2)

        // var allPos = chs.ToValByPos();

        // var vm = new VirtualMachine(lns);
        // var visited = new HashSet<int>();
        // bool rut()
        // {
        //     return !visited.Add(vm.Idx);
        // }
        // vm.RunUntilTrue(rut);
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var lns = ih.AsCharListOfLists().Select(lst => lst.Select(ch => ch == '#').ToList()).ToList();
            {
                int numSteps = 6;
                int xSize = lns.Count + numSteps * 2 + 2;
                int ySize = lns[0].Count + numSteps * 2 + 2;
                int zSize = 1 + numSteps * 2 + 2;
                var coords = new bool[xSize][][];
                var centerX = (xSize) / 2;
                var centerY = (ySize) / 2;
                var centerZ = (zSize) / 2;
                for (int x = 0; x < xSize; x++)
                {
                    coords[x] = new bool[ySize][];
                    for (int y = 0; y < ySize; y++)
                    {
                        coords[x][y] = new bool[zSize];
                        for (int z = 0; z < zSize; z++)
                        {
                        }
                    }
                }

                for (int xC = 0; xC < lns.Count; xC++)
                {
                    for (int yC = 0; yC < lns[xC].Count; yC++)
                    {
                        coords[xC + centerX - lns.Count / 2][yC + centerY - lns[xC].Count / 2][centerZ] = lns[xC][yC];
                    }
                }

                void step()
                {
                    Dictionary<(int, int, int), bool> newCoords = new();
                    for (int x = 1; x < xSize - 1; x++)
                    {
                        for (int y = 1; y < ySize - 1; y++)
                        {
                            for (int z = 1; z < zSize - 1; z++)
                            {
                                var numNeighbours =
                                    FRng(x - 1, x + 2).FRng(y - 1, y + 2).FRng(z - 1, z + 2)
                                    .Where(tpl => tpl != (x, y, z))
                                    .Select(tpl => coords[tpl.fst][tpl.snd][tpl.thd] ? 1 : 0)
                                    .Sum();
                                if (numNeighbours != 0)
                                {

                                }

                                if (coords[x][y][z] && !(numNeighbours >= 2 && numNeighbours <= 3))
                                {
                                    newCoords[(x, y, z)] = false;
                                }
                                else if (!coords[x][y][z] && numNeighbours == 3)
                                {
                                    newCoords[(x, y, z)] = true;
                                }
                            }
                        }
                    }

                    foreach (var kvp in newCoords)
                    {
                        var (x, y, z) = kvp.Key;
                        coords[x][y][z] = kvp.Value;
                    }
                }

                for (int i = 0; i < numSteps; i++)
                {
                    step();
                }

                var res1 = coords.SelectMany(c => c.SelectMany(cy => cy.Select(cz => cz ? 1 : 0))).Sum();
            }









            {
                int numSteps = 6;
                int xSize = lns.Count + numSteps * 2 + 2;
                int ySize = lns[0].Count + numSteps * 2 + 2;
                int zSize = 1 + numSteps * 2 + 2;
                int wSize = 1 + numSteps * 2 + 2;
                var coords = new bool[xSize][][][];
                var centerX = (xSize) / 2;
                var centerY = (ySize) / 2;
                var centerZ = (zSize) / 2;
                var centerW = (wSize) / 2;
                for (int x = 0; x < xSize; x++)
                {
                    coords[x] = new bool[ySize][][];
                    for (int y = 0; y < ySize; y++)
                    {
                        coords[x][y] = new bool[zSize][];
                        for (int z = 0; z < zSize; z++)
                        {
                            coords[x][y][z] = new bool[wSize];
                        }
                    }
                }

                for (int xC = 0; xC < lns.Count; xC++)
                {
                    for (int yC = 0; yC < lns[xC].Count; yC++)
                    {
                        coords[xC + centerX - lns.Count / 2][yC + centerY - lns[xC].Count / 2][centerZ][centerW] = lns[xC][yC];
                    }
                }

                void step()
                {
                    Dictionary<(int, int, int, int), bool> newCoords = new();
                    for (int x = 1; x < xSize - 1; x++)
                    {
                        for (int y = 1; y < ySize - 1; y++)
                        {
                            for (int z = 1; z < zSize - 1; z++)
                            {
                                for (int w = 1; w < wSize - 1; w++)
                                {
                                    var numNeighbours =
                                        FRng(x - 1, x + 2).FRng(y - 1, y + 2).FRng(z - 1, z + 2).FRng(w-1,w+2)
                                        .Where(tpl => tpl != (x, y, z, w))
                                        .Select(tpl => coords[tpl.fst][tpl.snd][tpl.thd][tpl.fth] ? 1 : 0)
                                        .Sum();
                                    if (numNeighbours != 0)
                                    {

                                    }

                                    if (coords[x][y][z][w] && !(numNeighbours >= 2 && numNeighbours <= 3))
                                    {
                                        newCoords[(x, y, z, w)] = false;
                                    }
                                    else if (!coords[x][y][z][w] && numNeighbours == 3)
                                    {
                                        newCoords[(x, y, z, w)] = true;
                                    }
                                }
                            }
                        }
                    }

                    foreach (var kvp in newCoords)
                    {
                        var (x, y, z, w) = kvp.Key;
                        coords[x][y][z][w] = kvp.Value;
                    }
                }

                for (int i = 0; i < numSteps; i++)
                {
                    step();
                }

                //var res1 = coords.SelectMany(_ => _?.SelectMany(_2 => _2?.Select(_3 => _3 ? 1 : 0)?.ToArray() ?? new int[] { })?.ToArray() ?? new int[] { }).Sum();
                var res2 = coords.SelectMany(c => c.SelectMany(cy => cy.SelectMany(cz => cz.Select(cw => cw ? 1 : 0)))).Sum();
            }
        }
    }
}
