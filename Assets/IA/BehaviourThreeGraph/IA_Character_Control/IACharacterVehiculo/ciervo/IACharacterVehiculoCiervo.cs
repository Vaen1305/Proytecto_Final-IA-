using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IACharacterVehiculoCiervo : IACharacterVehiculo
{
    [Header("Ciervo Movement Settings")]
    public float runAwaySpeed = 8f;
    public float wanderSpeed = 3f;
    public float avoidSpeed = 5f;

    private HealthCiervo _healthCiervo;

    void Start()
    {
        LoadComponent();
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

    // Override del método de deambular para incluir consumo de stamina
    public override void MoveToWander()
    {
        base.MoveToWander();
        
        // Consumir stamina mientras deambula
        if (_healthCiervo != null)
        {
            _healthCiervo.ConsumeStamina(2f * Time.deltaTime); // 2 puntos por segundo
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
        if (agent != null)
        {
            // Calcular dirección opuesta a la amenaza
            Vector3 runDirection = (transform.position - threatPosition).normalized;
            Vector3 runPosition = transform.position + runDirection * 10f; // Huir 10 unidades
            
            agent.speed = runAwaySpeed;
            agent.SetDestination(runPosition);
            
            // Consumir stamina rápidamente al huir
            if (_healthCiervo != null)
            {
                _healthCiervo.ConsumeStamina(8f * Time.deltaTime); // 8 puntos por segundo
            }
        }
    }

    // Método específico para evitar amenazas (menos drástico que huir)
    public void AvoidPosition(Vector3 threatPosition)
    {
        if (agent != null)
        {
            // Calcular posición de evitación lateral
            Vector3 avoidDirection = Vector3.Cross((threatPosition - transform.position).normalized, Vector3.up);
            Vector3 avoidPosition = transform.position + avoidDirection * 5f; // Evitar 5 unidades
            
            agent.speed = avoidSpeed;
            agent.SetDestination(avoidPosition);
            
            // Consumir stamina moderadamente al evitar
            if (_healthCiervo != null)
            {
                _healthCiervo.ConsumeStamina(4f * Time.deltaTime); // 4 puntos por segundo
            }
        }
    }

    // Método para descansar
    public void Rest()
    {
        if (agent != null)
        {
            agent.ResetPath(); // Detener movimiento
            agent.speed = 0f;
        }
        
        // Restaurar stamina mientras descansa
        if (_healthCiervo != null)
        {
            _healthCiervo.RestoreStamina(15f * Time.deltaTime); // 15 puntos por segundo
        }
    }

    // Override para cuando se mueve hacia el enemigo (no aplicable para Ciervo, pero por compatibilidad)
    public override void MoveToEnemy()
    {
        // El Ciervo no se mueve hacia enemigos, en su lugar huye
        if (AIEye != null && AIEye.ViewEnemy != null)
        {
            RunAwayFrom(AIEye.ViewEnemy.transform.position);
        }
    }
}