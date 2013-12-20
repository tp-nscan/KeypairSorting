using Genomic.Trackers;
using SorterEvo.Genomes;
using SorterEvo.Workflows;

namespace SorterEvo.Trackers
{
    public interface ISorterCompPoolWorkflowTracker: ITracker<ISorterCompPoolWorkflow>
    {
        IGenomePoolStats<ISorterGenome> SorterPoolStats { get; }
        string PoolReport { get; }
    }

    public static class SorterCompPoolWorkflowTracker
    {
        public static ISorterCompPoolWorkflowTracker Make()
        {
            return new SorterCompPoolWorkflowTrackerImpl(
                    sorterPoolStats: GenomePoolStats.Make<ISorterGenome>()
                );
        }
    }

    class SorterCompPoolWorkflowTrackerImpl : ISorterCompPoolWorkflowTracker
    {
        public SorterCompPoolWorkflowTrackerImpl(IGenomePoolStats<ISorterGenome> sorterPoolStats)
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

        public void TrackItem(ISorterCompPoolWorkflow sorterCompPoolWorkflow)
        {
            switch (sorterCompPoolWorkflow.CompWorkflowState)
            {
                case CompWorkflowState.ReproGenomes:
                    break;
                case CompWorkflowState.RunCompetition:
                    break;
                case CompWorkflowState.EvaluateResults:
                    break;
                case CompWorkflowState.UpdateGenomes:


                    _poolReport += "update_";

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
