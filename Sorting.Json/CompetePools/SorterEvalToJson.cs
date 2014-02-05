using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sorting.CompetePools;
using Sorting.Json.Sorters;
using Sorting.Json.Switchables;

namespace Sorting.Json.CompetePools
{
    public class SorterEvalToJson
    {
        public Guid SwitchableGroupGuid { get; set; }

        public List<double> SwitchUseList { get; set; } 

        public SorterToJson SorterToJson { get; set; }

        public SwitchableGroupToJson CoveringSet { get; set; }

        public bool Success { get; set; }

        public int SwitchesUsed { get; set; }
    }

    public static class SorterEvalToJsonExt
    {
        public static SorterEvalToJson ToJsonAdapter(this ISorterEval sorterEval)
        {
            var sorterEvalToJson = new SorterEvalToJson
            {
                SwitchableGroupGuid = sorterEval.SwitchableGroupGuid,
                SorterToJson = sorterEval.Sorter.ToJsonAdapter(),
                CoveringSet = sorterEval.CoveringSet.ToJsonAdapter(),
                Success = sorterEval.Success,
                SwitchesUsed = sorterEval.SwitchUseCount,
                SwitchUseList = sorterEval.SwitchUseList.ToList()
            };

            return sorterEvalToJson;
        }

        public static string ToJsonString(this ISorterEval sorterEval)
        {
            return JsonConvert.SerializeObject(sorterEval.ToJsonAdapter(), Formatting.None);
        }

        public static ISorterEval ToSorterEval(this string serialized)
        {
            return JsonConvert.DeserializeObject<SorterEvalToJson>(serialized)
                              .ToSorterEval();
        }

        public static ISorterEval ToSorterEval(this SorterEvalToJson sorterGenomeToJson)
        {
            return SorterEval.Make
                (
                    sorter: sorterGenomeToJson.SorterToJson.ToSorter(),
                    switchableGroupGuid: sorterGenomeToJson.SwitchableGroupGuid,
                    switchUseList: sorterGenomeToJson.SwitchUseList,
                    success: sorterGenomeToJson.Success,
                    coveringSet: sorterGenomeToJson.CoveringSet.ToSwitchableGroup()
                );
        }
    }
}
