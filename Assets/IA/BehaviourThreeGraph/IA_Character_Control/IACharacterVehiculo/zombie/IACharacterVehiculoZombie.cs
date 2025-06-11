using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IACharacterVehiculoZombie : IACharacterVehiculo
{
    [Header("Configuración Zombie")]
    public float aggressionLevel = 1.5f;
    public bool isAggressive = true;
    
    void Start()
    {
        LoadComponent();
        ConfigureZombieBehavior();
    }
    
    void ConfigureZombieBehavior()
    {
        // Configurar parámetros específicos para zombies
        if (_CalculateDiffuse != null)
        {
            _CalculateDiffuse.DistanceRay = 6f; // Zombies detectan obstáculos más cerca
            
            // Configurar comportamiento más agresivo (rotación más rápida)
            _CalculateDiffuse.cerca.Singleton = 5f * aggressionLevel;
            _CalculateDiffuse.medio.Singleton = 3f * aggressionLevel;
            _CalculateDiffuse.lejos.Singleton = 1f * aggressionLevel;
        }
        
        // Zombies son más lentos pero persistentes
        moveSpeed = 2f;
        rotationSpeed = 3f;
        RangeWander = 15f;
    }

    public override void LookRotationCollider()
    {
        base.LookRotationCollider();
        
        // Comportamiento específico de zombie con lógica difusa
        if (_CalculateDiffuse != null && _CalculateDiffuse.Collider && isAggressive)
        {
            // Los zombies son más persistentes, continúan moviéndose aunque detecten obstáculos
            if (agent != null)
            {
                float aggressiveSpeed = moveSpeed * 0.8f; // No reducen tanto la velocidad
                agent.speed = Mathf.Max(agent.speed, aggressiveSpeed);
            }
        }
    }
    
    protected override void GenerateWanderPosition()
    {
        // Zombies tienden a moverse hacia áreas con más actividad
        base.GenerateWanderPosition();
        
        // Comportamiento adicional: buscar targets cercanos
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, RangeWander * 0.5f);
        foreach (Collider target in nearbyTargets)
        {
            if (target.CompareTag("Player"))
            {
                positionWander = target.transform.position;
                MoveToPosition(positionWander);
                break;
            }
        }
    }
}