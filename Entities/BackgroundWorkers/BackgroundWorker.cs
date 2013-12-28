using System;
using System.Threading;
using System.Threading.Tasks;

namespace Entities.BackgroundWorkers
{
    public interface IBackgroundWorker<in TS, T>
    {
        ProgressStatus BackgroundWorkerStatus { get; }
        Task<T> RunAsync(TS arg);
        void Cancel();
    }

    public static class BackgroundWorker
    {
        public static IBackgroundWorker<TS, T> Make<TS, T>(Func<TS, T> opFunc)
        {
            return new BackgroundWorkerImpl<TS, T>(
                opFunc: opFunc);
        }
    }


    class BackgroundWorkerImpl<TS, T> : IBackgroundWorker<TS, T>
    {
        public BackgroundWorkerImpl(Func<TS, T> opFunc)
        {
            _opFunc = opFunc;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            //_backgroundWorkerStatus = ProgressStatus.None;
        }

        private readonly Func<TS, T> _opFunc;

        private ProgressStatus _backgroundWorkerStatus;
        public ProgressStatus BackgroundWorkerStatus
        {
            get { return _backgroundWorkerStatus; }
        }

        public async Task<T> RunAsync(TS arg)
        {
            var result = default(T);
            //_cancellationTokenSource = new CancellationTokenSource();
            //_cancellationTokenSource.Token.ThrowIfCancellationRequested();
            //_backgroundWorkerStatus = ProgressStatus.Running;
            try
            {
                await Task.Run
                (
                    () => result = _opFunc(arg)
                    ,
                    _cancellationTokenSource.Token
                );
            }
            catch (Exception)
            {
                //_backgroundWorkerStatus = ProgressStatus.Cancelled;
                return result;
            }
            //_backgroundWorkerStatus = ProgressStatus.Completed;
            return result;
        }

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
