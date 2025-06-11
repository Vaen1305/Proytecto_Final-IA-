using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IACharacterVehiculoSoldier : IACharacterVehiculo
{
    [Header("Configuración Soldado")]
    Vector3 normales = Vector3.zero;
    public bool ISDrawGizmos = false;
    public float tacticalDistance = 8f;
    public bool isCautious = true;
    
    [Header("Estrategia Táctica")]
    public float strategyRange = 12f;
    public Vector3[] tacticalPositions;
    
    void Start()
    {
        LoadComponent();
        ConfigureSoldierBehavior();
    }
    
    void ConfigureSoldierBehavior()
    {
        // Configurar parámetros específicos para soldados
        if (_CalculateDiffuse != null)
        {
            _CalculateDiffuse.DistanceRay = 12f; // Soldados detectan obstáculos más lejos
            
            // Configurar comportamiento más táctico (rotación más controlada)
            _CalculateDiffuse.cerca.Singleton = 3f;
            _CalculateDiffuse.medio.Singleton = 1.8f;
            _CalculateDiffuse.lejos.Singleton = 0.5f;
        }
        
        // Soldados son más rápidos y precisos
        moveSpeed = 4f;
        rotationSpeed = 2.5f;
        RangeWander = 20f;
    }

    // MÉTODOS ESPECÍFICOS PARA SOLDADOS
    public void MoveToStrategy()
    {
        if (AIEye != null && AIEye.ViewEnemy != null)
        {
            Vector3 strategicPosition = CalculateStrategicPosition();
            if (strategicPosition != Vector3.zero)
            {
                MoveToPosition(strategicPosition);
            }
        }
        else
        {
            // Si no hay enemigo, patrullar
            StartWandering();
        }
    }

    Vector3 CalculateStrategicPosition()
    {
        if (AIEye.ViewEnemy == null) return Vector3.zero;
        
        Vector3 enemyPosition = AIEye.ViewEnemy.transform.position;
        
        // Calcular posición táctica manteniendo distancia
        Vector3 directionToEnemy = (enemyPosition - transform.position).normalized;
        Vector3 strategicPos = transform.position - directionToEnemy * strategyRange;
        
        // Verificar si la posición es válida
        if (NavMesh.SamplePosition(strategicPos, out NavMeshHit hit, strategyRange, 1))
        {
            return hit.position;
        }
        
        return Vector3.zero;
    }

    public override void LookRotationCollider()
    {
        base.LookRotationCollider();
        
        // Comportamiento táctico del soldado
        if (_CalculateDiffuse != null && _CalculateDiffuse.Collider && isCautious)
        {
            // Los soldados mantienen distancia táctica
            float distanceToObstacle = _CalculateDiffuse.hit.distance;
            
            if (distanceToObstacle < tacticalDistance)
            {
                // Reducir velocidad significativamente cuando están muy cerca
                if (agent != null)
                {
                    agent.speed = moveSpeed * 0.4f;
                }
                
                // Calcular posición táctica alternativa
                Vector3 tacticalPosition = CalculateTacticalPosition();
                if (tacticalPosition != Vector3.zero)
                {
                    MoveToPosition(tacticalPosition);
                }
            }
        }
    }
    
    Vector3 CalculateTacticalPosition()
    {
        // Buscar una posición que mantenga distancia del obstáculo
        Vector3 avoidDirection = _CalculateDiffuse.hit.normal;
        Vector3 tacticalPos = transform.position + avoidDirection * tacticalDistance;
        
        // Verificar si la posición es válida en el NavMesh
        if (NavMesh.SamplePosition(tacticalPos, out NavMeshHit navHit, tacticalDistance, 1))
        {
            return navHit.position;
        }
        
        return Vector3.zero;
    }
    
    protected override void GenerateWanderPosition()
    {
        // Soldados patrullan de manera más sistemática
        base.GenerateWanderPosition();
        
        // Comportamiento adicional: evitar zombies
        Collider[] nearbyThreats = Physics.OverlapSphere(transform.position, RangeWander * 0.3f);
        foreach (Collider threat in nearbyThreats)
        {
            if (threat.CompareTag("Zombie"))
            {
                // Generar posición alejada de la amenaza
                Vector3 awayDirection = (transform.position - threat.transform.position).normalized;
                Vector3 safePosition = transform.position + awayDirection * RangeWander * 0.5f;
                
                if (NavMesh.SamplePosition(safePosition, out NavMeshHit hit, RangeWander, 1))
                {
                    positionWander = hit.position;
                    MoveToPosition(positionWander);
                    break;
                }
            }
        }
    }
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        if (ISDrawGizmos)
        {
            // Mostrar distancia táctica
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, tacticalDistance);
            
            // Mostrar rango estratégico
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, strategyRange);
            
            // Mostrar normales si están disponibles
            if (normales != Vector3.zero)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(transform.position, normales * 3f);
            }
        }
    }
}