using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Sorting.CompetePools;
using Sorting.Sorters;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace KeypairSorting.Models
{
    public static class SorterRandomSampler
    {
        static SorterSamplerParams GetSorterSamplerParams(int keyCount)
        {
            if (keyCount == 5)
            {
                return new SorterSamplerParams {KeyCount = 5, LowRangeMax = 7, HiRangeMin = 11, SwitchCount = 400 };
            }
            if (keyCount == 6)
            {
                return new SorterSamplerParams { KeyCount = 6, LowRangeMax = 11, HiRangeMin = 16, SwitchCount = 500 };
            }
            if (keyCount == 7)
            {
                return new SorterSamplerParams { KeyCount = 7, LowRangeMax = 16, HiRangeMin = 22, SwitchCount = 600 };
            }
            if (keyCount == 8)
            {
                return new SorterSamplerParams { KeyCount = 8, LowRangeMax = 19, HiRangeMin = 29, SwitchCount = 700 };
            }
            if (keyCount == 9)
            {
                return new SorterSamplerParams { KeyCount = 9, LowRangeMax = 25, HiRangeMin = 38, SwitchCount = 900 };
            }
            if (keyCount == 10)
            {
                return new SorterSamplerParams { KeyCount = 10, LowRangeMax = 30, HiRangeMin = 49, SwitchCount = 900 };
            }
            if (keyCount == 11)
            {
                return new SorterSamplerParams { KeyCount = 11, LowRangeMax = 38, HiRangeMin = 59, SwitchCount = 1000 };
            }
            if (keyCount == 12)
            {
                return new SorterSamplerParams { KeyCount = 12, LowRangeMax = 46, HiRangeMin = 70, SwitchCount = 1200 };
            }
            if (keyCount == 13)
            {
                return new SorterSamplerParams { KeyCount = 13, LowRangeMax = 54, HiRangeMin = 81, SwitchCount = 1400 };
            }
            if (keyCount == 14)
            {
                return new SorterSamplerParams { KeyCount = 14, LowRangeMax = 65, HiRangeMin = 94, SwitchCount = 1600 };
            }
            if (keyCount == 15)
            {
                return new SorterSamplerParams { KeyCount = 15, LowRangeMax = 75, HiRangeMin = 109, SwitchCount = 2000 };
            }
            if (keyCount == 16)
            {
                return new SorterSamplerParams { KeyCount = 16, LowRangeMax = 85, HiRangeMin = 119, SwitchCount = 2200 };
            }
            if (keyCount == 17)
            {
                return new SorterSamplerParams { KeyCount = 17, LowRangeMax = 95, HiRangeMin = 148, SwitchCount = 2400 };
            }
            if (keyCount == 18)
            {
                return new SorterSamplerParams { KeyCount = 18, LowRangeMax = 105, HiRangeMin = 162, SwitchCount = 2600 };
            }
            if (keyCount == 19)
            {
                return new SorterSamplerParams { KeyCount = 19, LowRangeMax = 118, HiRangeMin = 174, SwitchCount = 3000 };
            }
            if (keyCount == 20)
            {
                return new SorterSamplerParams { KeyCount = 20, LowRangeMax = 132, HiRangeMin = 184, SwitchCount = 3200 };
            }
            if (keyCount == 24)
            {
                return new SorterSamplerParams { KeyCount = 24, LowRangeMax = 125, HiRangeMin = 7, SwitchCount = 4000 };
            }
            if (keyCount == 32)
            {
                return new SorterSamplerParams { KeyCount = 32, LowRangeMax = 175, HiRangeMin = 500, SwitchCount = 8000 };
            }

            throw new Exception("Keycount:" + keyCount + " not handled");
        }

        public static SorterSamplerResults SorterSampler(int keyCount, int seed, int repCount)
        {

            return SorterSampler(GetSorterSamplerParams(keyCount), seed, repCount);

        }

        public static string ToReport(this Dictionary<int, int> histo, int? minVal, int? maxVal)
        {
            var filteredDictionary = histo;

            if (minVal.HasValue)
            {
                filteredDictionary = filteredDictionary.Where(t => t.Key > minVal.Value)
                    .ToDictionary(f => f.Key, f => f.Value);
            }

            if (maxVal.HasValue)
            {
                filteredDictionary = filteredDictionary.Where(t => t.Key < maxVal.Value)
                    .ToDictionary(f => f.Key, f => f.Value);
            }

            return filteredDictionary.Aggregate(String.Empty, (o,n) => o + "\n" + n.Key + "\t" + n.Value);
        }

        static SorterSamplerResults SorterSampler(SorterSamplerParams sorterSamplerParams, int seed, int repCount)
        {
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            // //stopwatch.Stop();

            var sorterOnSwitchableGroups = new List<ISorterOnSwitchableGroup>();
            var histo = new Dictionary<int, int>();
            for (var i = 0; i < 500; i++)
            {
                histo[i] = 0;
            }

            var sortFails = 0;

            for (int i = 0; i < repCount; i++)
            {
                foreach (var results in RandomSorterTests(seed, sorterSamplerParams.KeyCount, sorterSamplerParams.SwitchCount))
                {
                    foreach (var sorterOnSwitchableGroup in results)
                    {
                        if (!sorterOnSwitchableGroup.Success)
                        {
                            sortFails++;
                        }

                        histo[sorterOnSwitchableGroup.SwitchesUsed] = histo[sorterOnSwitchableGroup.SwitchesUsed] + 1;

                        if ((sorterOnSwitchableGroup.SwitchesUsed < sorterSamplerParams.LowRangeMax) || (sorterOnSwitchableGroup.SwitchesUsed > sorterSamplerParams.HiRangeMin))
                        {
                            sorterOnSwitchableGroups.Add(sorterOnSwitchableGroup);
                        }
                    }
                }
            }

            return new SorterSamplerResults() { SwitchUseHistogram = histo, SwitchResults = sorterOnSwitchableGroups };
        }

        static IEnumerable<ISorter> RandomSorters(int seed, int keyCount, int switchLength)
        {
            var rando = Rando.Fast(seed);
            while (true)
            {
                yield return rando.ToSorter(keyCount, switchLength, Guid.NewGuid());
            }
        }


        static IEnumerable<IEnumerable<ISorterOnSwitchableGroup>> RandomSorterTests(int seed, int keyCount,
            int switchLength)
        {
            KeyPairSwitchSet.Make<uint>(keyCount);
            var testSet = Switchable.AllSwitchablesForKeyCount(keyCount).ToSwitchableGroup(Guid.NewGuid(), keyCount);
            foreach (var chunk in RandomSorters(seed, keyCount, switchLength).Chunk(16))
            {
                yield return chunk.AsParallel().Select(s => s.Sort(testSet));
            }
        }


    }

    class SorterSamplerParams
    {
        public int KeyCount { get; set; }
        public int SwitchCount { get; set; }
        public int LowRangeMax { get; set; }
        public int HiRangeMin { get; set; }
    }

    public class SorterSamplerResults
    {
        public IReadOnlyDictionary<int, int> SwitchUseHistogram { get; set; }
        public IReadOnlyList<ISorterOnSwitchableGroup> SwitchResults { get; set; }
    }
}
