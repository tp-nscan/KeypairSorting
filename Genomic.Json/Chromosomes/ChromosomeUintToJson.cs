using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Chromosomes;
using Newtonsoft.Json;

namespace Genomic.Json.Chromosomes
{
    public class ChromosomeUintToJson
    {
        public uint MaxVal { get; set; }

        public List<uint> Sequence { get; set; }

        public static IChromosomeUint ToChromosomeUint(ChromosomeUintToJson chromosomeUintToJson)
        {
            return ChromosomeUint.Make
                (
                    sequence: chromosomeUintToJson.Sequence,
                    maxVal: chromosomeUintToJson.MaxVal
                );
        }
    }


    public static class ChromosomeUintToJsonExt
    {
        public static ChromosomeUintToJson ToJsonAdapter(this IChromosomeUint chromosomeUint)
        {
            var chromosomeUintToJson = new ChromosomeUintToJson
            {
                MaxVal = chromosomeUint.MaxVal,
                Sequence = chromosomeUint.Sequence.ToList()
            };

            return chromosomeUintToJson;
        }

        public static string ToJsonString(this IChromosomeUint chromosomeUint)
        {
            return JsonConvert.SerializeObject(chromosomeUint.ToJsonAdapter(), Formatting.Indented);
        }

        public static IChromosomeUint ToChromosomeUint(this string chromosomeUintString)
        {
            var chromosomeUintToJson = JsonConvert.DeserializeObject<ChromosomeUintToJson>(chromosomeUintString);
            return chromosomeUintToJson.ToChromosomeUint();
        }

        public static IChromosomeUint ToChromosomeUint(this ChromosomeUintToJson chromosomeUintToJson)
        {
            return ChromosomeUint.Make
                (
                    sequence: chromosomeUintToJson.Sequence,
                    maxVal: chromosomeUintToJson.MaxVal
                );
        }
    }

}
