using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMeshController : MonoBehaviour
{
    private NavMeshAgent agent;
    public float moveDistance = 5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            moveDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A))
            moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            moveDirection += Vector3.right;

        if (moveDirection != Vector3.zero)
        {
            Vector3 destination = transform.position + transform.TransformDirection(moveDirection.normalized) * moveDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(destination, out hit, 1.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }
}