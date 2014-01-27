using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using MathUtils.Rand;

namespace MathUtils.Collections
{
    public static class ListPropigatorEx
    {
        public static IReadOnlyList<T> Mutate<T>
            (
                this IReadOnlyList<T> sourceList,
                ListPropigator<T> listPropigator,
                IRando rando
            )
        {
            var track = new Tuple<IImmutableList<T>, int>
            (
                ImmutableList<T>.Empty.AddRange(sourceList),
                0
            );

            while (track.Item2 < listPropigator.FinalIndex)
            {
                track = listPropigator.Propigate(track.Item1, rando, track.Item2);
            }

            return track.Item1;
        }

        public static Tuple<IReadOnlyList<T>, IReadOnlyList<T>> Recombine<T>(this IReadOnlyList<T> first,
            IReadOnlyList<T> second, IRando rando, double recombinationFrequency)
        {
            var firstRet = new List<T>();

            var secondRet = new List<T>();

            return new Tuple<IReadOnlyList<T>, IReadOnlyList<T>>(firstRet, secondRet);
        }
    }

    public class ListPropigator<T>
    {
        private readonly int _finalIndex;
        private readonly double _mutationFrequency;
        private readonly double _insertionFrequency;
        private readonly double _deletionFrequency;
        private readonly Func<IImmutableList<T>, int, IImmutableList<T>> _inserter;
        private readonly Func<IImmutableList<T>, int, IImmutableList<T>> _deletor;
        private readonly Func<IImmutableList<T>, int, IImmutableList<T>> _mutator;

        public ListPropigator(
            Func<IImmutableList<T>, int, IImmutableList<T>> deletor, 
            double deletionFrequency,
            Func<IImmutableList<T>, int, IImmutableList<T>> inserter, 
            double insertionFrequency,
            Func<IImmutableList<T>, int, IImmutableList<T>> mutator, 
            double mutationFrequency, 
            int finalIndex
            )
        {
            _deletor = deletor;
            _deletionFrequency = deletionFrequency;
            _inserter = inserter;
            _insertionFrequency = insertionFrequency;
            _mutator = mutator;
            _mutationFrequency = mutationFrequency;
            _finalIndex = finalIndex;

            _deletionBound = DeletionFrequency;
            _insertionBound = DeletionFrequency + InsertionFrequency;
            _mutationBound = DeletionFrequency + InsertionFrequency + MutationFrequency;
        }

        private readonly double _deletionBound;
        private readonly double _insertionBound;
        private readonly double _mutationBound;

        public Func<IImmutableList<T>, int, IImmutableList<T>> Mutator
        {
            get { return _mutator; }
        }

        public Func<IImmutableList<T>, int, IImmutableList<T>> Deletor
        {
            get { return _deletor; }
        }

        public Func<IImmutableList<T>, int, IImmutableList<T>> Inserter
        {
            get { return _inserter; }
        }

        public double DeletionFrequency
        {
            get { return _deletionFrequency; }
        }

        public double InsertionFrequency
        {
            get { return _insertionFrequency; }
        }

        public double MutationFrequency
        {
            get { return _mutationFrequency; }
        }

        public int FinalIndex
        {
            get { return _finalIndex; }
        }

        public Tuple<IImmutableList<T>, int> Propigate(
            IImmutableList<T> source, 
            IRando rando, 
            int index)
        {
            if ((index == FinalIndex - 1) && (source.Count > FinalIndex )) // trim excess length
            {
                return new Tuple<IImmutableList<T>, int>
                    (
                        source.RemoveRange(FinalIndex, source.Count - FinalIndex), 
                        index + 1
                    );
            }

            if ((index == source.Count) && (source.Count < FinalIndex))  // pad the end using the inserter
            {
                return new Tuple<IImmutableList<T>, int>(
                        Inserter(source, source.Count),
                        index + 1
                    );
            }

            var rand = rando.NextDouble();
            if (rand > _mutationBound) // no change
            {
                return new Tuple<IImmutableList<T>, int>(source, index + 1);
            }

            if (rand > _insertionBound) // mutate
            {
                return new Tuple<IImmutableList<T>, int>(
                    Mutator(source, index),
                    index + 1
                );
            }

            if (rand > _deletionBound) // insert
            {
                return new Tuple<IImmutableList<T>, int>(
                    Inserter(source, index),
                    index + 1
                );
            }

            // delete
            {
                return new Tuple<IImmutableList<T>, int>(
                    Deletor(source, index),
                    index
                );
            }
        }


    }

}
