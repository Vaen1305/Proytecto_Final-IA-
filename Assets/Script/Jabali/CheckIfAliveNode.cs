public class CheckIfAliveNode : Node
{
    private JabaliHealth health;

    public CheckIfAliveNode(JabaliHealth health)
    {
        this.health = health;
    }

    public override NodeState Evaluate()
    {
        return health.IsAlive() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
