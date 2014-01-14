using System.Collections.Generic;

namespace SorterEvo.Workflows
{
    public interface ISorterCompPoolParams
    {
        string Name { get; }
        int SorterLayerStartingGenomeCount { get; }
        int SorterLayerExpandedGenomeCount { get; }
        double SorterMutationRate { get; }
        double SorterInsertionRate { get; }
        double SorterDeletionRate { get; }
    }

    public static class SorterCompPoolParams
    {
        public static IEnumerable<ISorterCompPoolParams> MakeStandards
            (
                string baseName,
                int count
            )
        {
            const int sorterLayerStartingGenomeCount = 100;
            const int sorterLayerExpandedGenomeCount = 200;
            const double baseMutationRate = 0.01;

            for (var i = 0; i < count; i++)
            {
                yield return Make
                    (
                        sorterLayerStartingGenomeCount: sorterLayerStartingGenomeCount,
                        sorterLayerExpandedGenomeCount: sorterLayerExpandedGenomeCount,
                        sorterMutationRate: baseMutationRate * i,
                        sorterInsertionRate: baseMutationRate * i,
                        sorterDeletionRate: baseMutationRate * i,
                        name: baseName + "_p" + i
                    );
            }
        }


        public static ISorterCompPoolParams Make
            (
                int sorterLayerStartingGenomeCount,
                int sorterLayerExpandedGenomeCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate,
                string name
            )
        {
            return new SorterCompPoolParamsImpl
                (
                    sorterLayerStartingGenomeCount: sorterLayerStartingGenomeCount,
                    sorterLayerExpandedGenomeCount: sorterLayerExpandedGenomeCount,
                    sorterMutationRate: sorterMutationRate,
                    sorterInsertionRate: sorterInsertionRate,
                    sorterDeletionRate: sorterDeletionRate,
                    name: name
                );
        }

        public static ISorterCompPoolParams MakeGenerational
        (
            int sorterLayerStartingGenomeCount,
            int sorterLayerExpandedGenomeCount,
            double sorterMutationRate,
            double sorterInsertionRate,
            double sorterDeletionRate,
            string name
        )
        {
            return new SorterCompPoolParamsImpl
                (
                    sorterLayerStartingGenomeCount: sorterLayerStartingGenomeCount,
                    sorterLayerExpandedGenomeCount: sorterLayerExpandedGenomeCount,
                    sorterMutationRate: sorterMutationRate,
                    sorterInsertionRate: sorterInsertionRate,
                    sorterDeletionRate: sorterDeletionRate,
                    name: name
                );
        }
    }

    public class SorterCompPoolParamsImpl : ISorterCompPoolParams
    {
        public SorterCompPoolParamsImpl (
                int sorterLayerStartingGenomeCount, 
                int sorterLayerExpandedGenomeCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate, 
                string name
            )
        {
            _sorterMutationRate = sorterMutationRate;
            _sorterInsertionRate = sorterInsertionRate;
            _sorterDeletionRate = sorterDeletionRate;
            _name = name;
            _sorterLayerExpandedGenomeCount = sorterLayerExpandedGenomeCount;
            _sorterLayerStartingGenomeCount = sorterLayerStartingGenomeCount;
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
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
