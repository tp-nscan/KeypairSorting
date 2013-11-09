﻿using System;
using System.Collections.Generic;
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
        IOrg<TG, TP> GetOrg(Guid orgGuid);
        int Generation { get; }
        IReadOnlyList<IOrg<TG, TP>> Orgs { get; }
    }

    public static class CompPool
    {
        public static ICompPool<TG, TP> Make<TG, TP>
            (
                Guid comPoolId,
                int generation,
                IEnumerable<IOrg<TG, TP>> orgs
            ) where TG : IGenome where TP : IGuid
        {
            return new CompPoolImpl<TG, TP>
                (
                    comPoolId,
                    generation,
                    orgs
                );
        }
    }

    class CompPoolImpl<TG, TP> : ICompPool<TG, TP>
        where TG : IGenome
        where TP : IGuid
    {
        private readonly IReadOnlyDictionary<Guid, IOrg<TG, TP>> _orgs;
        private readonly Guid _comPoolId;
        private readonly int _generation;

        public CompPoolImpl
            (
                Guid comPoolId, 
                int generation, 
                IEnumerable<IOrg<TG, TP>> orgs
            )
        {
            _comPoolId = comPoolId;
            _generation = generation;
            _orgs = orgs.ToDictionary(t=>t.Genome.Guid);
        }

        public Guid ComPoolId
        {
            get { return _comPoolId; }
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
