using System;
using SorterEvo.Evals;
using SorterEvo.Json.Evals;
using WpfUtils;

namespace KeypairSorting.ViewModels.Parts
{
    public interface ISorterGenomeEvalVm
    {
       // int SwitchesUsed { get; }
        //Guid SwitchableGroupGuid { get; }
        //bool Success { get; }
        string SorterGenomeEvalJson { get; }
        //Guid SorterGuid { get; }
    }

    public static class SorterGenomeEvalVm
    {
        public static ISorterGenomeEvalVm ToSorterGenomeEvalVm(this ISorterGenomeEval sorterGenomeEval)
        {
            return new SorterGenomeEvalVmImpl(sorterGenomeEval);
        }
    }

    public class SorterGenomeEvalVmImpl : ViewModelBase, ISorterGenomeEvalVm
    {
        public SorterGenomeEvalVmImpl(ISorterGenomeEval sorterGenomeEval)
        {
            _sorterGenomeEval = sorterGenomeEval;
        }

        private readonly ISorterGenomeEval _sorterGenomeEval;
        //public ISorterGenomeEval SorterGenomeEval
        //{
        //    get { return _sorterGenomeEval; }
        //}

        //public int SwitchesUsed
        //{
        //    get { return _sorterGenomeEval.SorterEval.SwitchesUsed; }
        //}

        //public Guid SwitchableGroupGuid
        //{
        //    get { return _sorterGenomeEval.SorterEval.SwitchableGroupGuid; }
        //}

        //public bool Success
        //{
        //    get { return _sorterGenomeEval.SorterEval.Success; }
        //}

        public string SorterGenomeEvalJson
        {
            get { return _sorterGenomeEval.ToJsonString(); }
        }

        //public Guid SorterGuid
        //{
        //    get { return _sorterGenomeEval.SorterEval.Sorter.Guid; }
        //}
    }
}
