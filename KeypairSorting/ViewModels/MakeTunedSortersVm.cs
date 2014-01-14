using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Entities.BackgroundWorkers;
using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using MathUtils.Rand;
using SorterEvo.Layers;
using SorterEvo.Workflows;
using WpfUtils;

namespace KeypairSorting.ViewModels
{
    public class MakeTunedSortersVm : ViewModelBase, IToolTemplateVm
    {
        public MakeTunedSortersVm()
        {
            _sorterGenomeEvalGridVmInitial = new SorterGenomeEvalGridVm();
            _sorterCompPoolParamsVm = new SorterCompPoolParamsVm
                (
                    SorterCompPoolParams.Make(
                        sorterLayerStartingGenomeCount: 10,
                        sorterLayerExpandedGenomeCount: 30,
                        sorterMutationRate: 0.03,
                        sorterInsertionRate: 0.03,
                        sorterDeletionRate: 0.03,
                        name: "standard"
                    )
                );
            _sorterGenomeEvalGridVm = new SorterGenomeEvalGridVm();
            _stopwatch = new Stopwatch();
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterTune; }
        }


        public string Description
        {
            get { return "Optimize Sorters"; }
        }

        private bool _busy;
        public bool Busy
        {
            get { return _busy; }
            set
            {
                _busy = value;
                CommandManager.InvalidateRequerySuggested();
                OnPropertyChanged("Busy");
            }
        }

        private int? _seed;
        public int? Seed
        {
            get { return _seed; }
            set
            {
                _seed = value;
                OnPropertyChanged("Seed");
            }
        }

        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVmInitial;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVmInitial
        {
            get { return _sorterGenomeEvalGridVmInitial; }
        }

        private readonly SorterCompPoolParamsVm _sorterCompPoolParamsVm;
        public SorterCompPoolParamsVm SorterCompPoolParamsVm
        {
            get { return _sorterCompPoolParamsVm; }
        }

        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVm;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVm
        {
            get { return _sorterGenomeEvalGridVm; }
        }

        #region RunCommand

        RelayCommand _runCommand;
        public ICommand RunCommand
        {
            get
            {
                return _runCommand ?? (_runCommand
                    = new RelayCommand
                        (
                            param => OnRunCommand(),
                            param => CanRunCommand()
                        ));
            }
        }

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        //ISorterCompPoolWorkflow
        async Task OnRunCommand()
        {
            Busy = true;
            _stopwatch.Reset();
            _stopwatch.Start();
            _cancellationTokenSource = new CancellationTokenSource();

            var rando = Rando.Fast(Seed.Value);


            var rbw = RecursiveBackgroundWorker.Make
                (
                    initialState: SorterCompPoolWorkflow.Make
                        (
                           sorterLayer: SorterLayer.Make(
                               sorterGenomes: _sorterGenomeEvalGridVmInitial.SorterGenomeEvalVms.Select(t=>t.GetSorterGenomeEval().SorterGenome) ,
                               generation: 0
                           ),
                           sorterCompPoolParams: SorterCompPoolParamsVm.GetParams,
                           generation: 0
                        ),
                   recursion: (i, c) =>
                        {
                            if (_cancellationTokenSource.IsCancellationRequested)
                            {
                                return IterationResult.Make<ISorterCompPoolWorkflow>(null, ProgressStatus.StepIncomplete);
                            }

                            return IterationResult.Make(i.Step(rando.NextInt()), ProgressStatus.StepComplete);
                        },
                    totalIterations: int.MaxValue,
                    cancellationTokenSource: _cancellationTokenSource
                );

            rbw.OnIterationResult.Subscribe(UpdateSorterTuneResults);
            await rbw.Start();

            _stopwatch.Stop();
            Busy = false;
        }

        private void UpdateSorterTuneResults(IIterationResult<ISorterCompPoolWorkflow> result)
        {
            if (result.ProgressStatus == ProgressStatus.StepComplete)
            {
                _generation = result.Data.Generation;
                OnPropertyChanged("Generation");
                OnPropertyChanged("ProcTime");
                foreach (var sorterEval in result.Data.CompPool.SorterEvals)
                {
                    //SorterGenomeEvalGridVm.SorterGenomeEvalVms.Add(sorterEval.ToSorterEvalVm());
                }
            }
        }

        bool CanRunCommand()
        {
            return 
                !_busy 
                && Seed.HasValue 
                && SorterCompPoolParamsVm.HasValidData;
        }

        #endregion // RunCommand


        #region StopCommand

        RelayCommand _stopCommand;

        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand
                    = new RelayCommand
                        (
                            param => OnStopCommand(),
                            param => CanStopCommand()
                        ));
            }
        }

        void OnStopCommand()
        {
            _cancellationTokenSource.Cancel();
        }

        bool CanStopCommand()
        {
            return _busy;
        }

        #endregion // StopCommand


        private readonly Stopwatch _stopwatch;
        public string ProcTime
        {
            get { return _stopwatch.Elapsed.TotalSeconds.ToString("0"); }
        }

        private int _generation = 0;
        public int Generation
        {
            get { return _generation; }
        }
    }
}
