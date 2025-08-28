using UnityEngine;

public class EnnemiZoneBehaviour : MonoBehaviour
{
    [Header("Configuration des zones")]
    public float rayonZoneAttente = 5f;
    public float rayonZoneChasse = 15f;
    
    [Header("Comportement")]
    public float tempsAttenteMin = 2f;
    public float tempsAttenteMax = 5f;
    
    private Vector3 positionInitiale;
    private Transform playerTarget;
    private AgentNavMesh navMesh;
    private EnnemiComportement comportement;
    private float timerAttente;
    private bool estEnModeChasse = false;
    private Vector3 pointAttenteActuel;

    public void Initialize(Vector3 posInit, float rayonAttente, float rayonChasse, Transform player)
    {
        positionInitiale = posInit;
        rayonZoneAttente = rayonAttente;
        rayonZoneChasse = rayonChasse;
        playerTarget = player;
        
        navMesh = GetComponent<AgentNavMesh>();
        comportement = GetComponent<EnnemiComportement>();
        
        // Commence en mode attente
        PasserEnModeAttente();
    }

    void Update()
    {
        if (playerTarget == null || navMesh == null) return;

        float distanceJoueur = Vector3.Distance(transform.position, playerTarget.position);

        // Détermine le mode selon la distance du joueur
        if (distanceJoueur <= rayonZoneChasse && !estEnModeChasse)
        {
            PasserEnModeChasse();
        }
        else if (distanceJoueur > rayonZoneChasse && estEnModeChasse)
        {
            PasserEnModeAttente();
        }

        // Gère le comportement selon le mode
        if (estEnModeChasse)
        {
            GererModeChasse();
        }
        else
        {
            GererModeAttente();
        }
    }

    void PasserEnModeChasse()
    {
        estEnModeChasse = true;
        navMesh.target = playerTarget;
        Debug.Log($"{gameObject.name}: Mode CHASSE activé");
    }

    void PasserEnModeAttente()
    {
        estEnModeChasse = false;
        timerAttente = Random.Range(tempsAttenteMin, tempsAttenteMax);
        ChoisirNouveauPointAttente();
        Debug.Log($"{gameObject.name}: Mode ATTENTE activé");
    }

    void GererModeChasse()
    {
        // En mode chasse, suit directement le joueur
        navMesh.target = playerTarget;
    }

    void GererModeAttente()
    {
        timerAttente -= Time.deltaTime;
        
        // Si arrivé au point d'attente ou timer écoulé
        if (timerAttente <= 0 || Vector3.Distance(transform.position, pointAttenteActuel) < 1f)
        {
            ChoisirNouveauPointAttente();
            timerAttente = Random.Range(tempsAttenteMin, tempsAttenteMax);
        }
    }

    void ChoisirNouveauPointAttente()
    {
        // Choisit un point aléatoire dans la zone d'attente
        Vector2 randomCircle = Random.insideUnitCircle * rayonZoneAttente;
        Vector3 nouveauPoint = positionInitiale + new Vector3(randomCircle.x, 0, randomCircle.y);
        
        // Raycast pour s'assurer que le point est sur le sol
        if (Physics.Raycast(nouveauPoint + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f))
        {
            pointAttenteActuel = hit.point;
        }
        else
        {
            pointAttenteActuel = positionInitiale; // Point de sécurité
        }
        
        // Crée un GameObject temporaire comme target pour le NavMesh
        if (navMesh.target == null || navMesh.target == playerTarget)
        {
            var targetGO = new GameObject($"Target_{gameObject.name}");
            navMesh.target = targetGO.transform;
        }
        
        navMesh.target.position = pointAttenteActuel;
    }

    void OnDrawGizmosSelected()
    {
        // Zone d'attente (vert)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(positionInitiale, rayonZoneAttente);
        
        // Zone de chasse (rouge)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(positionInitiale, rayonZoneChasse);
        
        // Point d'attente actuel
        if (pointAttenteActuel != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pointAttenteActuel, 0.5f);
        }
        
        // Mode actuel
        Gizmos.color = estEnModeChasse ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 2f, Vector3.one);
    }
}
