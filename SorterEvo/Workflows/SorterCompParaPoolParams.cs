namespace SorterEvo.Workflows
{
    public class SorterCompParaPoolParams
    {
        public SorterCompParaPoolParams(
                int populationSize, 
                int childCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate,
                int switchableLayerStartingGenomeCount,
                int switchableLayerExpandedGenomeCount,
                double switchableGroupMutationRate,
                double switchableGroupInsertionRate,
                double switchableGroupDeletionRate, 
                double sorterRecombinationRate
            )
        {
            _sorterMutationRate = sorterMutationRate;
            _sorterInsertionRate = sorterInsertionRate;
            _sorterDeletionRate = sorterDeletionRate;
            _switchableLayerExpandedGenomeCount = switchableLayerExpandedGenomeCount;
            _switchableGroupInsertionRate = switchableGroupInsertionRate;
            _switchableGroupDeletionRate = switchableGroupDeletionRate;
            _sorterRecombinationRate = sorterRecombinationRate;
            _childCount = childCount;
            _switchableLayerStartingGenomeCount = switchableLayerStartingGenomeCount;
            _populationSize = populationSize;
            _switchableGroupMutationRate = switchableGroupMutationRate;
        }

        private readonly int _populationSize;
        public int PopulationSize
        {
            get { return _populationSize; }
        }

        private readonly int _childCount;
        public int ChildCount
        {
            get { return _childCount; }
        }

        private readonly double _sorterMutationRate;
        public double SorterMutationRate
        {
            get { return _sorterMutationRate; }
        }

        private readonly double _sorterInsertionRate;
        public double SorterInsertionRate
        {
            get { return _sorterInsertionRate; }
        }

        private readonly double _sorterDeletionRate;
        public double SorterDeletionRate
        {
            get { return _sorterDeletionRate; }
        }

        private readonly double _sorterRecombinationRate;
        public double SorterRecombinationRate
        {
            get { return _sorterRecombinationRate; }
        }

        private readonly int _switchableLayerStartingGenomeCount;
        public int SwitchableLayerStartingGenomeCount
        {
            get { return _switchableLayerStartingGenomeCount; }
        }

        private readonly int _switchableLayerExpandedGenomeCount;
        public int SwitchableLayerExpandedGenomeCount
        {
            get { return _switchableLayerExpandedGenomeCount; }
        }

        private readonly double _switchableGroupMutationRate;
        public double SwitchableGroupMutationRate
        {
            get { return _switchableGroupMutationRate; }
        }

        private readonly double _switchableGroupInsertionRate;
        public double SwitchableGroupInsertionRate
        {
            get { return _switchableGroupInsertionRate; }
        }

        private readonly double _switchableGroupDeletionRate;
        public double SwitchableGroupDeletionRate
        {
            get { return _switchableGroupDeletionRate; }
        }
    }
}