public enum NodeState
{
    SUCCESS,
    FAILURE,
    RUNNING
}

public abstract class Node
{
    public NodeState state;
    public abstract NodeState Evaluate();
}
