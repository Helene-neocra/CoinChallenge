using UnityEngine;
using UnityEngine.AI;

public class AgentNavMesh : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Assure-toi que l'agent est configuré correctement
        if (agent != null)
        {
            agent.updateRotation = true;
            agent.updateUpAxis = false;
        }
    }

    void Update()
    {
        if (target != null && agent != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        if (agent != null)
        {
            agent.speed = newSpeed;
        }
    }

    public bool HasReachedDestination()
    {
        if (agent == null) return false;
        
        return !agent.pathPending && agent.remainingDistance < 0.5f;
    }
}