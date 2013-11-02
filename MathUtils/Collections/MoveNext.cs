using System.Collections.Generic;

namespace MathUtils.Collections
{
    public interface IMoveNext<T>
    {
        T Next();
    }

    public static class MoveNext
    {
        public static IMoveNext<T> ToMoveNext<T>(this IEnumerable<T> enumerable)
        {
            return new MoveNextImpl<T>(enumerable);
        }
    }

    public class MoveNextImpl<T> : IMoveNext<T>
    {
        public MoveNextImpl(IEnumerable<T> enumerable)
        {
            _enumerator = enumerable.GetEnumerator();
        }

        private readonly IEnumerator<T> _enumerator; 

        public T Next()
        {
            _enumerator.MoveNext();
            return _enumerator.Current;
        }
    }
}
