using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Move")]
public class ActionWander : ActionNodeVehicle
{
    public override void OnStart()
    {
        base.OnStart();
    }
    
    public override TaskStatus OnUpdate()
    {
        if(_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        // Ejecutar movimiento de deambular
        if (_UnitGame == UnitGame.Ciervo)
        {
            // Lógica específica para Ciervo
            IACharacterVehiculoCiervo ciervoVehicle = _IACharacterVehiculo as IACharacterVehiculoCiervo;
            if (ciervoVehicle != null)
            {
                ciervoVehicle.StartCiervoWandering(); // Método específico para ciervo
            }
        }
        else
        {
            // Lógica general para otros tipos
            _IACharacterVehiculo.StartWandering(); // Método público agregado
        }
        
        return TaskStatus.Running; // Continúa ejecutándose
    }
}