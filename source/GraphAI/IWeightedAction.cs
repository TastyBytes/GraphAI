namespace GraphAI
{
    public interface IWeightedAction : IAction
    {
        int Cost { get; }
    }
}
