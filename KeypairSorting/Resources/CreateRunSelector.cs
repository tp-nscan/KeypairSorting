using System.Windows;
using System.Windows.Controls;

namespace KeypairSorting.Resources
{
    public interface ICreateRunSelectorVm
    {
        CreateRunTemplateType CreateRunTemplateType { get; }
        string Description { get; }
    }

    public class CreateRunSelector : DataTemplateSelector
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
            var tabItem = item as ICreateRunSelectorVm;

            if (tabItem != null)
                switch (tabItem.CreateRunTemplateType)
                {
                    case CreateRunTemplateType.Create:
                        return ConfigTemplate;
                    case CreateRunTemplateType.Run:
                        return RunTemplate;
                    default:
                        return DefaultTemplate;
                }
            return DefaultTemplate;
        }
    }

    public enum CreateRunTemplateType
    {
        Create,
        Run
    }

}
