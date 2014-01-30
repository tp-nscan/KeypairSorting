using KeypairSorting.Resources;
using KeypairSorting.ViewModels.Parts;
using WpfUtils;

namespace KeypairSorting.ViewModels.MutateSorters
{
    public class MutateSortersVm : ViewModelBase, IToolTemplateVm
    {
        public MutateSortersVm()
        {
            _sorterGenomeEvalGridVmInitial = new SorterGenomeEvalGridVm("Parents");
            _sorterGenomeEvalGridVm = new SorterGenomeEvalGridVm("Mutants");
        }

        public ToolTemplateType ToolTemplateType
        {
            get { return ToolTemplateType.SorterMutate; }
        }

        public string Description
        {
            get { return "Convert Sorters to SorterGenomes"; }
        }


        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVmInitial;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVmInitial
        {
            get { return _sorterGenomeEvalGridVmInitial; }
        }

        private readonly SorterGenomeEvalGridVm _sorterGenomeEvalGridVm;
        public SorterGenomeEvalGridVm SorterGenomeEvalGridVm
        {
            get { return _sorterGenomeEvalGridVm; }
        }
    }
}
