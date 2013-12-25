using System.Collections.ObjectModel;
using System.Linq;

namespace WpfUtils.SelectableCollection
{
    public class ObservableSelectableCollection<T> : ViewModelBase where T : class, ISelectableVm
    {
        readonly ObservableCollection<T> _collection = new ObservableCollection<T>();
        public ObservableCollection<T> Items
        {
            get { return _collection; }
        }

        public T SelectedItem
        {
            get { return _collection.Single(T => T.IsSelected); }
            set
            {
                foreach (var item in _collection)
                {
                    item.IsSelected = item.Equals(value);
                }
            }
        }
    }
}
