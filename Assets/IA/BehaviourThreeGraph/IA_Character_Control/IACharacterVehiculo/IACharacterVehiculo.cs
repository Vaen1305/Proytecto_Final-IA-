using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IACharacterVehiculo : IACharacterControl
{
    [Header("Lógica Difusa")]
    protected CalculateDiffuse _CalculateDiffuse;
    protected float speedRotation = 0;
    
    [Header("Navegación")]
    public float RangeWander = 10f;
    public float moveSpeed = 3.5f;
    public float rotationSpeed = 2f;
    
    protected Vector3 positionWander;
    protected float FrameRate = 0;
    protected float Rate = 4;
    protected NavMeshAgent agent;

    public override void LoadComponent()
    {
        base.LoadComponent();
        
        // Obtener o agregar el componente de lógica difusa
        _CalculateDiffuse = GetComponent<CalculateDiffuse>();
        if (_CalculateDiffuse == null)
            _CalculateDiffuse = gameObject.AddComponent<CalculateDiffuse>();
            
        // Obtener NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            agent = gameObject.AddComponent<NavMeshAgent>();
            
        // Configurar NavMeshAgent
        agent.speed = moveSpeed;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
    }

    protected virtual void Update()
    {
        if (_CalculateDiffuse != null)
        {
            LookRotationCollider();
        }
        
        UpdateWander();
    }

    // MÉTODOS PÚBLICOS PARA BEHAVIOR TREE
    public virtual void StartWandering()
    {
        GenerateWanderPosition();
    }

    public virtual void MoveToEnemy()
    {
        if (AIEye != null && AIEye.ViewEnemy != null)
        {
            MoveToPosition(AIEye.ViewEnemy.transform.position);
        }
    }

    public virtual void MoveToEvadEnemy()
    {
        if (AIEye != null && AIEye.ViewEnemy != null)
        {
            // Calcular dirección opuesta al enemigo
            Vector3 enemyPosition = AIEye.ViewEnemy.transform.position;
            Vector3 evadeDirection = (transform.position - enemyPosition).normalized;
            Vector3 evadePosition = transform.position + evadeDirection * RangeWander;
            
            // Verificar si la posición es válida en NavMesh
            if (NavMesh.SamplePosition(evadePosition, out NavMeshHit hit, RangeWander, 1))
            {
                MoveToPosition(hit.position);
            }
        }
    }

    public virtual void LookEnemy()
    {
        if (AIEye != null && AIEye.ViewEnemy != null)
        {
            LookPosition(AIEye.ViewEnemy.transform.position);
        }
    }

    public virtual void LookRotationCollider()
    {
        if (_CalculateDiffuse.Collider)
        {
            // Usar la velocidad calculada por lógica difusa
            speedRotation = _CalculateDiffuse.speedRotation;
            
            // Calcular dirección de evasión basada en la normal del hit
            Vector3 avoidDirection = _CalculateDiffuse.hit.normal;
            Vector3 targetPosition = _CalculateDiffuse.hit.point + avoidDirection * 3f;
            
            // Aplicar rotación suave basada en lógica difusa
            LookPosition(targetPosition);
            
            // Reducir velocidad del agente cuando hay obstáculos cerca
            if (agent != null)
            {
                float speedMultiplier = Mathf.Lerp(0.3f, 1f, _CalculateDiffuse.hit.distance / _CalculateDiffuse.DistanceRay);
                agent.speed = moveSpeed * speedMultiplier;
            }
        }
        else
        {
            // Restaurar velocidad normal cuando no hay obstáculos
            if (agent != null)
                agent.speed = moveSpeed;
        }
    }

    public virtual void LookPosition(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Mantener rotación solo en el eje Y
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // Usar speedRotation de la lógica difusa para controlar la velocidad de rotación
            float rotSpeed = speedRotation * rotationSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                                                rotSpeed * Time.deltaTime);
        }
    }

    public virtual void MoveToPosition(Vector3 pos)
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(pos);
        }
    }
    
    protected virtual void UpdateWander()
    {
        FrameRate += Time.deltaTime;
        if (FrameRate >= Rate)
        {
            FrameRate = 0;
            
            // Solo generar nueva posición si no estamos evitando obstáculos
            if (!_CalculateDiffuse.Collider)
            {
                GenerateWanderPosition();
            }
        }
    }
    
    protected virtual void GenerateWanderPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * RangeWander;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y; // Mantener la altura
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, RangeWander, 1))
        {
            positionWander = hit.position;
            MoveToPosition(positionWander);
        }
    }
    
    protected virtual void OnDrawGizmos()
    {
        // Mostrar rango de vagabundeo
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, RangeWander);
        
        // Mostrar destino actual
        if (positionWander != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(positionWander, 0.5f);
            Gizmos.DrawLine(transform.position, positionWander);
        }
    }
}