using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WpfUtils.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestReplace()
        {
            var observableCol = new ObservableCollection<string>();
            observableCol.Add("zero");
            observableCol.Add("first");
            observableCol.Add("second");
            observableCol.Add("third");
            Assert.IsTrue(observableCol.IndexOf("second")==2);

            observableCol.Replace("second", "newsecond");
            Assert.IsTrue(observableCol.IndexOf("newsecond") == 2);

            observableCol.Replace("zero", "newzero");
            Assert.IsTrue(observableCol.IndexOf("newzero") == 0);
        }

        [TestMethod]
        public void TestReplace2()
        {
            var foo = "hi";
            var res = foo.CompareTo("there");
        }

    }
}
