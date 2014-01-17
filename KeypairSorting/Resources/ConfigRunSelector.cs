using System.Windows;
using System.Windows.Controls;

namespace KeypairSorting.Resources
{
    public interface IConfigRunSelectorVm
    {
        ConfigRunTemplateType ConfigRunTemplateType { get; }
        string Description { get; }
    }

    public class ConfigRunSelector : DataTemplateSelector
    {
        #region ConfigTemplate

        public DataTemplate ConfigTemplate { get; set; }

        #endregion

        #region RunTemplate

        public DataTemplate RunTemplate { get; set; }

        #endregion

        #region DefaultTemplate

        public DataTemplate DefaultTemplate { get; set; }

        #endregion

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var tabItem = item as IConfigRunSelectorVm;

            if (tabItem != null)
                switch (tabItem.ConfigRunTemplateType)
                {
                    case ConfigRunTemplateType.Config:
                        return ConfigTemplate;
                    case ConfigRunTemplateType.Run:
                        return RunTemplate;
                    default:
                        return DefaultTemplate;
                }
            return DefaultTemplate;
        }
    }

    public enum ConfigRunTemplateType
    {
        Config,
        Run
    }

}
