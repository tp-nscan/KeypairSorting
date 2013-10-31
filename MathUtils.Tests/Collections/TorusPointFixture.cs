using System;
using System.Linq;
using MathUtils.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Collections
{
    [TestClass]
    public class TorusPointFixture
    {
        [TestMethod]
        public void TestNeighbors()
        {
            var tp = new TorusPoint(3, 3, 5, 5);

            var nbrs4 = tp.FourNeighbors.Select(T => new Tuple<int, int>(T.X, T.Y)).ToList();
            var nbrs8 = tp.EightNeighbors.Select(T => new Tuple<int, int>(T.X, T.Y)).ToList();

            var tp2 = new TorusPoint(0, 4, 5, 5);

            var nbrsb4 = tp2.FourNeighbors.Select(T=> new Tuple<int,int>(T.X, T.Y)) .ToList();
            var nbrsb8 = tp2.EightNeighbors.Select(T => new Tuple<int, int>(T.X, T.Y)).ToList();

        }
    }
}
