namespace Entities.BackgroundWorkers
{
    public interface IIterationResult<T>
    {
        T Data { get; }
        ProgressStatus ProgressStatus { get; }
    }

    public static class IterationResult
    {
        public static IIterationResult<T> Make<T>(T data, ProgressStatus progressStatus)
        {
            return new IterationResultImpl<T>(data: data, progressStatus: progressStatus);
        }
    }

    class IterationResultImpl<T> : IIterationResult<T>
    {
        public IterationResultImpl(T data, ProgressStatus progressStatus)
        {
            _data = data;
            _progressStatus = progressStatus;
        }

        private readonly T _data;
        public T Data
        {
            get { return _data; }
        }

        private readonly ProgressStatus _progressStatus;
        public ProgressStatus ProgressStatus
        {
            get { return _progressStatus; }
        }
    }
}
