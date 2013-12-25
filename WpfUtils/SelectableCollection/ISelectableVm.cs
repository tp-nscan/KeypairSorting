using System.ComponentModel;

namespace WpfUtils.SelectableCollection
{
    public interface ISelectableVm : INotifyPropertyChanged
    {
        bool IsSelected { get; set; }
    }
}
