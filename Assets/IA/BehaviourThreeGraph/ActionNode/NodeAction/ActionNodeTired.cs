using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Action")]
public class ActionNodeTired : ActionNodeAction
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        // Logic to check if the wolf is tired
        if (IsTired())
            return TaskStatus.Success;

        return TaskStatus.Failure;
    }

    private bool IsTired()
    {
        // Replace with actual logic to determine if the wolf is tired
        return _IACharacterVehiculo.health.health < (_IACharacterVehiculo.health.healthMax * 0.5f);
    }
}
