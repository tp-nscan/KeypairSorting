namespace SorterEvo.Workflows
{
    public class SorterCompPoolParams
    {
        public SorterCompPoolParams(
                int sorterLayerStartingGenomeCount, 
                int sorterLayerExpandedGenomeCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate
            )
        {
            _sorterMutationRate = sorterMutationRate;
            _sorterInsertionRate = sorterInsertionRate;
            _sorterDeletionRate = sorterDeletionRate;
            _sorterLayerExpandedGenomeCount = sorterLayerExpandedGenomeCount;
            _sorterLayerStartingGenomeCount = sorterLayerStartingGenomeCount;
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

    }
}
