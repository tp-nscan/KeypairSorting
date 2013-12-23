using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sorting.KeyPairs;

namespace Sorting.Json.Sorters
{
    public class KeyPairToJson
    {
        //public static KeyPairToJson ToJsonAdapter(KeyPair keyPair)
        //{
        //    var chromosomeUintToJson = new KeyPairToJson
        //    {
        //        Sequence = keyPair.KeyPairs.Sequence.ToList()
        //    };

        //    return chromosomeUintToJson;
        //}

        //public static string ToJsonString(KeyPair sorter)
        //{
        //    return JsonConvert.SerializeObject(ToJsonAdapter(sorter), Formatting.Indented);
        //}

        //public Guid Guid { get; set; }

        //public uint MaxVal { get; set; }

        //public List<uint> Sequence { get; set; }

        //public static KeyPair ToKeyPair(ChromosomeUintToJson chromosomeUintToJson)
        //{
        //    return ChromosomeUint.Make
        //        (
        //            guid: chromosomeUintToJson.Guid,
        //            sequence: chromosomeUintToJson.Sequence,
        //            maxVal: chromosomeUintToJson.MaxVal
        //        );
        //}

    }
}
