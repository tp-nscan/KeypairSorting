using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Evo.Genomes;
using MathUtils.Collections;

namespace Evo.Orgs
{
    public interface ICompPool<TG, TP>
        where TG : IGenome
        where TP : IGuid
    {
        Guid ComPoolId { get; }
        Func<IOrg<TG, TP>, int, IOrg<TG, TP>> CopyFunc { get; }
        IOrg<TG, TP> GetOrg(Guid orgGuid);
        int Generation { get; }
        IReadOnlyList<IOrg<TG, TP>> Orgs { get; }
    }

    public static class CompPool
    {

    }

    abstract class CompPoolImpl<TG, TP> : ICompPool<TG, TP>
        where TG : IGenome
        where TP : IGuid
    {
        private readonly IReadOnlyDictionary<Guid, IOrg<TG, TP>> _orgs;
        private readonly Guid _comPoolId;
        private readonly int _generation;
        private readonly Func<IOrg<TG, TP>, int, IOrg<TG, TP>> _copyFunc;

        protected CompPoolImpl
            (
                Guid comPoolId, 
                int generation, 
                IEnumerable<IOrg<TG, TP>> orgs,
                Func<IOrg<TG, TP>, int, IOrg<TG, TP>> copyFunc
            )
        {
            _comPoolId = comPoolId;
            _generation = generation;
            _orgs = orgs.ToDictionary(t=>t.Genome.Guid);
            _copyFunc = copyFunc;
        }

        public Guid ComPoolId
        {
            get { return _comPoolId; }
        }

        public Func<IOrg<TG, TP>, int, IOrg<TG, TP>> CopyFunc
        {
            get { return _copyFunc; }
        }

        public IOrg<TG, TP> GetOrg(Guid orgGuid)
        {
            return _orgs[orgGuid];
        }

        public int Generation
        {
            get { return _generation; }
        }

        private List<IOrg<TG, TP>> _orgsList;
        public IReadOnlyList<IOrg<TG, TP>> Orgs
        {
            get {
                return _orgsList ?? (_orgsList = new List<IOrg<TG, TP>>(_orgs.Values));
            }
        }
    }
}
