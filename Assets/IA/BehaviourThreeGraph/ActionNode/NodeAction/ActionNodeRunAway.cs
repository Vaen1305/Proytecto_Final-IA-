using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Action")]
public class ActionNodeRunAway : ActionNodeAction
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        RunAway();
        return TaskStatus.Success;
    }
}
