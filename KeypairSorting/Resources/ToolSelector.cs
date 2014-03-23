using System.Windows;
using System.Windows.Controls;

namespace KeypairSorting.Resources
{
    public interface IToolTemplateVm
    {
        ToolTemplateType ToolTemplateType { get; }
        string Description { get; }
    }

    public enum ToolTemplateType
    {
        RandomGenome,
        SorterEval,
        SorterGenomeEval,
        SorterGenomeGen,
        SorterSwitchableEval,
        SwitchableGen,
        SorterMutate,
        SorterTune,
        MultiSorterTune,
        None
    }

    public class ToolSelector : DataTemplateSelector 
    {
        public DataTemplate DefaultTemplate { get; set; }

        public DataTemplate MultiSorterTuneTemplate { get; set; }

        public DataTemplate RandomGenomeTemplate { get; set; }

        public DataTemplate SorterEvalTemplate { get; set; }

        public DataTemplate SorterMuatateTemplate { get; set; }

        public DataTemplate SorterGenomeEvalTemplate { get; set; }

        public DataTemplate SorterGenomeGenTemplate { get; set; }

        public DataTemplate SorterSwitchableEvalTemplate { get; set; }

        public DataTemplate SwitchableGenTemplate { get; set; }

        public DataTemplate SorterTuneTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tabItem = item as IToolTemplateVm;

            if (tabItem != null)
                switch (tabItem.ToolTemplateType)
                {
                    case ToolTemplateType.MultiSorterTune:
                        return MultiSorterTuneTemplate;
                    case ToolTemplateType.RandomGenome:
                        return RandomGenomeTemplate;
                    case ToolTemplateType.SorterEval:
                        return SorterEvalTemplate;
                    case ToolTemplateType.SorterGenomeGen:
                        return SorterGenomeGenTemplate;
                    case ToolTemplateType.SorterGenomeEval:
                        return SorterGenomeEvalTemplate;
                    case ToolTemplateType.SorterMutate:
                        return SorterMuatateTemplate;
                    case ToolTemplateType.SorterSwitchableEval:
                        return SorterSwitchableEvalTemplate;
                    case ToolTemplateType.SwitchableGen:
                        return SwitchableGenTemplate;
                    case ToolTemplateType.SorterTune:
                        return SorterTuneTemplate;
                    default:
                        return DefaultTemplate;
                }
            return DefaultTemplate;
        }
    }
}
