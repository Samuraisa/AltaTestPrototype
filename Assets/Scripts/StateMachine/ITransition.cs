namespace Alta
{
    public interface ITransition
    {
        IState ToState { get; }
        IPredicate Condition { get; }
    }
}