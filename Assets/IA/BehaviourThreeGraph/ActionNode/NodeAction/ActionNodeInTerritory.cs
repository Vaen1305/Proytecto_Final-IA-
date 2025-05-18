using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Action")]
public class ActionNodeInTerritory : ActionNodeAction
{
    // --- Unity Methods ---
    public override void OnStart()
    {
        base.OnStart();
    }

    // --- Behavior Tree Logic ---
    public override TaskStatus OnUpdate()
    {
        if (_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        if (IsInTerritory())
            return TaskStatus.Success;

        return TaskStatus.Failure;
    }
}
