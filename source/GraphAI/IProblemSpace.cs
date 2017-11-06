namespace GraphAI
{
    public interface IProblemSpace
    {
        IAction[] Extend( IState state );
    }
}
