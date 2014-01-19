using System.Collections;
using System.Collections.Generic;
using KeypairSorting.Resources;
using WpfUtils;

namespace KeypairSorting.ViewModels.MakeTunedSorters
{
    public class MultiRunScpVm : ViewModelBase, IConfigRunSelectorVm
    {
        public MultiRunScpVm(IEnumerable<ConfigScpVm> configScpVms)
        {
            
        }

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Run; }
        }

        public string Description
        {
            get { return "Tune sorters"; }
        }
    }
}
