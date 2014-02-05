using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Entities.BackgroundWorkers;
using KeypairSorting.ViewModels.Parts;
using SorterEvo.Evals;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    public class MutateSortersRunnerVm : ViewModelBase
    {
        public MutateSortersRunnerVm(ISorterMutateParams sorterMutateParams, IEnumerable<ISorterGenomeEvalVm> sorterGenomeEvalVms)
        {
            var genomeEvalVms = sorterGenomeEvalVms.ToList();

            _sorterGenomeEvalGridVmInitial = new SorterGenomeEvalGridVm("Progenitors");
            _sorterGenomeEvalGridVmInitial.SorterGenomeEvalVms.AddMany(genomeEvalVms);
            _sgMutantProfileGridVm = new SgMutantProfileGridVm("Mutants");
            _sorterMutateParamsVm = new SorterMutateParamsVm(sorterMutateParams);
        }

        private readonly Subject<IEnumerable<ISorterGenomeEval>> _onIterationResult
                = new Subject<IEnumerable<ISorterGenomeEval>>();
        public IObservable<IEnumerable<ISorterGenomeEval>> OnIterationResult
        {
            get { return _onIterationResult; }
        }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            private set
            {
                _busy = value;
                OnPropertyChanged("Busy");
            }
        }

        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVmInitial;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVmInitial
        {
            get { return _sorterGenomeEvalGridVmInitial; }
        }

        private readonly SgMutantProfileGridVm _sgMutantProfileGridVm;
        public SgMutantProfileGridVm SgMutantProfileGridVm
        {
            get { return _sgMutantProfileGridVm; }
        }

        private readonly SorterMutateParamsVm _sorterMutateParamsVm;
        public SorterMutateParamsVm SorterMutateParamsVm
        {
            get { return _sorterMutateParamsVm; }
        }


        #region Run functions

        private CancellationTokenSource _cancellationTokenSource;

        public async Task OnRunAsync(CancellationTokenSource cancellationTokenSource)
        {

            var rbw = EnumerativeBackgroundWorker.Make
                        (
                            inputs: SorterGenomeEvalGridVmInitial.SorterGenomeEvalVms
                                                                 .Select(
                                         ev => new Tuple<ISorterGenomeEval, ISorterMutateParams>
                                             (
                                                ev.SorterGenomeEval, 
                                                SorterMutateParamsVm.GetParams
                                             )),
                            mapper: (s, c) =>
                            {
                                return IterationResult.Make
                                    (
                                        data: s.Item1.ToSgMutantProfile(s.Item2),
                                        progressStatus: ProgressStatus.StepComplete
                                    );
                            }
                        );

            Busy = true;
            //_cancellationTokenSource = cancellationTokenSource;

            rbw.OnIterationResult.Subscribe(UpdateSorterMutateResults);
            await rbw.Start(cancellationTokenSource);

            Busy = false;
        }

        public void UpdateSorterMutateResults(IIterationResult<ISgMutantProfile> result)
        {
            SgMutantProfileGridVm
                .SgMutantProfileVms.Add(
                        result.Data.ToSgMutantProfileVm()
                );

            _onIterationResult.OnNext(result.Data.SorterGenomeEvals);
        }

        #endregion // Run functions

        public int? ReportFrequency { get; set; }

    }
}
