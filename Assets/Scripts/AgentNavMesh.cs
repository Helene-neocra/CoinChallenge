using UnityEngine;
using UnityEngine.AI;

public class AgentNavMesh : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    [Header("Patrouille")]
    public bool enPatrouille = true;
    public Vector3 centreZoneAttente;
    public float rayonZoneAttente = 10f;
    public float tempsAttente = 1.5f;

    private float timerPatrouille = 0f;
    private bool enAttente = false;
    private Vector3 destinationPatrouille;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Assure-toi que l'agent est configuré correctement
        if (agent != null)
        {
            agent.updateRotation = true;
            agent.updateUpAxis = false;
        }

        if (enPatrouille)
        {
            centreZoneAttente = transform.position;
            ChoisirNouveauPointPatrouille();
        }
        // Ajout automatique du collider de détection (zone d'attente)
        SphereCollider trigger = GetComponent<SphereCollider>();
        if (trigger == null)
            trigger = gameObject.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = rayonZoneAttente;
    }

    void Update()
    {
        if (enPatrouille)
        {
            if (enAttente)
            {
                timerPatrouille += Time.deltaTime;
                if (timerPatrouille >= tempsAttente)
                {
                    enAttente = false;
                    ChoisirNouveauPointPatrouille();
                }
            }
            else if (agent != null && !agent.pathPending && agent.remainingDistance < 0.5f)
            {
                enAttente = true;
                timerPatrouille = 0f;
            }
        }
        else
        {
            if (target != null && agent != null)
            {
                agent.SetDestination(target.position);
            }
        }
    }

    void ChoisirNouveauPointPatrouille()
    {
        Vector2 randomCircle = Random.insideUnitCircle * rayonZoneAttente;
        destinationPatrouille = centreZoneAttente + new Vector3(randomCircle.x, 0, randomCircle.y);
        if (agent != null)
            agent.SetDestination(destinationPatrouille);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            enPatrouille = false; // Passe en mode chasse
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            enPatrouille = true; // Retour en patrouille
            ChoisirNouveauPointPatrouille();
        }
    }
}