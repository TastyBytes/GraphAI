namespace GraphAI
{
    public interface IAction
    {
        IState Source { get; }
        IState Result { get; }
    }
}
