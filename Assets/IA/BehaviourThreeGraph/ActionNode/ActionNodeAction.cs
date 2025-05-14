using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("MyAI/BaseClass")]
public class ActionNodeAction : ActionNode
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public bool DetectEnemy()
    {
        // Logic to detect an enemy (e.g., wolf)
        return _IACharacterVehiculo.AIEye.ViewEnemy != null;
    }

    public bool IsInTerritory()
    {
        // Logic to check if Jabali is in its territory
        return true; // Replace with actual territory check logic
    }

    public void Attack()
    {
        // Logic to attack the detected enemy
        if (_IACharacterActions is IACharacterActionsZombie)
        {
            ((IACharacterActionsZombie)_IACharacterActions).Attack();
        }
    }

    public bool IsLowHealth()
    {
        // Logic to check if Jabali has low health
        return _IACharacterVehiculo.health.health < (_IACharacterVehiculo.health.healthMax * 0.3f);
    }

    public void RunAway()
    {
        // Logic to run away from the enemy
        _IACharacterVehiculo.MoveToEvadEnemy();
    }

    public bool IsHungry()
    {
        // Logic to check if Jabali is hungry
        return false; // Replace with actual hunger check logic
    }

    public void SearchFood()
    {
        // Logic to search for food
        // Implement food search logic here
    }

    public bool NotViewEnemy()
    {
        // Logic to check if no enemy is in view
        return _IACharacterVehiculo.AIEye.ViewEnemy == null;
    }

    public void Wander()
    {
        // Logic to wander around
        _IACharacterVehiculo.MoveToWander();
    }
}