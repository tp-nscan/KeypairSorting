using System;
using System.Linq;
using Genomic.Table;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SorterEvo.Genomes;

namespace SorterEvo.Test.Table
{
    [TestClass]
    public class GenomeTableFixture
    {
        [TestMethod]
        public void TestCompParaPoolTable()
        {
            var compParaPool = TestData.SorterEvoTestData.CompParaPool;
            
            var compParaPoolScores =
                compParaPool.SorterOnSwitchableGroups.Select(
                    s =>
                        new Tuple<Guid, Guid, Tuple<bool, int>>(s.Sorter.Guid, s.SwitchableGroupGuid,
                            new Tuple<bool, int>(s.Success, s.SwitchesUsed)));

            var compParaPoolResults = compParaPoolScores.Select (
                s => new Tuple<ISorterGenome, ISwitchableGroupGenome, Tuple<bool, int>>
                (
                    TestData.SorterEvoTestData.SorterLayer().GetGenome(s.Item1),
                    TestData.SorterEvoTestData.SwitchableGroupLayer().GetGenome(s.Item2),
                    s.Item3
                ));

            var genomeTable = GenomeTable.Make(compParaPoolResults);

            System.Diagnostics.Debug.WriteLine(genomeTable.Print
                (
                    g=>g.Guid.ToString(), 
                    h=>h.Guid.ToString(), 
                    i=>i.Item2.ToString())
                );
        }
    }
}
