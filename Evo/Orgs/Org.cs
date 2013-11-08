using Evo.Genomes;
using MathUtils.Collections;

namespace Evo.Orgs
{
    public interface IOrg<out TG, out TP> 
        where TG : IGenome 
        where TP : IGuid
    {
        TG Genome { get; }
        TP Phenome { get; }
    }

    public static class Org
    {
        public static IOrg<TG, TP> ToOrg<TG, TP>(this TG genome, TP phenotype)
            where TG : IGenome
            where TP : IGuid
        {
            return new OrgImpl<TG, TP>(genome, phenotype);
        }
    }

    class OrgImpl<TG, TP> : IOrg<TG, TP>
        where TG : IGenome
        where TP : IGuid
    {
        private readonly TG _genome;
        private readonly TP _phenome;

        public OrgImpl(TG genome, TP phenome)
        {
            _genome = genome;
            _phenome = phenome;
        }

        public TG Genome
        {
            get { return _genome; }
        }

        public TP Phenome
        {
            get { return _phenome; }
        }
    }
}
