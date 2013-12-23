using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sorting.Sorters;
using Formatting = System.Xml.Formatting;

namespace Sorting.Json.Sorters
{
    public class SorterToJson
    {
        //public static SorterToJson ToJsonAdapter(ISorter sorter)
        //{
        //    var chromosomeUintToJson = new SorterToJson
        //    {
        //        Guid = sorter.Guid,
        //        Sequence = sorter.KeyPairs. Sequence.ToList()
        //    };

        //    return chromosomeUintToJson;
        //}

        //public static string ToJsonString(ISorter sorter)
        //{
        //    return JsonConvert.SerializeObject(ToJsonAdapter(sorter), Formatting.Indented);
        //}

        //public Guid Guid { get; set; }

        //public uint MaxVal { get; set; }

        //public List<uint> Sequence { get; set; }

        //public static ISorter ToSorter(SorterToJson chromosomeUintToJson)
        //{
        //    return Sorter.ToSorter(
        //            guid: chromosomeUintToJson.Guid,
        //            sequence: chromosomeUintToJson.Sequence,
        //            maxVal: chromosomeUintToJson.MaxVal
        //        );
        //}
    }
}
