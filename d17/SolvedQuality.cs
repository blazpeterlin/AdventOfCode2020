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
    class SolvedQuality
    {
        public static void Solve()
        {
            var ih = InputHelper.LoadInputP(2020);
            var lns = ih.AsCharListOfLists().Select(lst => lst.Select(ch => ch == '#').ToList()).ToList();
            {
                int numSteps = 6;
                int xSize = lns.Count + numSteps * 2 + 2;
                int ySize = lns[0].Count + numSteps * 2 + 2;
                int zSize = 1 + numSteps * 2 + 2;
                var (minX, maxX) = (0 - xSize / 2, 0 + xSize / 2);
                var (minY, maxY) = (0 - ySize / 2, 0 + ySize / 2);
                var (minZ, maxZ) = (0 - zSize / 2, 0 + zSize / 2);
                Dictionary<(int x, int y, int z),bool> crds = FRng(minX, maxX + 1).FRng(minY, maxY + 1).FRng(minZ, maxZ + 1).ToDictionary(_ => _, _ =>false);//.FTupled(false, true).ToList();
                
                for (int xC = 0; xC < lns.Count; xC++)
                {
                    for (int yC = 0; yC < lns[xC].Count; yC++)
                    {
                        crds[(xC - lns.Count / 2, yC - lns[xC].Count / 2, 0)] = lns[xC][yC];
                    }
                }

                void step()
                {
                    Dictionary<(int, int, int), bool> newCoords = new();
                    foreach(var xyz in FRng(minX+1,maxX).FRng(minY+1,maxY).FRng(minZ+1,maxZ))
                    {
                        var (x, y, z) = xyz;
                        var numNeighbours =
                            FRng(x - 1, x + 2).FRng(y - 1, y + 2).FRng(z - 1, z + 2)
                            .Where(tpl => tpl != xyz)
                            .Select(tpl => crds[tpl] ? 1 : 0)
                            .Sum();

                        newCoords[xyz] = (crds[xyz], numNeighbours) switch
                        {
                            (true, int nn) when 2 <= nn && nn <= 3 => true,
                            (true, _) => false,
                            (false, 3) => true,
                            (false, _) => false,
                        };
                    }
                                

                    foreach (var kvp in newCoords)
                    {
                        crds[kvp.Key] = kvp.Value;
                    }
                }

                for (int i = 0; i < numSteps; i++)
                {
                    step();
                }

                var res1 = crds.Values.Select(v => v ? 1 : 0).Sum();
            }


            {
                int numSteps = 6;
                int xSize = lns.Count + numSteps * 2 + 2;
                int ySize = lns[0].Count + numSteps * 2 + 2;
                int zSize = 1 + numSteps * 2 + 2;
                int wSize = 1 + numSteps * 2 + 2;
                var (minX, maxX) = (0 - xSize / 2, 0 + xSize / 2);
                var (minY, maxY) = (0 - ySize / 2, 0 + ySize / 2);
                var (minZ, maxZ) = (0 - zSize / 2, 0 + zSize / 2);
                var (minW, maxW) = (0 - wSize / 2, 0 + wSize / 2);
                Dictionary<(int x, int y, int z, int w), bool> crds = FRng(minX, maxX + 1).FRng(minY, maxY + 1).FRng(minZ, maxZ + 1).FRng(minW, maxW+1).ToDictionary(_ => _, _ => false);//.FTupled(false, true).ToList();

                for (int xC = 0; xC < lns.Count; xC++)
                {
                    for (int yC = 0; yC < lns[xC].Count; yC++)
                    {
                        crds[(xC - lns.Count / 2, yC - lns[xC].Count / 2, 0, 0)] = lns[xC][yC];
                    }
                }

                void step()
                {
                    Dictionary<(int, int, int, int), bool> newCoords = new();
                    foreach (var xyzw in FRng(minX + 1, maxX).FRng(minY + 1, maxY).FRng(minZ + 1, maxZ).FRng(minW+1,maxW))
                    {
                        var (x, y, z, w) = xyzw;
                        var numNeighbours =
                            FRng(x - 1, x + 2).FRng(y - 1, y + 2).FRng(z - 1, z + 2).FRng(w-1,w+2)
                            .Where(tpl => tpl != xyzw)
                            .Select(tpl => crds[tpl] ? 1 : 0)
                            .Sum();

                        newCoords[xyzw] = (crds[xyzw], numNeighbours) switch
                        {
                            (true, int nn) when 2 <= nn && nn <= 3 => true,
                            (true, _) => false,
                            (false, 3) => true,
                            (false, _) => false,
                        };
                    }


                    foreach (var kvp in newCoords)
                    {
                        crds[kvp.Key] = kvp.Value;
                    }
                }

                for (int i = 0; i < numSteps; i++)
                {
                    step();
                }

                var res2 = crds.Values.Select(v => v ? 1 : 0).Sum();
            }
        }
    }
}
