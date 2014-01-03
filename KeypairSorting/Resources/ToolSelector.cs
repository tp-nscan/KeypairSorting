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
        RandomSorter,
        SorterEval,
        SorterGenomeGen,
        SorterSwitchableEval,
        SwitchableGen,
        SorterTune,
        None
    }

    public class ToolSelector : DataTemplateSelector
    {
        #region DefaultTemplate

        public DataTemplate DefaultTemplate { get; set; }

        #endregion

        #region RandomSorterTemplate

        public DataTemplate RandomSorterTemplate { get; set; }

        #endregion

        #region SorterEvalTemplate

        public DataTemplate SorterEvalTemplate { get; set; }

        #endregion

        #region SorterGenomeGenTemplate

        public DataTemplate SorterGenomeGenTemplate { get; set; }

        #endregion

        #region SorterSwitchableEvalTemplate

        public DataTemplate SorterSwitchableEvalTemplate { get; set; }

        #endregion

        #region SwitchableGenTemplate

        public DataTemplate SwitchableGenTemplate { get; set; }

        #endregion

        #region SorterTuneTemplate

        public DataTemplate SorterTuneTemplate { get; set; }

        #endregion

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tabItem = item as IToolTemplateVm;

            if (tabItem != null)
                switch (tabItem.ToolTemplateType)
                {
                    case ToolTemplateType.RandomSorter:
                        return RandomSorterTemplate;
                    case ToolTemplateType.SorterEval:
                        return SorterEvalTemplate;
                    case ToolTemplateType.SorterGenomeGen:
                        return SorterGenomeGenTemplate;
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
