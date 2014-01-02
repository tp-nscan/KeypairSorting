using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Entities.BackgroundWorkers
{
    public interface IEnumerativeBackgroundWorker<TD, TR>
    {
        IObservable<IIterationResult<TR>> OnIterationResult { get; }
        Task Start(CancellationTokenSource cancellationTokenSource);
        void Stop();
        int CurrentIteration { get; }
        TD CurrentInput { get; }
        TR CurrentOutput { get; }
        int TotalIterations { get; }
    }

    public static class EnumerativeBackgroundWorker
    {
        public static IEnumerativeBackgroundWorker<TD, TR> Make<TD, TR>
        (
            IEnumerable<TD> inputs,
            Func<TD, CancellationToken, IIterationResult<TR>> mapper
        )
            {
                return new EnumerativeBackgroundWorkerImpl<TD, TR>
                    (
                        inputs: inputs,
                        mapper: mapper
                    );
            }
    }

    class EnumerativeBackgroundWorkerImpl<TD, TR> : IEnumerativeBackgroundWorker<TD, TR>
    {
        public EnumerativeBackgroundWorkerImpl
            (
                IEnumerable<TD> inputs,
                Func<TD, CancellationToken, IIterationResult<TR>> mapper
            )
        {
            _inputs = inputs.ToList();
            _mapper = mapper;
        }

        private readonly List<TD> _inputs;
        private readonly Func<TD, CancellationToken, IIterationResult<TR>> _mapper;

        private readonly Subject<IIterationResult<TR>> _onIterationResult = new Subject<IIterationResult<TR>>();
        public IObservable<IIterationResult<TR>> OnIterationResult
        {
            get { return _onIterationResult; }
        }

        public async Task Start(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            var keepGoing = true;
            while ((CurrentIteration < TotalIterations) & keepGoing)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    _onIterationResult.OnNext(
                        IterationResult.Make(default(TR), ProgressStatus.StepIncomplete)
                    );
                    keepGoing = false;
                }

                var result = IterationResult.Make(default(TR), ProgressStatus.StepIncomplete);
                await Task.Run(() => result = _mapper(CurrentInput, cancellationTokenSource.Token));

                if (result.ProgressStatus != ProgressStatus.StepComplete)
                {
                    keepGoing = false;
                }
                else
                {
                    _currentOutput = result.Data;
                    _onIterationResult.OnNext(result);
                    _currentIteration++;
                }
            }
        }

        private CancellationTokenSource _cancellationTokenSource;

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private int _currentIteration;
        public int CurrentIteration
        {
            get { return _currentIteration; }
        }

        public TD CurrentInput
        {
            get 
            {
                return (CurrentIteration < TotalIterations) ? 
                _inputs[CurrentIteration] : default(TD); 
            }
        }

        private TR _currentOutput;
        public TR CurrentOutput
        {
            get { return _currentOutput; }
        }

        public int TotalIterations
        {
            get { return _inputs.Count; }
        }
    }
}
