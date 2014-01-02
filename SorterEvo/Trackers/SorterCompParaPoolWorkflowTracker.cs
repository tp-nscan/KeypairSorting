using System;
using System.Linq;
using Genomic.Table;
using Genomic.Trackers;
using MathUtils;
using SorterEvo.Genomes;
using SorterEvo.Workflows;
using Sorting.CompetePools;
using Sorting.Sorters;

namespace SorterEvo.Trackers
{
    public interface ISorterCompParaPoolWorkflowTracker : ITracker<ISorterCompParaPoolWorkflow>
    {
        IGenomePoolStats<ISorterGenome> SorterPoolStats { get; }
        IGenomePoolStats<ISwitchableGroupGenome> SwitchablePoolStats { get; }
        string PoolReport { get; }
    }

    public static class SorterCompParaPoolWorkflowTracker
    {
        public static ISorterCompParaPoolWorkflowTracker Make()
        {
            return new SorterCompParaPoolWorkflowTrackerImpl(
                    sorterPoolStats:  GenomePoolStats.Make<ISorterGenome>(),
                    switchablePoolStats: GenomePoolStats.Make<ISwitchableGroupGenome>()
                );
        }

        public static ISorterCompParaPoolWorkflowTracker Trim(this ISorterCompParaPoolWorkflowTracker tracker, int count)
        {
            return new SorterCompParaPoolWorkflowTrackerImpl(
                    sorterPoolStats: GenomePoolStats.Make(tracker.SorterPoolStats.GenomeStatses.OrderBy(t=> ((ISorterOnSwitchableGroup)t.ReferenceResult).SwitchesUsed).Take(count)),
                    switchablePoolStats: GenomePoolStats.Make(tracker.SwitchablePoolStats.GenomeStatses.OrderBy(t => t.ReferenceResult).Take(count))
                );
        }

        public static ISorterOnSwitchableGroup SorterRefScore(ISorterGenome sorterGenome)
        {
            return sorterGenome.ToSorter().FullTest();
        }

        public static object SwitchableGroupRefScore(ISwitchableGroupGenome switchableGroupGenome)
        {
            return 0;
        }
    }

    class SorterCompParaPoolWorkflowTrackerImpl : ISorterCompParaPoolWorkflowTracker
    {
        public SorterCompParaPoolWorkflowTrackerImpl(
            IGenomePoolStats<ISorterGenome> sorterPoolStats,
            IGenomePoolStats<ISwitchableGroupGenome> switchablePoolStats
            )
        {
            _sorterPoolStats = sorterPoolStats;
            _switchablePoolStats = switchablePoolStats;
        }

        public void TrackItem(ISorterCompParaPoolWorkflow sorterCompParaPoolWorkflow)
        {
            //System.Diagnostics.Debug.WriteLine("tracking state: {0}", sorterCompParaPoolWorkflow.CompWorkflowState);
            switch (sorterCompParaPoolWorkflow.CompWorkflowState)
            {
                case CompWorkflowState.ReproGenomes:
                    break;
                case CompWorkflowState.RunCompetition:
                    break;
                case CompWorkflowState.EvaluateResults:
                    break;
                case CompWorkflowState.UpdateGenomes:
                    SorterPoolStats.AddGenomeEvals(
                            sorterCompParaPoolWorkflow.SorterLayerEval
                                .GenomeEvals
                                .OrderBy(t => t.Score),
                            SorterCompParaPoolWorkflowTracker.SorterRefScore
                        );
                    SwitchablePoolStats.AddGenomeEvals(
                            sorterCompParaPoolWorkflow.SwitchableGroupLayerEval
                                .GenomeEvals
                                .OrderBy(t => t.Score),
                            SorterCompParaPoolWorkflowTracker.SwitchableGroupRefScore
                        );


                    var compParaPool = sorterCompParaPoolWorkflow.CompParaPool;
            
                    var compParaPoolScores =
                        compParaPool.SorterOnSwitchableGroups
                            .Where(
                                    g=> SorterPoolStats.GenomeStatses.Any(ps=>ps.Guid==g.Sorter.Guid)
                                        &&
                                        SwitchablePoolStats.GenomeStatses.Any(ps => ps.Guid == g.SwitchableGroupGuid)
                                  )
                            .Select(
                                s =>
                                    new Tuple<Guid, Guid, Tuple<bool, int>>(s.Sorter.Guid, s.SwitchableGroupGuid,
                                        new Tuple<bool, int>(s.Success, s.SwitchesUsed)));

                        var compParaPoolResults = compParaPoolScores.Select(
                        s => new Tuple<IGenomeStats<ISorterGenome>, IGenomeStats<ISwitchableGroupGenome>, Tuple<bool, int>>
                        (
                            SorterPoolStats.GenomeStatses.Single(g=>g.Guid==s.Item1),
                            SwitchablePoolStats.GenomeStatses.Single(g=>g.Guid==s.Item2),
                            s.Item3
                        ));

                    var genomeTable = GenomeTable.Make(compParaPoolResults);

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
