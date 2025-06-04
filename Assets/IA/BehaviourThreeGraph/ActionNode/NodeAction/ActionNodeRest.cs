using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Action")]
public class ActionNodeRest : ActionNodeAction
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

        ExecuteRest();
        
        // Verificar si ya no está cansado para terminar el descanso
        if (!IsTired())
        {
            Debug.Log("Ciervo ha terminado de descansar");
            return TaskStatus.Success;
        }
        
        return TaskStatus.Running; // Continuar descansando
    }

    private void ExecuteRest()
    {
        // Verificar si es un Ciervo y usar su método específico
        if (_UnitGame == UnitGame.Ciervo && _IACharacterActions is IACharacterActionsCiervo)
        {
            ((IACharacterActionsCiervo)_IACharacterActions).Rest();
        }
        else
        {
            // Lógica genérica de descanso (detener movimiento)
            if (_IACharacterVehiculo.agent != null && _IACharacterVehiculo.agent.hasPath)
            {
                _IACharacterVehiculo.agent.ResetPath();
            }
        }
        
        Debug.Log("Ciervo descansando...");
    }

    private bool IsTired()
    {
        // Verificar si es un Ciervo y usar su método específico
        if (_UnitGame == UnitGame.Ciervo && _IACharacterActions is IACharacterActionsCiervo)
        {
            return ((IACharacterActionsCiervo)_IACharacterActions).IsTired();
        }
        
        return false; // Si no es ciervo, no se considera cansado
    }
}
