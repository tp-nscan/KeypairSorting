using System.Linq;
using SorterEvo.Genomes;
using Sorting.CompetePools;

namespace SorterEvo.TestData
{
    public static class Evals
    {
        public static ISorterEval SorterEval
        {
            get
            {
                return CompPool.MakeEmpty(Genomes.KeyCount).AddSorterEvalsParallel(

                    (new[] { Genomes.SorterGenome.ToSorter() })
                        ).SorterEvals.First();
            }
        }
    }
}
