using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Entities.BackgroundWorkers
{
    public interface IRecursiveBackgroundWorker<T>
    {
        IObservable<IIterationResult<T>> OnIterationResult { get; }
        Task Start();
        void Stop();
        int CurrentIteration { get; }
        T CurrentState { get; }
        int TotalIterations { get; }
    }

    public static class RecursiveBackgroundWorker
    {
        public static IRecursiveBackgroundWorker<T> Make<T>
            (
                T initialState,
                Func<T, CancellationToken, IIterationResult<T>> recursion,
                int totalIterations,
                CancellationTokenSource cancellationTokenSource
            )
        {
            return new RecursiveBackgroundWorkerImpl<T>
                (
                    recursion: recursion,
                    initialState: initialState,
                    totalIterations: totalIterations,
                    cancellationTokenSource: cancellationTokenSource
                );
        }
    }

    class RecursiveBackgroundWorkerImpl<T> : IRecursiveBackgroundWorker<T>
    {
        public RecursiveBackgroundWorkerImpl
            (
                Func<T, CancellationToken, IIterationResult<T>> recursion, 
                T initialState, 
                int totalIterations,
                CancellationTokenSource cancellationTokenSource
            )
        {
            _recursion = recursion;
            _currentIteration = 0;
            _currentState = initialState;
            _totalIterations = totalIterations;
            _cancellationTokenSource = cancellationTokenSource;
        }

        private readonly Func<T, CancellationToken, IIterationResult<T>> _recursion;

        private readonly Subject<IIterationResult<T>> _onIterationResult = new Subject<IIterationResult<T>>();
        public IObservable<IIterationResult<T>> OnIterationResult
        {
            get { return _onIterationResult; }
        }

        public async Task Start()
        {
            var keepGoing = true;
            while ((CurrentIteration < TotalIterations) & keepGoing)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    _onIterationResult.OnNext(
                        IterationResult.Make(default(T), ProgressStatus.StepIncomplete)
                    );
                    keepGoing = false;
                }

                var result = IterationResult.Make(default(T), ProgressStatus.StepIncomplete);
                await Task.Run(() => result = _recursion(CurrentState, _cancellationTokenSource.Token));

                if (result.ProgressStatus != ProgressStatus.StepComplete)
                {
                    keepGoing = false;
                }
                else
                {
                    _currentState = result.Data;
                    _onIterationResult.OnNext(result);
                    _currentIteration++;
                }
            }
        }

        private readonly CancellationTokenSource _cancellationTokenSource;
        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private int _currentIteration;
        public int CurrentIteration
        {
            get { return _currentIteration; }
        }

        private T _currentState;
        public T CurrentState
        {
            get { return _currentState; }
        }

        private readonly int _totalIterations;
        public int TotalIterations
        {
            get { return _totalIterations; }
        }
    }
}
