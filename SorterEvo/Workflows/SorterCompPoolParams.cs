namespace SorterEvo.Workflows
{
    public class SorterCompPoolParams
    {
        public SorterCompPoolParams(
                int sorterLayerStartingGenomeCount, 
                int sorterLayerExpandedGenomeCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate,
                int switchableLayerStartingGenomeCount,
                int switchableLayerExpandedGenomeCount,
                double switchableGroupMutationRate,
                double switchableGroupInsertionRate,
                double switchableGroupDeletionRate
            )
        {
            _sorterMutationRate = sorterMutationRate;
            _sorterInsertionRate = sorterInsertionRate;
            _sorterDeletionRate = sorterDeletionRate;
            _switchableLayerExpandedGenomeCount = switchableLayerExpandedGenomeCount;
            _switchableGroupInsertionRate = switchableGroupInsertionRate;
            _switchableGroupDeletionRate = switchableGroupDeletionRate;
            _sorterLayerExpandedGenomeCount = sorterLayerExpandedGenomeCount;
            _switchableLayerStartingGenomeCount = switchableLayerStartingGenomeCount;
            _sorterLayerStartingGenomeCount = sorterLayerStartingGenomeCount;
            _switchableGroupMutationRate = switchableGroupMutationRate;
        }

        private readonly int _sorterLayerStartingGenomeCount;
        public int SorterLayerStartingGenomeCount
        {
            get { return _sorterLayerStartingGenomeCount; }
        }

        private readonly int _sorterLayerExpandedGenomeCount;
        public int SorterLayerExpandedGenomeCount
        {
            get { return _sorterLayerExpandedGenomeCount; }
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