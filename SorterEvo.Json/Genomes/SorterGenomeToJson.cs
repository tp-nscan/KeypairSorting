using System;
using Genomic.Json.Chromosomes;
using Newtonsoft.Json;
using SorterEvo.Genomes;

namespace SorterEvo.Json.Genomes
{
    public class SorterGenomeToJson
    {
        public static SorterGenomeToJson ToJsonAdapter(ISorterGenome sorterGenome)
        {
            var chromosomeUintToJson = new SorterGenomeToJson
            {
                Guid = sorterGenome.Guid,
                ChromosomeUintToJson = sorterGenome.Chromosome.ToJsonAdapter(),
                KeyCount = sorterGenome.KeyCount,
                KeyPairCount = sorterGenome.KeyPairCount
            };

            return chromosomeUintToJson;
        }

        public Guid Guid { get; set; }

        public Guid ParentGuid { get; set; }

        public int KeyCount { get; set; }

        public int KeyPairCount { get; set; }

        public ChromosomeUintToJson ChromosomeUintToJson { get; set; }
    }

    public static class SorterGenomeToJsonExt
    {
        public static SorterGenomeToJson ToJsonAdapter(this ISorterGenome sorterGenome)
        {
            var chromosomeUintToJson = new SorterGenomeToJson
            {
                Guid = sorterGenome.Guid,
                ChromosomeUintToJson = sorterGenome.Chromosome.ToJsonAdapter(),
                KeyCount = sorterGenome.KeyCount,
                KeyPairCount = sorterGenome.KeyPairCount
            };

            return chromosomeUintToJson;
        }

        public static string ToJsonString(this ISorterGenome sorterGenome)
        {
            return JsonConvert.SerializeObject(sorterGenome.ToJsonAdapter(), Formatting.None);
        }

        public static ISorterGenome ToSorterGenome(this string sorterGenomeString)
        {
            var sorterGenomeToJson = JsonConvert.DeserializeObject<SorterGenomeToJson>(sorterGenomeString);
            return sorterGenomeToJson.ToSorterGenome();
        }

        public static ISorterGenome ToSorterGenome(this SorterGenomeToJson sorterGenomeToJson)
        {
            return SorterGenome.Make(
                    guid: sorterGenomeToJson.Guid,
                    keyCount: sorterGenomeToJson.KeyCount,
                    parentGuid:sorterGenomeToJson.ParentGuid,
                    chromosome: sorterGenomeToJson.ChromosomeUintToJson.ToChromosomeUint(),
                    keyPairCount: sorterGenomeToJson.KeyPairCount
                );
        }
    }
}
