using System;
using System.Collections.Generic;
using System.Linq;
using Genomic.Chromosomes;
using Newtonsoft.Json;
using SorterEvo.Genomes;
using Sorting.KeyPairs;

namespace SorterEvo.Json.Genomes
{
    public class SorterGenomeToJson
    {
        public static SorterGenomeToJson ToJsonAdapter(ISorterGenome sorterGenome)
        {
            var chromosomeUintToJson = new SorterGenomeToJson
            {
                Guid = sorterGenome.Guid,
                Sequence = sorterGenome.Chromosome.Sequence.ToList(),
                ChromosomeGuid = sorterGenome.Chromosome.Guid,
                KeyCount = sorterGenome.KeyCount,
                KeyPairCount = sorterGenome.KeyPairCount
            };

            return chromosomeUintToJson;
        }

        public Guid ChromosomeGuid { get; set; }

        public Guid Guid { get; set; }

        public Guid ParentGuid { get; set; }

        public int KeyCount { get; set; }

        public int KeyPairCount { get; set; }

        public List<uint> Sequence { get; set; }
    }

    public static class SorterGenomeToJsonExt
    {
        public static string ToJsonString(this ISorterGenome sorterGenome)
        {
            return JsonConvert.SerializeObject(SorterGenomeToJson.ToJsonAdapter(sorterGenome), Formatting.None);
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
                    chromosome: sorterGenomeToJson.Sequence.ToChromosomeUint
                                   (
                                        guid: sorterGenomeToJson.ChromosomeGuid,
                                        maxVal: (uint)KeyPairRepository.KeyPairSetSizeForKeyCount(sorterGenomeToJson.KeyCount)
                                   ), 
                    keyPairCount: sorterGenomeToJson.KeyPairCount
                );
        }
    }

}
