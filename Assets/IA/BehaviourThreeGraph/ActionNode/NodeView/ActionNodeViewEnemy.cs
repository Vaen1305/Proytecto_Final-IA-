using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("MyAI/View")]
public class ActionNodeViewEnemy : ActionNodeView
{
     

    public override void OnStart()
    {
        base.OnStart();
    }    public override TaskStatus OnUpdate()
    {
        if (_UnitGame == UnitGame.Ciervo)
        {
            // For Ciervo, look specifically for "Lobo" (wolf) enemies
            GameObject[] wolves = GameObject.FindGameObjectsWithTag("Lobo");
            foreach (GameObject wolf in wolves)
            {
                float distance = Vector3.Distance(transform.position, wolf.transform.position);
                if (distance <= _IACharacterVehiculo.AIEye.mainDataView.Distance)
                {
                    // Check if wolf is within view angle
                    Vector3 directionToWolf = (wolf.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, directionToWolf);
                    
                    if (angle <= _IACharacterVehiculo.AIEye.mainDataView.angle / 2)
                    {
                        // Store the detected wolf as the enemy
                        _IACharacterVehiculo.AIEye.ViewEnemy = wolf.GetComponent<Health>();
                        return TaskStatus.Success;
                    }
                }
            }
            return TaskStatus.Failure;
        }
        
        // Default behavior for other units
        if(_IACharacterVehiculo.AIEye.ViewEnemy==null)
          return TaskStatus.Failure;

        return TaskStatus.Success;
    }


}