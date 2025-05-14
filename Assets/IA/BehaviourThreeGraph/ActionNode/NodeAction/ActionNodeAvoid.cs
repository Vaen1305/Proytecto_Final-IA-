using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Action")]
public class ActionNodeAvoid : ActionNodeAction
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        Avoid();
        return TaskStatus.Success;
    }

    private void Avoid()
    {
        // Logic to avoid Jabali
        _IACharacterVehiculo.MoveToEvadEnemy();
    }
}
