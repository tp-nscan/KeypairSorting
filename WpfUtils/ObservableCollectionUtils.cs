using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WpfUtils
{
    public static class ObservableCollectionUtils
    {
        public static void Replace<T>(this ObservableCollection<T> collection, T target, T replacement)
        {
            var dex = collection.IndexOf(target);
            if(dex >= 0)
            {
                collection.RemoveAt(dex);
                collection.Insert(dex, replacement);
            }
        }

        public static void AddMany<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static T SelectedItem<T>(this ObservableCollection<T> collection)
        {
            var collectionView = CollectionViewSource.GetDefaultView(collection);
            return (T) (collectionView != null ? collectionView.CurrentItem : default(T));
        }

        public static void MoveCurrentTo<T>(this ObservableCollection<T> collection, T item)
        {
            if (! collection.Contains(item))
            {
                return;
            }
            var collectionView = CollectionViewSource.GetDefaultView(collection);
            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(item);
            }
        }
    }
}
