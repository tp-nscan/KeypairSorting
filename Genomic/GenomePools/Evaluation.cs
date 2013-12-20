using MathUtils.Collections;

namespace Genomic.GenomePools
{
    public interface IEvaluation<T>
        where T: IGuid
    {
        T Item { get; }
        double Score { get; }
    }

    public static class Evaluation
    {
        public static IEvaluation<T> Make<T>(this T item, double score)
            where T: IGuid
        {
            return new EvaluationImpl<T>(item, score);
        }
    }

    class EvaluationImpl<T> : IEvaluation<T>
        where T: IGuid
    {
        private readonly T _item;
        private readonly double _score;

        public EvaluationImpl(T item, double score)
        {
            _item = item;
            _score = score;
        }

        public T Item
        {
            get { return _item; }
        }

        public double Score
        {
            get { return _score; }
        }
    }
}
