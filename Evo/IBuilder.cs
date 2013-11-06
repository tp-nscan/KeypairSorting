namespace Evo
{
    public interface IBuilder<out T>
    {
        T Buld();
    }
}