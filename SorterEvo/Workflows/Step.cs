namespace SorterEvo.Workflows
{
    public interface IStep
    {
        bool CanContinue { get; }
        void Step();
    }
}