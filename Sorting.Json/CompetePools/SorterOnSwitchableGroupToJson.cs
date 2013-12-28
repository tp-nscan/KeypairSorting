using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sorting.CompetePools;
using Sorting.Json.Sorters;

namespace Sorting.Json.CompetePools
{
    public class SorterOnSwitchableGroupToJson
    {
        public static SorterOnSwitchableGroupToJson ToJsonAdapter(ISorterOnSwitchableGroup sorterOnSwitchableGroup)
        {
            var chromosomeUintToJson = new SorterOnSwitchableGroupToJson
            {
                SwitchableGroupGuid = sorterOnSwitchableGroup.SwitchableGroupGuid,
                SorterToJson = SorterToJson.ToJsonAdapter(sorterOnSwitchableGroup.Sorter),
                Success = sorterOnSwitchableGroup.Success,
                SwitchesUsed = sorterOnSwitchableGroup.SwitchesUsed
                //SwitchUseList = sorterOnSwitchableGroup.SwitchUseList.ToList()
            };

            return chromosomeUintToJson;
        }

        public static string ToJsonString(ISorterOnSwitchableGroup sorterOnSwitchableGroup)
        {
            return JsonConvert.SerializeObject(ToJsonAdapter(sorterOnSwitchableGroup), Formatting.None);
        }

        public Guid SwitchableGroupGuid { get; set; }

        //public List<double> SwitchUseList { get; set; } 

        public SorterToJson SorterToJson { get; set; }

        public bool Success { get; set; }

        public int SwitchesUsed { get; set; }

        public static ISorterOnSwitchableGroup ToSorterOnSwitchableGroup(SorterOnSwitchableGroupToJson sorterOnSwitchableGroupToJson)
        {
            return SorterOnSwitchableGroup.Make
                (
                    sorter: SorterToJson.ToSorter(sorterOnSwitchableGroupToJson.SorterToJson),
                    switchableGroupGuid: sorterOnSwitchableGroupToJson.SwitchableGroupGuid,
                    //switchUseList: sorterOnSwitchableGroupToJson.SwitchUseList,
                    switchUseList: null,
                    success: sorterOnSwitchableGroupToJson.Success
                );
        }

        public static ISorterOnSwitchableGroup ToSorterOnSwitchableGroup(string serialized)
        {
            return ToSorterOnSwitchableGroup(
            JsonConvert.DeserializeObject<SorterOnSwitchableGroupToJson>(serialized));
        }
    }
}
