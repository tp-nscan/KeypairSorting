using System.Linq;
using Genomic.Trackers;
using SorterEvo.Genomes;
using SorterEvo.Workflows;

namespace SorterEvo.Trackers
{
    public interface IScpWorkflowTracker: ITracker<IScpWorkflow>
    {
        IGenomePoolStats<ISorterGenome> SorterPoolStats { get; }
        string PoolReport { get; }
    }

    public static class ScpWorkflowTracker
    {
        public static IScpWorkflowTracker Make()
        {
            return new ScpWorkflowTrackerImpl(
                    sorterPoolStats: GenomePoolStats.Make<ISorterGenome>()
                );
        }
    }

    class ScpWorkflowTrackerImpl : IScpWorkflowTracker
    {
        public ScpWorkflowTrackerImpl(IGenomePoolStats<ISorterGenome> sorterPoolStats)
        {
            _sorterPoolStats = sorterPoolStats;
        }

        private readonly IGenomePoolStats<ISorterGenome> _sorterPoolStats;
        public IGenomePoolStats<ISorterGenome> SorterPoolStats
        {
            get { return _sorterPoolStats; }
        }

        private string _poolReport;
        public string PoolReport
        {
            get { return _poolReport; }
        }

        public void TrackItem(IScpWorkflow scpWorkflow)
        {
            switch (scpWorkflow.CompWorkflowState)
            {
                case CompWorkflowState.ReproGenomes:
                    break;
                case CompWorkflowState.RunCompetition:
                    break;
                case CompWorkflowState.EvaluateResults:
                    break;
                case CompWorkflowState.UpdateGenomes:


                    _poolReport = string.Format(
                        "{0}\t{1}\t{2}", 
                        scpWorkflow.ScpParams.Name, 
                        scpWorkflow.Generation,
                        scpWorkflow.CompPool.SorterEvals
                                              .Select(t=>t.SwitchUseCount)
                                              .OrderBy(c => c)
                                              .First()
                        );
                    

                    //_poolReport = scpWorkflow.CompPool.SorterOnSwitchableGroups.Select(
                    //    t=>t.SwitchesUsed)
                    //    .OrderBy(c=>c)
                    //    .Take(100)
                    //    .Aggregate(string.Empty,(r,n)=> n + "\t" + r);

                    //SorterPoolStats.AddGenomeEvals(
                    //        sorterCompParaPoolWorkflow.SorterLayerEval
                    //            .GenomeEvals
                    //            .OrderBy(t => t.Score),
                    //        SorterCompParaPoolWorkflowTracker.SorterRefScore
                    //    );
                    //SwitchablePoolStats.AddGenomeEvals(
                    //        sorterCompParaPoolWorkflow.SwitchableGroupLayerEval
                    //            .GenomeEvals
                    //            .OrderBy(t => t.Score),
                    //        SorterCompParaPoolWorkflowTracker.SwitchableGroupRefScore
                    //    );


                    //var compParaPool = sorterCompParaPoolWorkflow.CompParaPool;

                    //var compParaPoolScores =
                    //    compParaPool.SorterOnSwitchableGroups
                    //        .Where(
                    //                g => SorterPoolStats.GenomeStatses.Any(ps => ps.Guid == g.Sorter.Guid)
                    //                    &&
                    //                    SwitchablePoolStats.GenomeStatses.Any(ps => ps.Guid == g.SwitchableGroup.Guid)
                    //              )
                    //        .Select(
                    //            s =>
                    //                new Tuple<Guid, Guid, Tuple<bool, int>>(s.Sorter.Guid, s.SwitchableGroup.Guid,
                    //                    new Tuple<bool, int>(s.Success, s.SwitchesUsed)));

                    //var compParaPoolResults = compParaPoolScores.Select(
                    //s => new Tuple<IGenomeStats<ISorterGenome>, IGenomeStats<ISwitchableGroupGenome>, Tuple<bool, int>>
                    //(
                    //    SorterPoolStats.GenomeStatses.Single(g => g.Guid == s.Item1),
                    //    SwitchablePoolStats.GenomeStatses.Single(g => g.Guid == s.Item2),
                    //    s.Item3
                    //));

                    //var genomeTable = GenomeTable.Make(compParaPoolResults);

                    //_poolReport = genomeTable.Print
                    //(
                    //    g => g.FirstGeneration.ToString() + "_" + g.ReferenceResult.Cast<ISorterOnSwitchableGroup>().SwitchesUsed,
                    //    h => h.FirstGeneration.ToString() + "_",
                    //    i => i.Item2.ToString()
                    //);

                    break;
                default:
                    break;
            }
        }
    }
}
