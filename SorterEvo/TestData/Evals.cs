using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genomic.GenomePools;
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
                return
                    (new[] {Genomes.SorterGenome.ToSorter()})
                                .ToCompPoolParallel()
                                .SorterEvals.First();
            }
        }
    }
}
