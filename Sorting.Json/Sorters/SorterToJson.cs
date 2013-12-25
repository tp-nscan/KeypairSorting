using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace Sorting.Json.Sorters
{
    public class SorterToJson
    {
        public static SorterToJson ToJsonAdapter(ISorter sorter)
        {
            var chromosomeUintToJson = new SorterToJson
            {
                Guid = sorter.Guid,
                Sequence = sorter.KeyPairs.Select(kp=>kp.Index).ToList(),
                KeyCount = sorter.KeyCount
            };

            return chromosomeUintToJson;
        }

        public static string ToJsonString(ISorter sorter)
        {
            return JsonConvert.SerializeObject(ToJsonAdapter(sorter), Formatting.None);
        }

        public Guid Guid { get; set; }

        public int KeyCount { get; set; }

        public List<int> Sequence { get; set; }

        public static ISorter ToSorter(SorterToJson sorterToJson)
        {
            return Sorter.ToSorter(
                    guid: sorterToJson.Guid,
                    keyPairs: sorterToJson.Sequence.Select(KeyPairRepository.AtIndex),
                    keyCount: sorterToJson.KeyCount
                );
        }
    }
}
