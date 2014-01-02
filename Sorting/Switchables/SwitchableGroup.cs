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
        int SwitchableCount { get; }
        Type SwitchableDataType { get; }
        IEnumerable<string> SwitchableStrings { get; }  
    }

    public static class SwitchableGroup
    {
        const int AllSwitchableGroupsForKeyCount = 1;

        public static Guid GuidOfAllSwitchableGroupsForKeyCount(int keyCount)
        {
            return new Guid(
                    a: AllSwitchableGroupsForKeyCount,
                    b: (short)keyCount, 
                    c: 3, 
                    d: 4, 
                    e: 5, 
                    f:6, 
                    g:7, 
                    h:8, 
                    i:9, 
                    j:10, 
                    k: 11
                );
        }

        public static ISwitchableGroup<T> ToSwitchableGroup<T>(this IEnumerable<ISwitchable<T>> switchables, Guid guid, int keyCount)
        {
            return new SwitchableGroupImpl<T>(guid, keyCount, switchables);
        }

        public static ISwitchableGroup<T> ToSwitchableGroup<T>(this IRando rando, Guid guid, int keyCount, int switchableCount)
        {
            return new SwitchableGroupImpl<T>(guid, keyCount, rando.MakeSwitchables<T>(keyCount).Take(switchableCount));
        }

        public static IEnumerable<ISwitchableGroup<T>> Mutate<T>(this ISwitchableGroup<T> switchableGroup, IRando rando,
            double mutationRate, IEnumerable<Guid> newGuids)
        {
            var mutationSource = rando.MakeSwitchables<T>(switchableGroup.KeyCount).ToMoveNext();

            foreach (var newGuid in newGuids)
            {
                var switchables = switchableGroup.Switchables.Mutate(rando.ToBoolEnumerator(mutationRate),
                    t => mutationSource.Next());

                yield return new SwitchableGroupImpl<T>
                    (
                        guid: newGuid,
                        keyCount: switchableGroup.KeyCount,
                        switchables: switchables
                    );
            }
        }
    }

    class SwitchableGroupImpl<T> : ISwitchableGroup<T>
    {
        public SwitchableGroupImpl(Guid guid, int keyCount, IEnumerable<ISwitchable<T>> switchables)
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

        public int SwitchableCount
        {
            get { return _switchables.Count; }
        }

        private readonly Type _switchableDataType;
        public Type SwitchableDataType
        {
            get { return _switchableDataType; }
        }

        public IEnumerable<string> SwitchableStrings
        {
            get { return Switchables.Select(s=>s.Item.ToString()); }
        }
    }
}
