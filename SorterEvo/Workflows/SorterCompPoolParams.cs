using System.Collections.Generic;

namespace SorterEvo.Workflows
{
    public interface IScpParams
    {
        string Name { get; }
        int SorterLayerStartingGenomeCount { get; }
        int SorterLayerExpandedGenomeCount { get; }
        double SorterMutationRate { get; }
        double SorterInsertionRate { get; }
        double SorterDeletionRate { get; }
        int Seed { get; }
        int TotalGenerations { get; }
    }

    public static class ScpParams
    {
        public static IEnumerable<IScpParams> MakeStandards
            (
                string baseName,
                int count,
                int seed,
                int totalGenerations
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
                        name: baseName + "_p" + i,
                        seed: seed,
                        totalGenerations: totalGenerations
                    );
            }
        }

        public static IScpParams Make
            (
                int sorterLayerStartingGenomeCount,
                int sorterLayerExpandedGenomeCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate,
                string name,
                int seed,
                int totalGenerations
            )
        {
            return new ScpParamsImpl
                (
                    sorterLayerStartingGenomeCount: sorterLayerStartingGenomeCount,
                    sorterLayerExpandedGenomeCount: sorterLayerExpandedGenomeCount,
                    sorterMutationRate: sorterMutationRate,
                    sorterInsertionRate: sorterInsertionRate,
                    sorterDeletionRate: sorterDeletionRate,
                    name: name,
                    seed: seed,
                    totalGenerations: totalGenerations
                );
        }

        public static IScpParams MakeGenerational
        (
            int sorterLayerStartingGenomeCount,
            int sorterLayerExpandedGenomeCount,
            double sorterMutationRate,
            double sorterInsertionRate,
            double sorterDeletionRate,
            string name,
            int seed,
            int totalGenerations
        )
        {
            return new ScpParamsImpl
                (
                    sorterLayerStartingGenomeCount: sorterLayerStartingGenomeCount,
                    sorterLayerExpandedGenomeCount: sorterLayerExpandedGenomeCount,
                    sorterMutationRate: sorterMutationRate,
                    sorterInsertionRate: sorterInsertionRate,
                    sorterDeletionRate: sorterDeletionRate,
                    name: name,
                    seed: seed,
                    totalGenerations: totalGenerations
                );
        }
    }

    public class ScpParamsImpl : IScpParams
    {
        public ScpParamsImpl (
                int sorterLayerStartingGenomeCount, 
                int sorterLayerExpandedGenomeCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate, 
                string name, 
                int seed,
                int totalGenerations
            )
        {
            _sorterMutationRate = sorterMutationRate;
            _sorterInsertionRate = sorterInsertionRate;
            _sorterDeletionRate = sorterDeletionRate;
            _name = name;
            _seed = seed;
            _sorterLayerExpandedGenomeCount = sorterLayerExpandedGenomeCount;
            _sorterLayerStartingGenomeCount = sorterLayerStartingGenomeCount;
            _totalGenerations = totalGenerations;
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

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        private int _totalGenerations;
        public int TotalGenerations
        {
            get { return _totalGenerations; }
        }
    }
}
