using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SorterEvo.Evals;
using SorterEvo.Json.Genomes;
using Sorting.Json.CompetePools;

namespace SorterEvo.Json.Evals
{
    public class SorterGenomeEvalToJson
    {
        public SorterGenomeToJson SorterGenomeToJson { get; set; }

        public IImmutableStack<Guid> ParentGuids { get; set; }

        public SorterEvalToJson SorterEvalToJson { get; set; }
    }

    public static class SorterGenomeEvalToJsonExt
    {
        public static SorterGenomeEvalToJson ToJsonAdapter(this ISorterGenomeEval sorterGenomeEval)
        {
            var chromosomeUintToJson = new SorterGenomeEvalToJson
            {
                SorterGenomeToJson = sorterGenomeEval.SorterGenome.ToJsonAdapter(),
                SorterEvalToJson = sorterGenomeEval.SorterEval.ToJsonAdapter(),
                ParentGuids = sorterGenomeEval.ParentGuids
            };

            return chromosomeUintToJson;
        }

        public static string ToJsonString(this ISorterGenomeEval sorterGenome)
        {
            return JsonConvert.SerializeObject(sorterGenome.ToJsonAdapter(), Formatting.None);
        }

        public static ISorterGenomeEval ToSorterGenomeEval(this string sorterGenomeString)
        {
            var sorterGenomeEvalToJson = JsonConvert.DeserializeObject<SorterGenomeEvalToJson>(sorterGenomeString);
            return sorterGenomeEvalToJson.ToSorterGenomeEval();
        }

        public static ISorterGenomeEval ToSorterGenomeEval(this SorterGenomeEvalToJson sorterGenomeEvalToJson)
        {
            return SorterGenomeEval.Make(
                    sorterGenome: sorterGenomeEvalToJson.SorterGenomeToJson.ToSorterGenome(),
                    parentGuids: sorterGenomeEvalToJson.ParentGuids,
                    sorterEval: sorterGenomeEvalToJson.SorterEvalToJson.ToSorterEval()
                );
        }
    }
}
