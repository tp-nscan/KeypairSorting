using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeypairSorting.Resources;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    public class ConfigMutateSortersVm : ViewModelBase, IConfigRunSelectorVm
    {

        public ConfigRunTemplateType ConfigRunTemplateType
        {
            get { return ConfigRunTemplateType.Config; }
        }

        public string Description
        {
            get { return "Config mutate sorters"; }
        }
    }
}
