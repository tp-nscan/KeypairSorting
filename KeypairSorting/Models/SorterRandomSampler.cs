using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public static string ToHistogramReport(this Dictionary<int, int> histo, int? minVal, int? maxVal)
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

        public static SorterSamplerResults SorterSampler(
                int keyCount,
                int switchCount,
                int histogramMin,
                int histogramMax, 
                int seed, 
                int repCount, 
                int lowRangeMax, 
                int highRangeMin,
                CancellationToken cancellationToken
            )
        {
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            // //stopwatch.Stop();

            var sorterOnSwitchableGroups = new List<ISorterEval>();
            var histo = new Dictionary<int, int>();
            for (var i = 0; i < 500; i++)
            {
                histo[i] = 0;
            }

            var sortFails = 0;

            foreach (var results in RandomSorterTests(seed, keyCount, switchCount).Take(repCount))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new SorterSamplerResults { WasCancelled = true };
                }

                foreach (var sorterOnSwitchableGroup in results)
                {
                    if (!sorterOnSwitchableGroup.Success)
                    {
                        sortFails++;
                    }

                    histo[sorterOnSwitchableGroup.SwitchUseCount] = histo[sorterOnSwitchableGroup.SwitchUseCount] + 1;

                    if ((sorterOnSwitchableGroup.SwitchUseCount < lowRangeMax) || (sorterOnSwitchableGroup.SwitchUseCount > highRangeMin))
                    {
                        sorterOnSwitchableGroups.Add(sorterOnSwitchableGroup);
                    }
                }
            }

            return new SorterSamplerResults { SwitchUseHistogram = histo, SwitchResults = sorterOnSwitchableGroups, SortFails= sortFails };
        }

        static IEnumerable<ISorter> RandomSorters(int seed, int keyCount, int switchLength)
        {
            var rando = Rando.Fast(seed);
            var randoK = rando.Spawn();
            while (true)
            {
                yield return rando.ToSorter(keyCount, switchLength, randoK.NextGuid());
            }
        }


        static IEnumerable<IEnumerable<ISorterEval>> RandomSorterTests(int seed, int keyCount,
            int switchLength)
        {
            KeyPairSwitchSet.Make<uint>(keyCount);
            var testSet = Switchable.AllSwitchablesForKeyCount(keyCount).ToSwitchableGroup(
                guid: SwitchableGroup.GuidOfAllSwitchableGroupsForKeyCount(keyCount), 
                keyCount:keyCount);

            foreach (var chunk in RandomSorters(seed, keyCount, switchLength).Chunk(16))
            {
                yield return chunk.AsParallel().Select(s => s.Sort(testSet));
            }
        }


    }

    public class SorterSamplerResults
    {
        public IReadOnlyDictionary<int, int> SwitchUseHistogram { get; set; }
        public IReadOnlyList<ISorterEval> SwitchResults { get; set; }
        public int SortFails { get; set; }
        public bool WasCancelled { get; set; }
    }
}
