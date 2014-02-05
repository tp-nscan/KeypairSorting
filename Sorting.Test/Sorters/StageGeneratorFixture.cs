using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MathUtils.Collections;
using MathUtils.Rand;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sorting.KeyPairs;
using Sorting.Sorters.StageGenerators;
using Sorting.Switchables;

namespace Sorting.Test.Sorters
{
    [TestClass]
    public class StageGeneratorFixture
    {
        [TestMethod]
        public void TestMethod1()
        {
            var sorterStage = TestKeyPairList().ToReducedSorterStage(Sorting.TestData.Switchables.SwitchableSet);
        }

        IReadOnlyList<IKeyPair> TestKeyPairList()
        {
            return TestKeyPairs().ToList();
        }

        [TestMethod]
        public void TestMethod2()
        {
            const int keyCount = 13;
            for (var numKeyPairs = 4; numKeyPairs < 30; numKeyPairs+=2)
            {
                for (var numSwitchables = 10; numSwitchables < 100; numSwitchables+=10)
                {
                    var rando = Rando.Fast(numSwitchables + 377);
                    IReadOnlyList<IKeyPair> sorterStage = KeyPairRepository.RandomKeyPairs(13, numKeyPairs, rando.NextInt()).ToList();
                    for (var rep = 0; rep < 10; rep++)
                    {
                        var reducedSorterStage =
                            sorterStage.ToReducedSorterStage(
                            rando.Spawn().ToSwitchableGroup<uint>(Guid.NewGuid(), keyCount, numSwitchables)
                            );
                        //if (reducedSorterStage.KeyPairs.Count > 6)
                        //{
                        //    System.Diagnostics.Debug.WriteLine(sorterStage.Aggregate("\n", (o, n) => o + " [" + n.LowKey + "," + n.HiKey + "]"));
                        //    System.Diagnostics.Debug.WriteLine(reducedSorterStage.KeyPairs.Aggregate(String.Empty, (o,n)=> o + " [" + n.LowKey + "," + n.HiKey + "]" ));
                        //}
                        System.Diagnostics.Debug.WriteLine(String.Format("{0}\t{1}\t{2}\t{3}\t{4}", 
                            numKeyPairs, 
                            numSwitchables, 
                            reducedSorterStage.KeyPairs.Count,
                            sorterStage.Aggregate("", (o, n) => o + " [" + n.LowKey + "," + n.HiKey + "]"),
                            reducedSorterStage.KeyPairs.Aggregate(String.Empty, (o, n) => o + " [" + n.LowKey + "," + n.HiKey + "]")
                            ));
                    }
                }
            }
        }

        IEnumerable<IKeyPair> TestKeyPairs()
        {
            yield return KeyPairRepository.ForKeys(0, 1);
            yield return KeyPairRepository.ForKeys(10, 11);
            yield return KeyPairRepository.ForKeys(3, 9);
            yield return KeyPairRepository.ForKeys(10, 12);
            yield return KeyPairRepository.ForKeys(3, 5);
            yield return KeyPairRepository.ForKeys(0, 5);

            yield return KeyPairRepository.ForKeys(1, 5);
            yield return KeyPairRepository.ForKeys(2, 4);

            yield return KeyPairRepository.ForKeys(7, 8);
            yield return KeyPairRepository.ForKeys(0, 5);

            yield return KeyPairRepository.ForKeys(8, 9);
            yield return KeyPairRepository.ForKeys(4, 7);

            yield return KeyPairRepository.ForKeys(2, 6);
            yield return KeyPairRepository.ForKeys(6, 8);

            yield return KeyPairRepository.ForKeys(5, 7);
            yield return KeyPairRepository.ForKeys(1, 11);

        }
    }
}
