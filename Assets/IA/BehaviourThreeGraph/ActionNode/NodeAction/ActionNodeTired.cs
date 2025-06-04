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
        if (_IACharacterVehiculo == null || _IACharacterVehiculo.health == null)
        {
            return TaskStatus.Failure;
        }

        if (_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        if (IsTired())
        {
            return TaskStatus.Success;
        }
        
        return TaskStatus.Failure;
    }

    private bool IsTired()
    {
        if (_UnitGame == UnitGame.Ciervo)
        {
            // Lógica específica para Ciervo
            IACharacterActionsCiervo ciervoActions = _IACharacterActions as IACharacterActionsCiervo;
            if (ciervoActions != null)
            {
                return ciervoActions.IsTired();
            }
        }
          // Lógica general para otros tipos
        return _IACharacterVehiculo.health.health < (_IACharacterVehiculo.health.healthMax * 0.3f);
    }
}
