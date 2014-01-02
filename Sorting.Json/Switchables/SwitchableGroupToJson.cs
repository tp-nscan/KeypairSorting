using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sorting.Switchables;

namespace Sorting.Json.Switchables
{
    public class SwitchableGroupToJson
    {
        public int KeyCount { get; set; }

        public Guid Guid { get; set; }

        public Type SwitchableDataType { get; set; }

        public List<string> SwitchableData { get; set; } 
    }

    public static class SwitchableGroupToJsonExt
    {
        public static SwitchableGroupToJson ToJsonAdapter(this ISwitchableGroup switchableGroup)
        {
            return new SwitchableGroupToJson
            {
                Guid = switchableGroup.Guid,
                KeyCount = switchableGroup.KeyCount,
                SwitchableDataType = switchableGroup.SwitchableDataType,
                SwitchableData = switchableGroup.SwitchableStrings.ToList()
            };
        }

        public static string ToJsonString(this ISwitchableGroup switchableGroup)
        {
            return JsonConvert.SerializeObject(switchableGroup.ToJsonAdapter(), Formatting.None);
        }

        public static ISwitchableGroup ToSwitchableGroup(this string switchableGroupString)
        {
            var switchableGroup = JsonConvert.DeserializeObject<SwitchableGroupToJson>(switchableGroupString);
            return switchableGroup.ToSwitchableGroup();
        }

        public static ISwitchableGroup ToSwitchableGroup(this SwitchableGroupToJson switchableGroupToJson)
        {
            if (switchableGroupToJson.SwitchableDataType == typeof(uint))
            {
                var swtichables = switchableGroupToJson.SwitchableData.Select(
                    t=> uint.Parse(t).ToSwitchableUint(switchableGroupToJson.KeyCount));

                return swtichables.ToSwitchableGroup
                    (
                        guid: switchableGroupToJson.Guid, 
                        keyCount:   switchableGroupToJson.KeyCount
                    );
            }

            if (switchableGroupToJson.SwitchableDataType == typeof(ulong))
            {
                var swtichables = switchableGroupToJson.SwitchableData.Select(
                    t => ulong.Parse(t).ToSwitchableUlong(switchableGroupToJson.KeyCount));

                return swtichables.ToSwitchableGroup
                    (
                        guid: switchableGroupToJson.Guid,
                        keyCount: switchableGroupToJson.KeyCount
                    );
            }

            if (switchableGroupToJson.SwitchableDataType == typeof(uint[]))
            {
                var swtichables = switchableGroupToJson.SwitchableData.Select(
                    t => JsonConvert.DeserializeObject<uint[]>(t).ToSwitchableIntArray());

                return swtichables.ToSwitchableGroup
                    (
                        guid: switchableGroupToJson.Guid,
                        keyCount: switchableGroupToJson.KeyCount
                    );
            }

            if (switchableGroupToJson.SwitchableDataType == typeof(bool[]))
            {
                var swtichables = switchableGroupToJson.SwitchableData.Select(
                    t => JsonConvert.DeserializeObject<bool[]>(t).ToSwitchableBitArray());

                return swtichables.ToSwitchableGroup
                    (
                        guid: switchableGroupToJson.Guid,
                        keyCount: switchableGroupToJson.KeyCount
                    );
            }


            throw new Exception(string.Format("data type {0} not  handled", switchableGroupToJson.SwitchableDataType));
        }
    }
}
