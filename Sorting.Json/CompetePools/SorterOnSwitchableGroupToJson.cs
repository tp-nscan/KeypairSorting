using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sorting.CompetePools;
using Sorting.KeyPairs;
using Sorting.Sorters;

namespace Sorting.Json.CompetePools
{
    //public class SorterOnSwitchableGroupToJson
    //{
    //    public static SorterOnSwitchableGroupToJson ToJsonAdapter(ISorterOnSwitchableGroup sorter)
    //    {
    //        var chromosomeUintToJson = new SorterOnSwitchableGroupToJson
    //        {
    //            Guid = sorter.Guid,
    //            Sequence = sorter.KeyPairs.Select(kp=>kp.Index).ToList(),
    //            KeyCount = sorter.KeyCount
    //        };

    //        return chromosomeUintToJson;
    //    }

    //    public static string ToJsonString(ISorter sorter)
    //    {
    //        return JsonConvert.SerializeObject(ToJsonAdapter(sorter), Formatting.None);
    //    }

    //    public Guid Guid { get; set; }

    //    public int KeyCount { get; set; }

    //    public List<int> Sequence { get; set; }

    //    public static ISorterOnSwitchableGroup ToSorterOnSwitchableGroup(SorterOnSwitchableGroupToJson sorterToJson)
    //    {
    //        return SorterOnSwitchableGroup.Make
    //            (
                
    //            )
    //        //return Sorter.ToSorter(
    //        //        guid: sorterToJson.Guid,
    //        //        keyPairs: sorterToJson.Sequence.Select(KeyPairRepository.AtIndex),
    //        //        keyCount: sorterToJson.KeyCount
    //        //    );
    //    }
    //}
}
