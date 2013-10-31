namespace Sorting.Switchables
{
    public interface ISwitchable
    {
        int KeyCount { get; }
        SwitchableDataType SwitchableType { get; }
    }

    public interface ISwitchable<T> : ISwitchable
    {
        T Item { get; }
    }
}
