using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Chromosomes;
using Newtonsoft.Json;

namespace Genomic.Json.Chromosomes
{
    public class ChromosomeUintToJson
    {
        public static ChromosomeUintToJson ToJsonAdapter(IChromosomeUint chromosomeUint)
        {
            var chromosomeUintToJson = new ChromosomeUintToJson
            {
                Guid = chromosomeUint.Guid,
                MaxVal = chromosomeUint.MaxVal,
                Sequence = chromosomeUint.Sequence.ToList()
            };

            return chromosomeUintToJson;
        }

        public static string ToJsonString(IChromosomeUint chromosomeUint)
        {
            return JsonConvert.SerializeObject(ToJsonAdapter(chromosomeUint), Formatting.Indented);
        }

        public Guid Guid { get; set; }

        public uint MaxVal { get; set; }

        public List<uint> Sequence { get; set; }

        public static IChromosomeUint ToChromosomeUint(ChromosomeUintToJson chromosomeUintToJson)
        {
            return ChromosomeUint.Make
                (
                    guid: chromosomeUintToJson.Guid,
                    sequence: chromosomeUintToJson.Sequence,
                    maxVal: chromosomeUintToJson.MaxVal
                );
        }
    }

}
