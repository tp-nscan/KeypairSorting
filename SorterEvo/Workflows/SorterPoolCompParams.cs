namespace SorterEvo.Workflows
{
    public class SorterPoolCompParams
    {
        public SorterPoolCompParams(
            double sorterMutationRate,
            double sorterInsertionRate,
            double sorterDeletionRate,
            double switchableGroupMutationRate,
            double switchableGroupInsertionRate,
            double switchableGroupDeletionRate,
            int sorterMuliplicationRate, 
            int switchableGroupMuliplicationRate)
        {
            _sorterMutationRate = sorterMutationRate;
            _sorterInsertionRate = sorterInsertionRate;
            _sorterDeletionRate = sorterDeletionRate;
            _switchableGroupMutationRate = switchableGroupMutationRate;
            _switchableGroupInsertionRate = switchableGroupInsertionRate;
            _switchableGroupDeletionRate = switchableGroupDeletionRate;
            _sorterMuliplicationRate = sorterMuliplicationRate;
            _switchableGroupMuliplicationRate = switchableGroupMuliplicationRate;
        }

        private readonly int _sorterMuliplicationRate;
        private readonly int _switchableGroupMuliplicationRate;


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

        public int SorterMuliplicationRate
        {
            get { return _sorterMuliplicationRate; }
        }

        public int SwitchableGroupMuliplicationRate
        {
            get { return _switchableGroupMuliplicationRate; }
        }
    }
}