using System;
using System.Collections.Immutable;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathUtils.Tests.Collections
{
    [TestClass]
    public class ListPropigatorFixture
    {
        [TestMethod]
        public void TestListPropigatorDelete()
        {
            const int startLen = 40;
            const int finalLen = 30;

            var lp = new ListPropigator<int>
                (
                    deletor: Deletor,
                    deletionFrequency: 0.5,
                    inserter: Inserter,
                    insertionFrequency: 0.5,
                    mutator: Mutator,
                    mutationFrequency: 0.0,
                    finalIndex: finalLen
                );

            var randy = Rando.Fast(123);

            var track = new Tuple<IImmutableList<int>, int>
                (
                    ImmutableList<int>.Empty.AddRange(Enumerable.Range(0, startLen)),
                    0
                );

            while (track.Item2 < lp.FinalIndex)
            {
                track = lp.Propigate(track.Item1, randy, track.Item2);
            }

            Assert.AreEqual(track.Item1.Count, finalLen);
        }

        [TestMethod]
        public void TestListPropigatorInsert()
        {
            const int startLen = 30;
            const int finalLen = 40;

            var lp = new ListPropigator<int>
                (
                    deletor: Deletor,
                    deletionFrequency: 0.0,
                    inserter: Inserter,
                    insertionFrequency: 1.0,
                    mutator: Mutator,
                    mutationFrequency: 0.0,
                    finalIndex: finalLen
                );

            var randy = Rando.Fast(123);

            var track = new Tuple<IImmutableList<int>, int>
                (
                    ImmutableList<int>.Empty.AddRange(Enumerable.Range(0, startLen)),
                    0
                );

            while (track.Item2 < lp.FinalIndex)
            {
                track = lp.Propigate(track.Item1, randy, track.Item2);
            }

            Assert.AreEqual(track.Item1.Count, finalLen);
        }

        [TestMethod]
        public void TestListPropigatorMutate()
        {
            const int startLen = 40;
            const int finalLen = 40;

            var lp = new ListPropigator<int>
                (
                    deletor: Deletor,
                    deletionFrequency: 0.0,
                    inserter: Inserter,
                    insertionFrequency: 0.0,
                    mutator: Mutator,
                    mutationFrequency: 1.0,
                    finalIndex: finalLen
                );

            var randy = Rando.Fast(123);

            var track = new Tuple<IImmutableList<int>, int>
                (
                    ImmutableList<int>.Empty.AddRange(Enumerable.Range(0, startLen)),
                    0
                );

            while (track.Item2 < lp.FinalIndex)
            {
                track = lp.Propigate(track.Item1, randy, track.Item2);
            }

            Assert.AreEqual(track.Item1.Count, finalLen);
            Assert.IsTrue(track.Item1.All(t=>t==55));
        }

        Func<IImmutableList<int>, int, IImmutableList<int>> Deletor
        {
            get { return (l, i) => l.RemoveAt(i); }
        }

        Func<IImmutableList<int>, int, IImmutableList<int>> Inserter
        {
            get { return (l, i) => l.Insert(i, 999); }
        }

        Func<IImmutableList<int>, int, IImmutableList<int>> Mutator
        {
            get { return (l, i) => l.SetItem(i, 55); }
        }
    }
}
