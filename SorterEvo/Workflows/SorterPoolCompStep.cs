namespace SorterEvo.Workflows
{
    public enum SorterPoolCompState
    {
        TranscribeSwitchableGenome,
        TranscribeSorterGenome,
        RunCompetition,
        UpdateSwitchableGenome,
        UpdateSorterGenome
    }

    public interface ISorterPoolCompStep
    {
        SorterPoolCompState SorterPoolCompState { get; }
    }

    public class SorterPoolCompStep
    {
        
    }
}