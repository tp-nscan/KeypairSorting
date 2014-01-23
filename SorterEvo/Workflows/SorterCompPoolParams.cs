using System.Collections.Generic;

namespace SorterEvo.Workflows
{
    public interface IScpParams
    {
        string Name { get; }
        int PopulationCount { get; }
        int ChildCount { get; }
        int CubCount { get; }
        int LegacyCount { get; }
        double SorterMutationRate { get; }
        double SorterInsertionRate { get; }
        double SorterDeletionRate { get; }
        double SorterRecombinationRate { get; }
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
                        populationCount: sorterLayerStartingGenomeCount,
                        childCount: sorterLayerExpandedGenomeCount,
                        legacyCount:0,
                        cubCount:0,
                        sorterMutationRate: baseMutationRate * i,
                        sorterInsertionRate: baseMutationRate * i,
                        sorterDeletionRate: baseMutationRate * i,
                        sorterRecombinationRate: 0,
                        name: baseName + "_p" + i,
                        seed: seed,
                        totalGenerations: totalGenerations
                    );
            }
        }

        public static IScpParams Make
            (
                int populationCount,
                int childCount,
                int cubCount,
                int legacyCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate,
                double sorterRecombinationRate,
                string name,
                int seed,
                int totalGenerations
            )
        {
            return new ScpParamsImpl
                (
                    populationCount: populationCount,
                    childCount: childCount,
                    cubCount: cubCount,
                    legacyCount: legacyCount,
                    sorterMutationRate: sorterMutationRate,
                    sorterInsertionRate: sorterInsertionRate,
                    sorterDeletionRate: sorterDeletionRate,
                    sorterRecombinationRate: sorterRecombinationRate,
                    name: name,
                    seed: seed,
                    totalGenerations: totalGenerations
                );
        }

        public static IScpParams MakeGenerational
        (
            int populationCount,
            int childCount,
            int cubCount,
            int legacyCount,
            double sorterMutationRate,
            double sorterInsertionRate,
            double sorterDeletionRate,
            double sorterRecombinationRate,
            string name,
            int seed,
            int totalGenerations
        )
        {
            return new ScpParamsImpl
                (
                    populationCount: populationCount,
                    childCount: childCount,
                    cubCount: cubCount,
                    legacyCount: legacyCount,
                    sorterMutationRate: sorterMutationRate,
                    sorterInsertionRate: sorterInsertionRate,
                    sorterDeletionRate: sorterDeletionRate,
                    sorterRecombinationRate: sorterRecombinationRate,
                    name: name,
                    seed: seed,
                    totalGenerations: totalGenerations
                );
        }
    }

    public class ScpParamsImpl : IScpParams
    {
        public ScpParamsImpl (
                int populationCount, 
                int childCount,
                int cubCount, 
                int legacyCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate, 
                string name, 
                int seed,
                int totalGenerations, double sorterRecombinationRate)
        {
            _sorterMutationRate = sorterMutationRate;
            _sorterInsertionRate = sorterInsertionRate;
            _sorterDeletionRate = sorterDeletionRate;
            _name = name;
            _seed = seed;
            _childCount = childCount;
            _populationCount = populationCount;
            _totalGenerations = totalGenerations;
            _sorterRecombinationRate = sorterRecombinationRate;
            _cubCount = cubCount;
            _legacyCount = legacyCount;
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        private readonly int _populationCount;
        public int PopulationCount
        {
            get { return _populationCount; }
        }

        private readonly int _childCount;
        public int ChildCount
        {
            get { return _childCount; }
        }

        private readonly int _cubCount;
        public int CubCount
        {
            get { return _cubCount; }
        }

        private readonly int _legacyCount;
        public int LegacyCount
        {
            get { return _legacyCount; }
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

        private readonly int _seed;
        public int Seed
        {
            get { return _seed; }
        }

        private readonly int _totalGenerations;
        public int TotalGenerations
        {
            get { return _totalGenerations; }
        }
    }
}
