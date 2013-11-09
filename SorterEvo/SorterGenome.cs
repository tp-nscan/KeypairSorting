using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorterEvo
{
    public interface ISorterGenome
    {

    }

    public static class SorterGenome
    {
        public static ISorterGenome Make()
        {
            return new SorterGenomeImpl();
        }
    }

    class SorterGenomeImpl : ISorterGenome
    {

    }

}
