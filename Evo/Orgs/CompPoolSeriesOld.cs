using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Evo.Genomes;
using MathUtils.Collections;
using MathUtils.Rand;


namespace Evo.Orgs
{
    public interface ICompPoolSeries<TG, TP>
        where TG : IGenome
        where TP : IGuid
    {
        ICompPool_Old<TG, TP> CurrentPoolOld { get; }
        int CurrentSeed { get; }
        Guid Id { get; }
        Func<IOrg<TG, TP>, int, IOrg<TG, TP>> CopyOrgFunc { get; }
        Func<int, IOrg<TG, TP>> InitOrgFunc { get; }
        int StartingSeed { get; }
        int StartingSize { get; }
    }

    public static class CompPoolSeriesOld
    {

        public static ICompPoolSeries<TG, TP> Update<TG, TP>
            (
                this ICompPoolSeries<TG, TP> compPool,
                IReadOnlyList<Tuple<TP, double>> scores,
                int selectionRatio
            )
            where TG : IGenome
            where TP : IGuid
        {
            var randy = Rando.Fast(compPool.CurrentSeed);

            var winners = scores.OrderByDescending(t => t.Item2)
                .Take(scores.Count/selectionRatio)
                .Select(p => compPool.CurrentPoolOld.GetOrg(p.Item1.Guid))
                .ToList();

            return Make
                (
                    id: randy.NextGuid(),
                    copyOrgFunc: compPool.CopyOrgFunc,
                    initOrgFunc: compPool.InitOrgFunc,
                    startingSeed: compPool.StartingSeed,
                    currentSeed: Rando.Fast(compPool.CurrentSeed ^ compPool.CurrentSeed).NextInt(),
                    startingSize: compPool.StartingSize,
                    currentPoolOld: CompPool_Old.Make
                        (
                            comPoolId: randy.NextGuid(),
                            generation: compPool.CurrentPoolOld.Generation +1,
                            orgs:winners.Repeat().Take(compPool.StartingSize)
                                                 .Select(p=> compPool.CopyOrgFunc(p, randy.NextInt()))
                        )
                );
        }


        public static ICompPoolSeries<TG, TP> Create<TG, TP>
        (
            Guid id,
            Func<IOrg<TG, TP>, int, IOrg<TG, TP>> copyOrgFunc,
            Func<int, IOrg<TG, TP>> initOrgFunc,
            int startingSeed,
            int startingSize
        )
            where TG : IGenome
            where TP : IGuid
        {
            var randy = Rando.Fast(startingSeed);

            return Make
                (
                    id:id,
                    copyOrgFunc:copyOrgFunc,
                    initOrgFunc:initOrgFunc,
                    startingSeed:startingSeed,
                    currentSeed:startingSeed,
                    startingSize:startingSize,
                    currentPoolOld: CompPool_Old.Make
                                (
                                    comPoolId: randy.NextGuid(),
                                    generation: 0,
                                    orgs: Enumerable.Range(0, startingSize).Select(i => initOrgFunc(randy.NextInt()))
                                )
                );
        }



        static ICompPoolSeries<TG, TP> Make<TG, TP>
            (
                Guid id,
                Func<IOrg<TG, TP>, int, IOrg<TG, TP>> copyOrgFunc,
                Func<int, IOrg<TG, TP>> initOrgFunc,
                int startingSeed,
                int currentSeed,
                int startingSize,
                ICompPool_Old<TG, TP> currentPoolOld
            )
            where TG : IGenome
            where TP : IGuid
        {
            return new CompPoolSeriesImpl<TG, TP>
                (
                    id: id,
                    initOrgFunc: initOrgFunc,
                    copyOrgFunc: copyOrgFunc,
                    startingSeed: startingSeed,
                    startingSize: startingSize,
                    currentPoolOld: currentPoolOld,
                    currentSeed: currentSeed
                );
        }
    }

    class CompPoolSeriesImpl<TG, TP> : ICompPoolSeries<TG, TP>
        where TG : IGenome
        where TP : IGuid
    {
        private readonly int _startingSeed;
        private readonly int _startingSize;
        private readonly ICompPool_Old<TG, TP> _currentPoolOld;
        private readonly Func<int, IOrg<TG, TP>> _initOrgFunc;
        private readonly Guid _id;
        private readonly Func<IOrg<TG, TP>, int, IOrg<TG, TP>> _copyOrgOrgFunc;
        private readonly int _currentSeed;

        public CompPoolSeriesImpl
            (
                Guid id, 
                Func<int, IOrg<TG, TP>> initOrgFunc,
                Func<IOrg<TG, TP>, int, IOrg<TG, TP>> copyOrgFunc,
                int startingSeed, 
                int startingSize, 
                ICompPool_Old<TG, TP> currentPoolOld, 
                int currentSeed
            )
        {
            _id = id;
            _initOrgFunc = initOrgFunc;
            _copyOrgOrgFunc = copyOrgFunc;
            _startingSeed = startingSeed;
            _startingSize = startingSize;
            _currentPoolOld = currentPoolOld;
            _currentSeed = currentSeed;
        }

        public ICompPool_Old<TG, TP> CurrentPoolOld
        {
            get { return _currentPoolOld; }
        }

        public int CurrentSeed
        {
            get { return _currentSeed; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public Func<IOrg<TG, TP>, int, IOrg<TG, TP>> CopyOrgFunc
        {
            get { return _copyOrgOrgFunc; }
        }

        public Func<int, IOrg<TG, TP>> InitOrgFunc
        {
            get { return _initOrgFunc; }
        }

        public int StartingSeed
        {
            get { return _startingSeed; }
        }

        public int StartingSize
        {
            get { return _startingSize; }
        }
    }
}
