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
    public static class SorterEvoTestData
    {
        public static int Seed = 1234;
        public static int KeyCount = 16;
        public static int KeyPairCount = 800;
        public static SwitchableGroupGenomeType SwitchableGroupGenomeType = SwitchableGroupGenomeType.UInt;

        public static int SorterGenomeCount = 10;
        public static int SorterExpandedGenomeCount = 20;
        public static double SorterMutationRate = 0.02;
        public static double SorterInsertionRate = 0.02;
        public static double SorterDeletionRate = 0.02;

        public static int SwitchableGroupGenomeCount = 3;
        public static int SwitchableGroupExpandedGenomeCount = 6;
        public static double SwitchableMutationRate = 0.02;
        public static double SwitchableInsertionRate = 0.02;
        public static double SwitchableDeletionRate = 0.02;

        public static int SwitchableGroupSize = 200;

        public static IEnumerable<int> Seeds
        {
            get
            {
                return Rando.Fast(9876).ToIntEnumerator();
            }
        }

        public static ILayer<ISorterGenome> SorterLayer()
        {
            return Layers.SorterLayer.Create(
                    seed: Seed,
                    genomeCount: SorterGenomeCount,
                    keyCount: KeyCount,
                    keyPairCount: KeyPairCount
                );
        }

        public static ILayer<ISorterGenome> SorterExpandedLayer()
        {
            return Layers.SorterLayer.Create(
                    seed: Seed,
                    genomeCount: SorterExpandedGenomeCount,
                    keyCount: KeyCount,
                    keyPairCount: KeyPairCount
                );
        }

        public static ILayer<ISwitchableGroupGenome> SwitchableGroupLayer()
        {
            return Layers.SwitchableGroupLayer.Create(
                    seed: Seed,
                    switchableGroupGenomeType: SwitchableGroupGenomeType,
                    genomeCount: SwitchableGroupGenomeCount,
                    keyCount: KeyCount,
                    groupSize: SwitchableGroupSize
                );
        }

        public static ILayer<ISwitchableGroupGenome> SwitchableGroupExpandedLayer()
        {
            return Layers.SwitchableGroupLayer.Create(
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
                     sorterLayerStartingGenomeCount: SorterGenomeCount,
                     sorterLayerExpandedGenomeCount: SorterExpandedGenomeCount,
                     sorterMutationRate: SorterMutationRate,
                     sorterInsertionRate: SorterMutationRate,
                     sorterDeletionRate: SorterDeletionRate,
                     switchableLayerStartingGenomeCount: SwitchableGroupGenomeCount,
                     switchableLayerExpandedGenomeCount: SwitchableGroupExpandedGenomeCount,
                     switchableGroupMutationRate: SwitchableMutationRate,
                     switchableGroupInsertionRate: SwitchableInsertionRate,
                     switchableGroupDeletionRate: SwitchableDeletionRate
                );
        }

        public static SorterCompPoolParams SorterCompPoolParams()
        {
            return new SorterCompPoolParams(
                     sorterLayerStartingGenomeCount: SorterGenomeCount,
                     sorterLayerExpandedGenomeCount: SorterExpandedGenomeCount,
                     sorterMutationRate: SorterMutationRate,
                     sorterInsertionRate: SorterMutationRate,
                     sorterDeletionRate: SorterDeletionRate
                );
        }

        static ICompParaPool _compParaPool;
        public static ICompParaPool CompParaPool
        {
            get
            {
                return _compParaPool ?? 
                    (
                        _compParaPool = SorterLayer().Genomes.Select(g=>g.ToSorter())
                                                 .ToCompParaPoolParallel(SwitchableGroupLayer()
                                                 .Genomes.Select(g=>g.ToSwitchableGroup()
                                                 .Cast<ISwitchableGroup<uint>>()))
                    );
            }
        }

    }
}
