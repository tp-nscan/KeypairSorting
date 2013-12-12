using System.Collections.Generic;
using Genomic.Layers;
using MathUtils.Rand;
using SorterEvo.Genomes;
using SorterEvo.Workflows;

namespace SorterEvo.TestData
{
    public static class TestSorterEvo
    {
        public static int Seed = 1234;
        public static int KeyCount = 16;
        public static int KeyPairCount = 500;
        public static SwitchableGroupGenomeType SwitchableGroupGenomeType = SwitchableGroupGenomeType.UInt;

        public static int SorterGenomeCount = 10;
        public static int SorterLayerExpandedGenomeCount = 20;
        public static double SorterMutationRate = 0.3;
        public static double SorterInsertionRate = 0.3;
        public static double SorterDeletionRate = 0.3;

        public static int SwitchableGroupGenomeCount = 10;
        public static int SwitchableGroupExpandedGenomeCount = 20;
        public static double SwitchableMutationRate = 0.3;
        public static double SwitchableInsertionRate = 0.3;
        public static double SwitchableDeletionRate = 0.3;

        public static int SwitchableGroupSize = 250;

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

        public static SorterCompPoolParams SorterPoolCompParams()
        {
            return new SorterCompPoolParams(
                     sorterLayerStartingGenomeCount: SorterGenomeCount,
                     sorterLayerExpandedGenomeCount: SorterLayerExpandedGenomeCount,
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

    }
}
