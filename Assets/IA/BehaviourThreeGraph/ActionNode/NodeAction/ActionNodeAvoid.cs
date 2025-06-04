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
        if (_IACharacterVehiculo == null || _IACharacterVehiculo.health == null)
        {
            return TaskStatus.Failure;
        }

        if (_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        ExecuteAvoid();
        return TaskStatus.Running; // Usar Running para que continue evitando
    }

    private void ExecuteAvoid()
    {
        // Verificar si es un Ciervo y usar su método específico
        if (_UnitGame == UnitGame.Ciervo && _IACharacterActions is IACharacterActionsCiervo)
        {
            ((IACharacterActionsCiervo)_IACharacterActions).Avoid();
        }
        else
        {
            // Lógica genérica de evitar
            if (_IACharacterVehiculo.AIEye.ViewEnemy != null)
            {
                _IACharacterVehiculo.MoveToEvadEnemy();
            }
        }
        
        Debug.Log("Ciervo evitando amenaza");
    }
}
