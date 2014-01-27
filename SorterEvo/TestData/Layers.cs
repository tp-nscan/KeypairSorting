using System.Collections.Generic;
using System.Linq;
using Genomic.Layers;
using MathUtils;
using MathUtils.Rand;
using SorterEvo.Genomes;
using SorterEvo.Workflows;
using Sorting.CompetePools;
using Sorting.Switchables;

namespace SorterEvo.TestData
{
    public static class Layers
    {
        public static int Seed = 447744;
        public static int KeyCount = 12;
        public static int KeyPairCount = 1000;
        public static SwitchableGroupGenomeType SwitchableGroupGenomeType = SwitchableGroupGenomeType.UInt;

        public static int PopulationSize = 100;
        public static int ChildCount = 100;
        public static int LegacyCount = 0;
        public static int CubCount = 0;
        public static double SorterMutationRate = 0.025;
        public static double SorterInsertionRate = 0.005;
        public static double SorterDeletionRate = 0.005;

        public static int SwitchableGroupGenomeCount = 3;
        public static int SwitchableGroupExpandedGenomeCount = 6;
        public static double SwitchableMutationRate = 0.02;
        public static double SwitchableInsertionRate = 0.02;
        public static double SwitchableDeletionRate = 0.02;
        public static double SorterRecombinationRate = 0;
        public static int SwitchableGroupSize = 200;
        public static int TotalGenerations = 10;

        public static IEnumerable<int> Seeds
        {
            get
            {
                return Rando.Fast(9876).ToIntEnumerator();
            }
        }

        public static ILayer<ISorterGenome> SorterLayer()
        {
            return SorterEvo.Layers.SorterLayer.Create(
                    seed: Seed,
                    genomeCount: PopulationSize,
                    keyCount: KeyCount,
                    keyPairCount: KeyPairCount
                );
        }

        public static ILayer<ISorterGenome> SorterExpandedLayer()
        {
            return SorterEvo.Layers.SorterLayer.Create(
                    seed: Seed,
                    genomeCount: ChildCount,
                    keyCount: KeyCount,
                    keyPairCount: KeyPairCount
                );
        }

        public static ILayer<ISwitchableGroupGenome> SwitchableGroupLayer()
        {
            return SorterEvo.Layers.SwitchableGroupLayer.Create(
                    seed: Seed,
                    switchableGroupGenomeType: SwitchableGroupGenomeType,
                    genomeCount: SwitchableGroupGenomeCount,
                    keyCount: KeyCount,
                    groupSize: SwitchableGroupSize
                );
        }

        public static ILayer<ISwitchableGroupGenome> SwitchableGroupExpandedLayer()
        {
            return SorterEvo.Layers.SwitchableGroupLayer.Create(
                    seed: Seed,
                    switchableGroupGenomeType: SwitchableGroupGenomeType,
                    genomeCount: SwitchableGroupExpandedGenomeCount,
                    keyCount: KeyCount,
                    groupSize: SwitchableGroupSize
                );
        }

        public static SorterCompParaPoolParams SorterCompParaPoolParams()
        {
            return new SorterCompParaPoolParams(
                     populationSize: PopulationSize,
                     childCount: ChildCount,
                     sorterMutationRate: SorterMutationRate,
                     sorterInsertionRate: SorterInsertionRate,
                     sorterDeletionRate: SorterDeletionRate,
                     switchableLayerStartingGenomeCount: SwitchableGroupGenomeCount,
                     switchableLayerExpandedGenomeCount: SwitchableGroupExpandedGenomeCount,
                     switchableGroupMutationRate: SwitchableMutationRate,
                     switchableGroupInsertionRate: SwitchableInsertionRate,
                     switchableGroupDeletionRate: SwitchableDeletionRate,
                     sorterRecombinationRate: SorterRecombinationRate
                );
        }

        public static IScpParams ScpParams()
        {
            return Workflows.ScpParams.Make(
                     populationCount: PopulationSize,
                     childCount: ChildCount,
                     legacyCount: LegacyCount,
                     cubCount: CubCount,
                     sorterMutationRate: SorterMutationRate,
                     sorterInsertionRate: SorterInsertionRate,
                     sorterDeletionRate: SorterDeletionRate,
                     sorterRecombinationRate: SorterRecombinationRate,
                     name: "Test Params",
                     seed: Seed,
                     totalGenerations: TotalGenerations  
                );
        }

        static ICompParaPool _compParaPool;
        public static ICompParaPool CompParaPool
        {
            get
            {
                return _compParaPool ?? 
                    (
                        _compParaPool = SorterLayer()
                                        .Genomes.Select(g=>g.ToSorter())
                                        .ToCompParaPoolParallel(SwitchableGroupLayer()
                                        .Genomes.Select(g=>g.ToSwitchableGroup()
                                        .Cast<ISwitchableGroup<uint>>()))
                    );
            }
        }

    }
}
