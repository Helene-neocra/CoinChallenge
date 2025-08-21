using UnityEngine;
using UnityEngine.AI;

public class AgentNavMesh : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}