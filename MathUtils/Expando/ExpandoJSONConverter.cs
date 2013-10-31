using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Web.Script.Serialization;

namespace MathUtils.Expando
{
    /// <summary>
    /// Allows JSON serialization of Expando objects into expected results (e.g., "x: 1, y: 2") instead of the default dictionary serialization.
    /// </summary>
    public class ExpandoJsonConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            // See full-source link for this extension method (/Helpers/IDictionaryExtensions.cs)
            return dictionary.ToExpando();
        }
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var dictionary = obj as IDictionary<string, object>;
            return dictionary.ToDictionary(item => item.Key, item => item.Value);
        }
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new ReadOnlyCollection<Type>(new[] { typeof(ExpandoObject) });
            }
        }
    }
}
