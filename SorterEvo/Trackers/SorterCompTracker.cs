using System;
using System.Linq;
using Genomic.Genomes;
using Genomic.Table;
using Genomic.Trackers;
using MathUtils;
using SorterEvo.Genomes;
using SorterEvo.Workflows;
using Sorting.CompetePool;
using Sorting.Sorters;

namespace SorterEvo.Trackers
{
    public interface ISorterPoolCompWorkflowTracker : ITracker<ISorterCompWorkflow>
    {
        IGenomePoolStats<ISorterGenome> SorterPoolStats { get; }
        IGenomePoolStats<ISwitchableGroupGenome> SwitchablePoolStats { get; }
        string PoolReport { get; }
    }

    public static class SorterCompTracker
    {
        public static ISorterPoolCompWorkflowTracker Make()
        {
            return new SorterPoolCompWorkflowTrackerImpl(
                    sorterPoolStats:  GenomePoolStats.Make<ISorterGenome>(),
                    switchablePoolStats: GenomePoolStats.Make<ISwitchableGroupGenome>()
                );
        }

        public static ISorterPoolCompWorkflowTracker Trim(this ISorterPoolCompWorkflowTracker tracker, int count)
        {
            return new SorterPoolCompWorkflowTrackerImpl(
                    sorterPoolStats: GenomePoolStats.Make(tracker.SorterPoolStats.GenomeStatses.OrderBy(t=> ((ISorterOnSwitchableGroup)t.ReferenceResult).SwitchesUsed).Take(count)),
                    switchablePoolStats: GenomePoolStats.Make(tracker.SwitchablePoolStats.GenomeStatses.OrderBy(t => t.ReferenceResult).Take(count))
                );
        }

        public static ISorterOnSwitchableGroup SorterRefScore(ISorterGenome sorterGenome)
        {
            return sorterGenome.ToSorter().FullTest(sorterGenome.KeyCount);
        }

        public static object SwitchableGroupRefScore(ISwitchableGroupGenome switchableGroupGenome)
        {
            return 0;
        }
    }

    class SorterPoolCompWorkflowTrackerImpl : ISorterPoolCompWorkflowTracker
    {
        public void TrackItem(ISorterCompWorkflow sorterCompWorkflow)
        {
            //System.Diagnostics.Debug.WriteLine("tracking state: {0}", sorterCompWorkflow.SorterPoolCompState);
            switch (sorterCompWorkflow.SorterPoolCompState)
            {
                case SorterCompState.ReproGenomes:
                    break;
                case SorterCompState.RunCompetition:
                    break;
                case SorterCompState.EvaluateResults:
                    break;
                case SorterCompState.UpdateGenomes:
                    SorterPoolStats.AddGenomeEvals(
                            sorterCompWorkflow.SorterLayerEval
                                .GenomeEvals
                                .OrderBy(t => t.Score),
                                //.Take(sorterCompWorkflow.SorterPoolCompParams.SorterLayerStartingGenomeCount), 
                            SorterCompTracker.SorterRefScore
                        );
                    SwitchablePoolStats.AddGenomeEvals(
                            sorterCompWorkflow.SwitchableGroupLayerEval
                                .GenomeEvals
                                .OrderBy(t => t.Score),
                                //.Take(sorterCompWorkflow.SorterPoolCompParams.SwitchableLayerStartingGenomeCount), 
                            SorterCompTracker.SwitchableGroupRefScore
                        );


                    var compPool = sorterCompWorkflow.CompPool;
            
                    var compPoolScores =
                        compPool.SorterOnSwitchableGroups
                            .Where(
                                    g=> SorterPoolStats.GenomeStatses.Any(ps=>ps.Guid==g.Sorter.Guid)
                                        &&
                                        SwitchablePoolStats.GenomeStatses.Any(ps => ps.Guid == g.SwitchableGroup.Guid)
                                  )
                            .Select(
                                s =>
                                    new Tuple<Guid, Guid, Tuple<bool, int>>(s.Sorter.Guid, s.SwitchableGroup.Guid,
                                        new Tuple<bool, int>(s.Success, s.SwitchesUsed)));

                    //var compPoolResults = compPoolScores.Select (
                    //    s => new Tuple<IGenomeEval<ISorterGenome>, IGenomeEval<ISwitchableGroupGenome>, Tuple<bool, int>>
                    //    (
                    //        sorterCompWorkflow.SorterLayerEval.GetGenomeEval(s.Item1),
                    //        sorterCompWorkflow.SwitchableGroupLayerEval.GetGenomeEval(s.Item2),
                    //        s.Item3
                    //    ));

                        var compPoolResults = compPoolScores.Select(
                        s => new Tuple<IGenomeStats<ISorterGenome>, IGenomeStats<ISwitchableGroupGenome>, Tuple<bool, int>>
                        (
                            SorterPoolStats.GenomeStatses.Single(g=>g.Guid==s.Item1),
                            SwitchablePoolStats.GenomeStatses.Single(g=>g.Guid==s.Item2),
                            s.Item3
                        ));

                    var genomeTable = GenomeTable.Make(compPoolResults);

                    _poolReport = genomeTable.Print
                    (
                        g=>g.FirstGeneration.ToString() + "_" + g.ReferenceResult.Cast<ISorterOnSwitchableGroup>().SwitchesUsed,
                        h => h.FirstGeneration.ToString() + "_",
                        i=>i.Item2.ToString()
                    );

                    break;
                default:
                    break;
            }
        }

        private string _poolReport = string.Empty;
        public string PoolReport
        {
            get { return _poolReport; }
        }

        public SorterPoolCompWorkflowTrackerImpl(
            IGenomePoolStats<ISorterGenome> sorterPoolStats, 
            IGenomePoolStats<ISwitchableGroupGenome> switchablePoolStats
            )
        {
            _sorterPoolStats = sorterPoolStats;
            _switchablePoolStats = switchablePoolStats;
        }

        private readonly IGenomePoolStats<ISorterGenome> _sorterPoolStats;
        public IGenomePoolStats<ISorterGenome> SorterPoolStats
        {
            get { return _sorterPoolStats; }
        }

        private readonly IGenomePoolStats<ISwitchableGroupGenome> _switchablePoolStats;
        public IGenomePoolStats<ISwitchableGroupGenome> SwitchablePoolStats
        {
            get { return _switchablePoolStats; }
        }
    }
}
