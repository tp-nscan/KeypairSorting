using System;
using System.Collections.Generic;
using System.Linq;
using MathUtils.Collections;
using MathUtils.Rand;

namespace Sorting.Switchables
{
    public interface ISwitchableGroup<out T> : ISwitchableGroup
    {
        IReadOnlyList<ISwitchable<T>> Switchables { get; }
    }

    public interface ISwitchableGroup
    {
        Guid Guid { get; }
        int KeyCount { get; }
        Type SwitchableDataType { get; }
    }

    public static class SwitchableGroup
    {
        public static ISwitchableGroup<T> MakeSwitchableGroup<T>(Guid guid, int keyCount, IEnumerable<ISwitchable<T>> switchables)
        {
            return new SwitchableGroup<T>(guid, keyCount, switchables);
        }

        public static ISwitchableGroup<T> ToSwitchableGroup<T>(this IRando rando, Guid guid, int keyCount, int switchableCount)
        {
            return new SwitchableGroup<T>(guid, keyCount, rando.MakeSwitchables<T>(keyCount).Take(switchableCount));
        }

        public static IEnumerable<ISwitchableGroup<T>> Mutate<T>(this ISwitchableGroup<T> switchableGroup, IRando rando,
            double mutationRate, IEnumerable<Guid> newGuids)
        {
            var mutationSource = rando.MakeSwitchables<T>(switchableGroup.KeyCount).ToMoveNext();

            foreach (var newGuid in newGuids)
            {
                var switchables = switchableGroup.Switchables.Mutate(rando.ToBoolEnumerator(mutationRate),
                    t => mutationSource.Next());

                yield return new SwitchableGroup<T>
                    (
                        guid: newGuid,
                        keyCount: switchableGroup.KeyCount,
                        switchables: switchables
                    );
            }
        }
    }

    public class SwitchableGroup<T> : ISwitchableGroup<T>
    {
        public SwitchableGroup(Guid guid, int keyCount, IEnumerable<ISwitchable<T>> switchables)
        {
            _switchableDataType = typeof(T);
            _keyCount = keyCount;
            _guid = guid;
            _switchables = switchables.ToList();
        }

        private readonly List<ISwitchable<T>> _switchables;
        public IReadOnlyList<ISwitchable<T>> Switchables
        {
            get { return _switchables; }
        }

        private readonly Guid _guid;
        public Guid Guid
        {
            get { return _guid; }
        }

        private readonly int _keyCount;
        public int KeyCount
        {
            get { return _keyCount; }
        }

        private readonly Type _switchableDataType;
        public Type SwitchableDataType
        {
            get { return _switchableDataType; }
        }
    }
}
