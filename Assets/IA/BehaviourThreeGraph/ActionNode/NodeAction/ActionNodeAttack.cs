using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("MyAI/Action")]
public class ActionNodeAttack : ActionNodeAction
{
    // --- Unity Methods ---
    public override void OnStart()
    {
        base.OnStart();
    }

    // --- Behavior Tree Logic ---
    public override TaskStatus OnUpdate()
    {
        if (_IACharacterVehiculo.health.IsDead)
            return TaskStatus.Failure;

        SwitchUnit();
        return TaskStatus.Success;
    }

    // --- Custom Logic ---
    void SwitchUnit()
    {
        switch (_UnitGame)
        {
            case UnitGame.Zombie:
                if (_IACharacterActions is IACharacterActionsZombie)
                {
                    ((IACharacterActionsZombie)_IACharacterActions).Attack();
                }
                break;
            case UnitGame.Soldier:
                if (_IACharacterActions is IACharacterActionsSoldier)
                {
                    ((IACharacterActionsSoldier)_IACharacterActions).Attack();
                }
                break;
            case UnitGame.None:
                break;
            default:
                break;
        }
    }
}