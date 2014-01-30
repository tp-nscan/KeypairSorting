﻿namespace SorterEvo.Workflows
{
    public interface ISorterMutateParams
    {
        string Name { get; }
        int MutantCount { get; }
        double SorterMutationRate { get; }
        double SorterInsertionRate { get; }
        double SorterDeletionRate { get; }
        int Seed { get; }
    }

    public static class SorterMutateParams
    {
        public static ISorterMutateParams Make
        (
            int mutantCount,
            double sorterMutationRate,
            double sorterInsertionRate,
            double sorterDeletionRate,
            string name,
            int seed
        )
        {
            return new SorterMutateParamsImpl
                (
                    mutantCount: mutantCount,
                    sorterMutationRate: sorterMutationRate,
                    sorterInsertionRate: sorterInsertionRate,
                    sorterDeletionRate: sorterDeletionRate,
                    name: name,
                    seed: seed
                );
        }
    }

    public class SorterMutateParamsImpl : ISorterMutateParams
    {
        public SorterMutateParamsImpl(
                int mutantCount,
                double sorterMutationRate,
                double sorterInsertionRate,
                double sorterDeletionRate,
                string name,
                int seed
            )
        {
            _sorterMutationRate = sorterMutationRate;
            _sorterInsertionRate = sorterInsertionRate;
            _sorterDeletionRate = sorterDeletionRate;
            _name = name;
            _seed = seed;
            _mutantCount = mutantCount;
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        private readonly int _mutantCount;
        public int MutantCount
        {
            get { return _mutantCount; }
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

    }




}
