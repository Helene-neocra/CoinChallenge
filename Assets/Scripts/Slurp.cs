using UnityEngine;

public class Slurp : EnnemiComportement
{
    private Animator animator;
    private bool isDead = false;
    private Transform player;
    
    [SerializeField] private float detectionRadius = 1.5f;
    [SerializeField] private float jumpDetectionHeight = 2f;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }
    
    private void Update()
    {
        if (isDead || player == null) return;
        
        CheckPlayerInteraction();
    }
    
    private void CheckPlayerInteraction()
    {
        Vector3 slurpPosition = transform.position;
        Vector3 playerPosition = player.position;
        
        float distanceToPlayer = Vector3.Distance(slurpPosition, playerPosition);
        
        // Vérifier si le joueur saute sur la tête de Slurp
        if (playerPosition.y > slurpPosition.y + 0.5f && distanceToPlayer <= 1.5f)
        {
            DestroySlurp();
            return;
        }
        
        // Vérifier collision latérale avec le joueur (même niveau Y)
        if (Mathf.Abs(playerPosition.y - slurpPosition.y) < 1f && distanceToPlayer <= 1.2f)
        {
            KillPlayer(player.GetComponent<Collider>());
            Destroy(gameObject);
        }
    }
    
    private void DestroySlurp()
    {
        isDead = true;
        
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDying", true);
        }
        
        Destroy(gameObject, 2f);
    }
}
