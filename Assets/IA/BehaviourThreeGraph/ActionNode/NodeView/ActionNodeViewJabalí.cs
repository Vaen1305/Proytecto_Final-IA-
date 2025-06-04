using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/View")]
public class ActionNodeViewJabalí : ActionNodeView
{
    [Header("Jabalí Detection")]
    public string jabaliTag = "Jabalí"; // Tag que debe tener el Jabalí

    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (_UnitGame == UnitGame.Ciervo)
        {
            // Lógica específica para Ciervo detectando Jabalí
            var eye = _IACharacterVehiculo.AIEye;
            // Forzar el cast usando reflection para evitar ambigüedad
            var method = eye?.GetType().GetMethod("IsJabaliInSight");
            if (method != null && (bool)method.Invoke(eye, null))
            {
                return TaskStatus.Success;
            }
        }
        else
        {
            // Lógica general para otros tipos de unidades
            if (_IACharacterVehiculo.AIEye.ViewEnemy != null && 
                _IACharacterVehiculo.AIEye.ViewEnemy.gameObject.CompareTag(jabaliTag))
            {
                return TaskStatus.Success;
            }
        }
        
        return TaskStatus.Failure;
    }
}