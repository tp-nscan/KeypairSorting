using System;
using System.Collections.Generic;
using System.Linq;
using Sorting.KeyPairs;
using Sorting.Switchables;
using Sorting.SwitchFunctionSets;

namespace Sorting.Sorters.StageGenerators
{
    public interface IStageGenerator<T>
    {
        ISorterStage MakeSorterStage<T>(IReadOnlyList<IKeyPair> keyPairs, ISwitchableGroup<T> switchableGroup);
    }

    public static class StageGenerator
    {
        public static ISorterStage ToReducedSorterStage<T>(this IReadOnlyList<IKeyPair> keyPairs,
            ISwitchableGroup<T> switchableGroup)
        {
            var switchSet = KeyPairSwitchSet.Make<T>(switchableGroup.KeyCount);
            var dependentKeyPairs = keyPairs.ToDependentKeyPairs().ToList();
            foreach (var switchable in switchableGroup.Switchables)
            {
                var currentItem = switchable.Item;
                for (var i = 0; i < dependentKeyPairs.Count; i++)
                {
                    var currentDp = dependentKeyPairs[i];
                    if (currentDp.IsDisabled)
                    {
                        continue;
                    }
                    if (switchSet.IsSorted(currentItem))
                    {
                        break;
                    }
                    var res = switchSet.SwitchFunction(currentDp)(currentItem);
                    currentItem = res.Item1;
                    if (res.Item2)
                    {
                        if (! currentDp.IsUsed)
                        {
                            currentDp.IsUsed = true;
                            currentDp.DisableDependentKeyPairs();
                        }
                        if (switchSet.IsSorted(currentItem))
                        {
                            break;
                        }
                    }
                }
            }

            IReadOnlyList<IKeyPair> reducedKeyPairs = dependentKeyPairs.Reduce().ToList();

            return SorterStage.Make(switchableGroup.KeyCount, reducedKeyPairs);

        }
    }
}
