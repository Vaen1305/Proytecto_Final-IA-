using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Move")]
public class ActionMoveStrategy : ActionNodeVehicle
{
    public override void OnStart()
    {
        base.OnStart();
    }
    
    public override TaskStatus OnUpdate()
    {
        if(_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        // Solo soldados tienen estrategia táctica
        if (_UnitGame == UnitGame.Soldier)
        {
            IACharacterVehiculoSoldier soldierVehicle = _IACharacterVehiculo as IACharacterVehiculoSoldier;
            if (soldierVehicle != null)
            {
                soldierVehicle.MoveToStrategy(); // Método específico del soldado
                soldierVehicle.LookEnemy();      // Método heredado de la clase base
            }
        }
        else
        {
            // Para otros tipos, usar movimiento normal hacia el enemigo
            _IACharacterVehiculo.MoveToEnemy();
            _IACharacterVehiculo.LookEnemy();
        }
        
        return TaskStatus.Running;
    }
}