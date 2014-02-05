using System.Collections.Generic;
using System.Linq;
using Sorting.KeyPairs;

namespace Sorting.Sorters.StageGenerators
{
    public static class DependentKeyPairEx
    {
        public static IEnumerable<DependentKeyPair> ToDependentKeyPairs(this IReadOnlyList<IKeyPair> keyPairs)
        {
            var dependentKeyPairList = keyPairs.Select(kp => new DependentKeyPair(kp)).ToList();

            for (var i = 0; i < dependentKeyPairList.Count; i++)
            {
                var curDp = dependentKeyPairList[i];
                curDp.SetDependentKeyPairs(
                    dependentKeyPairList.Where(dp => dp.Overlaps(curDp))
                   );
                yield return dependentKeyPairList[i];
            }
        }

        public static IEnumerable<IKeyPair> Reduce(this IReadOnlyList<DependentKeyPair> dependentKeyPairs)
        {
            for (var i = 0; i < dependentKeyPairs.Count; i++)
            {
                if (dependentKeyPairs[i].IsUsed)
                {
                    yield return KeyPairRepository.AtIndex(dependentKeyPairs[i].Index);
                }
            }
        }
    }

    public class DependentKeyPair : IKeyPair
    {
        public DependentKeyPair(IKeyPair keyPair)
        {
            _keyPair = keyPair;
            IsUsed = IsDisabled = false;
        }

        private readonly IKeyPair _keyPair;

        public int LowKey
        {
            get { return _keyPair.LowKey; }
        }

        public int HiKey
        {
            get { return _keyPair.HiKey; }
        }

        public int Index
        {
            get { return _keyPair.Index; }
        }

        public bool IsUsed { get; set; }

        public bool IsDisabled { get; set; }

        public void SetDependentKeyPairs(IEnumerable<DependentKeyPair> dependentKeyPairs)
        {
            _dependentKeyPairs = dependentKeyPairs.ToList();
        }

        private  List<DependentKeyPair> _dependentKeyPairs;
        public void DisableDependentKeyPairs()
        {
            foreach (var dependentKeyPair in _dependentKeyPairs)
            {
                dependentKeyPair.IsDisabled = true;
            }
        }
    }
}