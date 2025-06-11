using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IACharacterVehiculoCiervo : IACharacterVehiculo
{
    [Header("Ciervo Movement Settings")]
    public float runAwaySpeed = 8f;
    public float wanderSpeed = 3f;
    public float avoidSpeed = 5f;
    public float detectionRange = 15f;

    private HealthCiervo _healthCiervo;
    private bool isResting = false;
    private bool isFleeing = false;

    void Start()
    {
        LoadComponent();
        ConfigureCiervoBehavior();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        _healthCiervo = GetComponent<HealthCiervo>();
        
        // Configurar velocidades del NavMeshAgent
        if (agent != null)
        {
            agent.speed = wanderSpeed;
        }
    }

    void ConfigureCiervoBehavior()
    {
        // Configurar parámetros específicos para ciervos
        if (_CalculateDiffuse != null)
        {
            _CalculateDiffuse.DistanceRay = 15f; // Ciervos detectan obstáculos más lejos
            
            // Configurar comportamiento más cauteloso (rotación rápida para evasión)
            _CalculateDiffuse.cerca.Singleton = 5f;
            _CalculateDiffuse.medio.Singleton = 3f;
            _CalculateDiffuse.lejos.Singleton = 1f;
        }
        
        // Ciervos son rápidos y ágiles
        moveSpeed = wanderSpeed;
        rotationSpeed = 4f;
        RangeWander = 25f;
    }

    protected override void Update()
    {
        base.Update();
        
        // Verificar amenazas cercanas
        CheckForThreats();
        
        // Manejar stamina y descanso
        HandleStaminaAndRest();
    }

    // MÉTODO PÚBLICO PARA BEHAVIOR TREE - ESPECÍFICO PARA CIERVO
    public void StartCiervoWandering()
    {
        WanderAround();
    }

    void CheckForThreats()
    {
        if (isResting) return;

        Collider[] threats = Physics.OverlapSphere(transform.position, detectionRange);
        Vector3 closestThreat = Vector3.zero;
        float closestDistance = float.MaxValue;

        foreach (Collider threat in threats)
        {
            if (threat.CompareTag("Player") || threat.CompareTag("Zombie") || threat.CompareTag("Human"))
            {
                float distance = Vector3.Distance(transform.position, threat.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestThreat = threat.transform.position;
                }
            }
        }

        if (closestThreat != Vector3.zero)
        {
            if (closestDistance < 8f)
            {
                RunAwayFrom(closestThreat);
            }
            else if (closestDistance < 12f)
            {
                AvoidPosition(closestThreat);
            }
        }
        else
        {
            // No hay amenazas, comportamiento normal
            isFleeing = false;
            if (agent != null)
            {
                agent.speed = wanderSpeed;
            }
        }
    }

    void HandleStaminaAndRest()
    {
        if (_healthCiervo == null) return;

        // Si la stamina está muy baja, descansar
        if (_healthCiervo.GetStaminaPercentage() < 0.2f && !isResting)
        {
            Rest();
        }
        // Si la stamina se ha recuperado lo suficiente, dejar de descansar
        else if (_healthCiervo.GetStaminaPercentage() > 0.8f && isResting)
        {
            StopResting();
        }
    }

    // Método para deambular (USADO POR BEHAVIOR TREE)
    public void WanderAround()
    {
        if (isResting || isFleeing) return;

        // Generar nueva posición de vagabundeo
        GenerateWanderPosition();
        
        // Consumir stamina mientras deambula
        if (_healthCiervo != null)
        {
            _healthCiervo.ConsumeStamina(2f * Time.deltaTime);
        }
        
        // Ajustar velocidad
        if (agent != null)
        {
            agent.speed = wanderSpeed;
        }
    }

    // Método específico para huir de amenazas
    public void RunAwayFrom(Vector3 threatPosition)
    {
        if (agent != null && !isResting)
        {
            isFleeing = true;
            
            // Calcular dirección opuesta a la amenaza
            Vector3 runDirection = (transform.position - threatPosition).normalized;
            Vector3 runPosition = transform.position + runDirection * 15f;
            
            // Verificar si la posición es válida
            if (NavMesh.SamplePosition(runPosition, out NavMeshHit hit, 15f, 1))
            {
                agent.speed = runAwaySpeed;
                agent.SetDestination(hit.position);
            }
            
            // Consumir stamina rápidamente al huir
            if (_healthCiervo != null)
            {
                _healthCiervo.ConsumeStamina(8f * Time.deltaTime);
            }
        }
    }

    // Método específico para evitar amenazas
    public void AvoidPosition(Vector3 threatPosition)
    {
        if (agent != null && !isResting)
        {
            // Calcular posición de evitación lateral
            Vector3 avoidDirection = Vector3.Cross((threatPosition - transform.position).normalized, Vector3.up);
            Vector3 avoidPosition = transform.position + avoidDirection * 8f;
            
            // Verificar si la posición es válida
            if (NavMesh.SamplePosition(avoidPosition, out NavMeshHit hit, 8f, 1))
            {
                agent.speed = avoidSpeed;
                agent.SetDestination(hit.position);
            }
            
            // Consumir stamina moderadamente al evitar
            if (_healthCiervo != null)
            {
                _healthCiervo.ConsumeStamina(4f * Time.deltaTime);
            }
        }
    }

    // Método para descansar
    public void Rest()
    {
        isResting = true;
        isFleeing = false;
        
        if (agent != null)
        {
            agent.ResetPath();
            agent.speed = 0f;
        }
        
        // Restaurar stamina mientras descansa
        if (_healthCiervo != null)
        {
            _healthCiervo.RestoreStamina(15f * Time.deltaTime);
        }
    }

    // Método para dejar de descansar
    public void StopResting()
    {
        isResting = false;
        
        if (agent != null)
        {
            agent.speed = wanderSpeed;
        }
    }

    // Override del comportamiento de lógica difusa para ciervos
    public override void LookRotationCollider()
    {
        base.LookRotationCollider();
        
        // Comportamiento específico del ciervo con lógica difusa
        if (_CalculateDiffuse != null && _CalculateDiffuse.Collider)
        {
            // Los ciervos son más sensibles a los obstáculos
            float distanceToObstacle = _CalculateDiffuse.hit.distance;
            
            if (distanceToObstacle < 5f && !isResting)
            {
                // Evitar el obstáculo rápidamente
                AvoidPosition(_CalculateDiffuse.hit.point);
            }
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        // Mostrar rango de detección de amenazas
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Mostrar estado actual
        if (isResting)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.5f);
        }
        else if (isFleeing)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.5f);
        }
    }
}