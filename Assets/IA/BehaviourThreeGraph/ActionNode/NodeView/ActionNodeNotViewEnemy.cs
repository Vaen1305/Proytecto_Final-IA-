using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("MyAI/View")]
public class ActionNodeNotViewEnemy : ActionNodeView
{
     

    public override void OnStart()
    {
        base.OnStart();
    }    public override TaskStatus OnUpdate()
    {
        if (_UnitGame == UnitGame.Ciervo)
        {
            // For Ciervo, check if no "Lobo" (wolf) or "Jabalí" (boar) enemies are in sight
            GameObject[] wolves = GameObject.FindGameObjectsWithTag("Lobo");
            GameObject[] boars = GameObject.FindGameObjectsWithTag("Jabalí");
            
            // Check for wolves
            foreach (GameObject wolf in wolves)
            {
                float distance = Vector3.Distance(transform.position, wolf.transform.position);
                if (distance <= _IACharacterVehiculo.AIEye.mainDataView.Distance)
                {
                    Vector3 directionToWolf = (wolf.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, directionToWolf);
                    
                    if (angle <= _IACharacterVehiculo.AIEye.mainDataView.angle / 2)
                    {
                        return TaskStatus.Failure; // Enemy spotted
                    }
                }
            }
            
            // Check for boars
            foreach (GameObject boar in boars)
            {
                float distance = Vector3.Distance(transform.position, boar.transform.position);
                if (distance <= _IACharacterVehiculo.AIEye.mainDataView.Distance)
                {
                    Vector3 directionToBoar = (boar.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, directionToBoar);
                    
                    if (angle <= _IACharacterVehiculo.AIEye.mainDataView.angle / 2)
                    {
                        return TaskStatus.Failure; // Enemy spotted
                    }
                }
            }
            
            // Clear any previously detected enemy
            _IACharacterVehiculo.AIEye.ViewEnemy = null;
            return TaskStatus.Success; // No enemies in sight
        }
        
        // Default behavior for other units
        if(_IACharacterVehiculo.AIEye.ViewEnemy==null)
          return TaskStatus.Success;

        return TaskStatus.Failure;
    }


}