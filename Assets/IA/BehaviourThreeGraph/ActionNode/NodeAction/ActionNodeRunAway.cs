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
        if (_IACharacterVehiculo == null || _IACharacterVehiculo.health == null)
        {
            return TaskStatus.Failure;
        }

        if (_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        ExecuteRunAway();
        return TaskStatus.Running; // Usar Running para que continue huyendo
    }

    private void ExecuteRunAway()
    {
        // Verificar si es un Ciervo y usar su método específico
        if (_UnitGame == UnitGame.Ciervo && _IACharacterActions is IACharacterActionsCiervo)
        {
            ((IACharacterActionsCiervo)_IACharacterActions).RunAway();
        }
        else
        {
            // Usar el método base para otros tipos
            RunAway();
        }
        
        Debug.Log("Ejecutando RunAway");
    }
}
