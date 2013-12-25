namespace WpfUtils.SelectableCollection.TestData
{
    public class ObservableSelectableNames : ObservableSelectableCollection<SelectableNameVm>
    {
        public ObservableSelectableNames()
        {
            Items.Add(new SelectableNameVm { Name = "Ralph"});
            Items.Add(new SelectableNameVm { Name = "Ritchie" });
            Items.Add(new SelectableNameVm { Name = "Joanie" });
            Items.Add(new SelectableNameVm { Name = "Fonz" });
        }
    }
}
