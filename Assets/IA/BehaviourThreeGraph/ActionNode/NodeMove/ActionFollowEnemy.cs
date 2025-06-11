using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Move")]
public class ActionFollowEnemy : ActionNodeVehicle
{
    public override void OnStart()
    {
        base.OnStart();
    }
    
    public override TaskStatus OnUpdate()
    {
        if(_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        // Verificar si hay enemigo en vista
        if (_IACharacterVehiculo.AIEye.ViewEnemy == null)
            return TaskStatus.Failure;

        // Ejecutar seguimiento basado en tipo de unidad
        if (_UnitGame == UnitGame.Zombie)
        {
            IACharacterVehiculoZombie zombieVehicle = _IACharacterVehiculo as IACharacterVehiculoZombie;
            if (zombieVehicle != null)
            {
                zombieVehicle.MoveToEnemy(); // Método heredado de la clase base
                zombieVehicle.LookEnemy();   // Método heredado de la clase base
            }
        }
        else if (_UnitGame == UnitGame.Soldier)
        {
            IACharacterVehiculoSoldier soldierVehicle = _IACharacterVehiculo as IACharacterVehiculoSoldier;
            if (soldierVehicle != null)
            {
                soldierVehicle.MoveToEnemy(); // Método heredado de la clase base
                soldierVehicle.LookEnemy();   // Método heredado de la clase base
            }
        }
        else
        {
            // Comportamiento general
            _IACharacterVehiculo.MoveToEnemy();
            _IACharacterVehiculo.LookEnemy();
        }
        
        return TaskStatus.Running;
    }
}