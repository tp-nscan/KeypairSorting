using System;
using Evo.Genomes;
using MathUtils.Collections;

namespace Evo.Orgs
{
    public interface ICompPoolSeries<TG, TP>
        where TG : IGenome
        where TP : IGuid
    {
        ICompPool<TG, TP> CuurentPool { get; }
        Guid Id { get; }
        Func<int, IOrg<TG, TP>> InitOrgFunc { get; }
        int Seed { get; }
        int StartingSize { get; }
    }

    public static class CompPoolSeries
    {

    }

    class CompPoolSeriesImpl<TG, TP> : ICompPoolSeries<TG, TP>
        where TG : IGenome
        where TP : IGuid
    {
        private readonly int _seed;
        private readonly int _startingSize;
        private readonly ICompPool<TG, TP> _cuurentPool;
        private readonly Func<int, IOrg<TG, TP>> _initOrgFunc;
        private readonly Guid _id;

        public CompPoolSeriesImpl
            (
                Guid id, 
                Func<int, IOrg<TG, TP>> initOrgFunc, 
                int seed, 
                int startingSize, 
                ICompPool<TG, TP> cuurentPool
            )
        {
            _id = id;
            _initOrgFunc = initOrgFunc;
            _seed = seed;
            _startingSize = startingSize;
            _cuurentPool = cuurentPool;
        }


        public ICompPool<TG, TP> CuurentPool
        {
            get { return _cuurentPool; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public Func<int, IOrg<TG, TP>> InitOrgFunc
        {
            get { return _initOrgFunc; }
        }

        public int Seed
        {
            get { return _seed; }
        }

        public int StartingSize
        {
            get { return _startingSize; }
        }
    }
}
