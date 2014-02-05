using System;
using MathUtils.Rand;
using Sorting.Switchables;

namespace Sorting.TestData
{
    public static class Switchables
    {
        static Switchables()
        {
            switchableSet = Rando.Fast(129).ToSwitchableGroup<uint>(Guid.NewGuid(), 13, 11);
        }

        static readonly ISwitchableGroup<uint> switchableSet;
        public static ISwitchableGroup<uint> SwitchableSet
        {
            get { return switchableSet; }
        }

    }
}
