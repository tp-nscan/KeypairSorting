using System;
using System.Dynamic;
using System.Web.Script.Serialization;
using MathUtils.Expando;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Expando
{
    [TestClass]
    public class ExpandoJsonConverterFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] {new ExpandoJsonConverter()});
            dynamic testObj = new ExpandoObject();
            testObj.Name = "Ralph";

            var serialized = serializer.Serialize(testObj);

            var deserialized = serializer.Deserialize<ExpandoObject>(serialized);
        }
    }
}
