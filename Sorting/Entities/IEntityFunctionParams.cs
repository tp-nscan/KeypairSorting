using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting.Entities
{
    public interface IEntityFunctionParams
    {
    }

    public enum EntityFunctionParamsType
    {
        SorterRandGen,
        SwitchableRandGen,
        SorterMutate,
        SwitchableMutate,

    }
}
