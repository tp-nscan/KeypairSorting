using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.Json.Sorters;
using Sorting.Sorters;
using Sorting.CompetePools;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace SorterEvo.Test.Workflows
{
    [TestClass]
    public class RandSorterGen
    {
        [TestMethod]
        public void TestMethod1()
        {
            Sample20(55555);
        }

        void Sample5(int seed)
        {
            SorterSampler(seed, 5, 400, 2, 18);
        }

        void Sample6(int seed)
        {
            SorterSampler(seed, 6, 500, 2, 24);
        }

        void Sample7(int seed)
        {
            SorterSampler(seed, 7, 600, 12, 12);
        }

        void Sample8(int seed)
        {
            SorterSampler(seed, 8, 800, 17, 17);
        }

        void Sample9(int seed)
        {
            SorterSampler(seed, 9, 800, 22, 18);
        }

        void Sample10(int seed)
        {
            SorterSampler(seed, 10, 800, 30, 20);
        }

        void Sample11(int seed)
        {
            SorterSampler(seed, 11, 1000, 38, 21);
        }

        void Sample12(int seed)
        {
            SorterSampler(seed, 12, 1200, 46, 24);
        }

        void Sample13(int seed)
        {
            SorterSampler(seed, 13, 1400, 54, 28);
        }

        void Sample14(int seed)
        {
            SorterSampler(seed, 14, 1600, 65, 30);
        }

        void Sample15(int seed)
        {
            SorterSampler(seed, 15, 2000, 75, 35);
        }

        void Sample16(int seed)
        {
            SorterSampler(seed, 16, 2200, 85, 40);
        }

        void Sample18(int seed)
        {
            SorterSampler(seed, 18, 2600, 100, 60);
        }

        void Sample20(int seed)
        {
            SorterSampler(seed, 20, 3200, 125, 100);
        }

        void Sample24(int seed)
        {
            SorterSampler(seed, 24, 4000, 125, 200);
        }

        public void SorterSampler(int seed, int keyCount, int switches, int repStart, int repRange)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            // //stopwatch.Stop();

            // //Debug.WriteLine("Time(ms): {0}", stopwatch.ElapsedMilliseconds);

            var histo = new Dictionary<int, int>();
            for (var i = 0; i < 500; i++)
            {
                histo[i] = 0;
            }

            var sortFails = 0;
            while (true)
            {
                var count = 0;
                foreach (var results in RandomSorterTests(seed, keyCount, switches))
                {
                    foreach (var sorterOnSwitchableGroup in results)
                    {
                        if (!sorterOnSwitchableGroup.Success)
                        {
                            sortFails++;
                        }
                        histo[sorterOnSwitchableGroup.SwitchesUsed] = histo[sorterOnSwitchableGroup.SwitchesUsed] + 1;

                        if ((sorterOnSwitchableGroup.SwitchesUsed < 145) || (sorterOnSwitchableGroup.SwitchesUsed > 169))
                        {
                            Debug.WriteLine("****\t{0}\t{1}\t{2}", sorterOnSwitchableGroup.SwitchesUsed, sorterOnSwitchableGroup.Success, SorterToJson.ToJsonString(sorterOnSwitchableGroup.Sorter));
                        }
                    }

                    if (count % 1 == 0)
                    {
                        Debug.WriteLine("\n\n\n\n\n\n");
                        Debug.WriteLine("key count:" + keyCount);
                        Debug.WriteLine("Seed:" + seed);
                        Debug.WriteLine("Sort fails:" + sortFails);
                        Debug.WriteLine("count:" + count);
                        Debug.WriteLine("time:" + stopwatch.Elapsed);
                        Debug.WriteLine(
                                            Enumerable.Range(repStart, repRange)
                                                      .Select(c => new Tuple<int, int>(c, histo[c]))
                                                      .Aggregate(String.Empty, (o, n) => o + "\n" + n.Item1 + "\t" + n.Item2)
                                        );
                    }

                    count++;
                }
            }
        }

        public IEnumerable<ISorter> RandomSorters(int seed, int keyCount, int switchLength)
        {
            var rando = Rando.Fast(seed);
            while (true)
            {
                yield return rando.ToSorter(keyCount, switchLength, Guid.NewGuid());
            }
        }


        public IEnumerable<IEnumerable<ISorterOnSwitchableGroup>> RandomSorterTests(int seed, int keyCount,
            int switchLength)
        {
            KeyPairSwitchSet.Make<uint>(keyCount);
            var testSet = Switchable.AllSwitchablesForKeyCount(keyCount).ToSwitchableGroup(Guid.NewGuid(), keyCount);
            foreach (var chunk in RandomSorters(seed,keyCount,switchLength).Chunk(16))
            {
                yield return chunk.AsParallel().Select(s => s.Sort(testSet));
            }
        }

    }
}
