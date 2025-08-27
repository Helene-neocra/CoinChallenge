using UnityEngine;

public class Slurp : EnnemiComportement
{
    private Animator animator;
    private bool isDead = false;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return; // Bloquer toute interaction si déjà mort
        
        // Si c'est le collider des pieds du joueur = destruction de Slurp
        if (other.name == "ColliderPlayer")
        {
            Debug.Log("Slurp détruit par un saut du joueur !");
            DestroySlurp();
            return;
        }
        
        // Si c'est le joueur principal = Game Over
        if (other.CompareTag("Player"))
        {
            KillPlayer(other);
        }
    }
    
    private void DestroySlurp()
    {
        isDead = true;
        
        // Désactiver immédiatement le collider pour éviter d'autres triggers
       //GetComponent<Collider>().enabled = false;
        
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsDying", true);
        }
        
        // Détruire après l'animation
        Destroy(gameObject, 3f);
    }
}
