using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Genomic.Json.Chromosomes
{
    public class JsonConverterForChromosome : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}

//public class JsonConverterForSwitchable : JsonConverter
//{
//    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//    {
//        serializer.Serialize(writer, value);
//    }

//    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//    {
//        var jArray = JArray.Load(reader);

//        var retList = new List<object>();

//        for (var i = 0; i < jArray.Count; i++)
//        {
//            var jObject = jArray[i];

//            var fv = (SwitchableType)Enum.Parse(typeof(SwitchableType), (string)jObject["SwitchableType"]);
//            switch (fv)
//            {
//                case SwitchableType.BitArray:
//                    retList.Add(serializer.Deserialize<SwitchableBitArrayToJson>(jObject.CreateReader()));
//                    break;
//                case SwitchableType.IntArray:
//                    retList.Add(serializer.Deserialize<SwitchableIntArrayToJson>(jObject.CreateReader()));
//                    break;
//                case SwitchableType.Short:
//                    retList.Add(serializer.Deserialize<SwitchableShortToJson>(jObject.CreateReader()));
//                    break;
//                default:
//                    throw new Exception("SwitchableType not handled");
//            }
//        }

//        return retList;
//    }

//    public override bool CanConvert(Type objectType)
//    {
//        return true;
//    }
//}