using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genomic
{
    public interface IGenomeGenerator
    {
    }

    public static class GenomeGenerator
    {
        static readonly Dictionary<string, Func<IReadOnlyDictionary<string, object>, IGenome>> GenomeGenerators 
            = new Dictionary<string, Func<IReadOnlyDictionary<string, object>, IGenome>>();

        public static void Register(string key, Func<IReadOnlyDictionary<string, object>, IGenome> generatorFunc)
        {
            GenomeGenerators[key] = generatorFunc;
        }

        public static IGenome ToGenome(this IReadOnlyDictionary<string, object> buildSpec)
        {
            var functionKey = (string)buildSpec["FuncKey"];
            var func = GenomeGenerators[functionKey];
            return func(buildSpec);
        }

        //public static IReadOnlyList<IReadOnlyDictionary<string, object>> ToChromosomeGenerators(
        //    this IReadOnlyDictionary<string, object> buildSpec)
        //{
        //    var chromosomeCount = (int)buildSpec["chromosomeCount"];
        //    List<>
        //}
    }

    class GenomeGeneratorImpl : IGenomeGenerator
    {

    }
}
