using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Action")]
public class ActionNodeSleep : ActionNodeAction
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        Sleep();
        return TaskStatus.Success;
    }

    private void Sleep()
    {
        // Logic to make the wolf sleep (e.g., restore health or energy)
        _IACharacterVehiculo.health.health += 10; // Example: restore health
        if (_IACharacterVehiculo.health.health > _IACharacterVehiculo.health.healthMax)
            _IACharacterVehiculo.health.health = _IACharacterVehiculo.health.healthMax;
    }
}
