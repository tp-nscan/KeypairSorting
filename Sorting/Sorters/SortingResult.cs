
using Sorting.Switchables;

namespace Sorting.Sorters
{
    public interface ISortingResult<T>
    {
        ISwitchable<T> Switchable { get; }
        T Result { get; }
        bool Success { get; }
    }

    public static class SortingResult
    {
        public static ISortingResult<T> ToSortingResult<T>(
            this ISwitchable<T> switchable, T result, bool success)
        {
            return new SortingResultImpl<T>(switchable: switchable, result: result, success: success);
        }
    }

    public class SortingResultImpl<T> : ISortingResult<T>
    {
        private readonly ISwitchable<T> _switchable;
        private readonly T _result;
        private readonly bool _success;

        public SortingResultImpl(ISwitchable<T> switchable, T result, bool success)
        {
            _switchable = switchable;
            _result = result;
            _success = success;
        }

        public ISwitchable<T> Switchable
        {
            get { return _switchable; }
        }

        public T Result
        {
            get { return _result; }
        }

        public bool Success
        {
            get { return _success; }
        }
    }
}
