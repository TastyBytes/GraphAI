namespace GraphAI
{
    public interface IAction
    {
        IState OriginalState { get; }
        IState Result { get; }
    }
}
