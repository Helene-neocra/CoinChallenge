using UnityEngine;

public class Slurp : EnnemiComportement
{
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        
        // Commencer en mode marche
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Si le collider des pieds du joueur touche Slurp
        if (other.name == "ColliderPlayer")
        {
            Debug.Log("Slurp détruit par un saut du joueur !");
            
            // Déclencher l'animation de mort
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsDying", true);
            }
            
            // Désactiver après un court délai pour permettre l'animation
            StartCoroutine(DestroyAfterAnimation());
            return; // Important : évite le Game Over
        }

        else
        {
            KillPlayer(other);
        }
    }
    
    private System.Collections.IEnumerator DestroyAfterAnimation()
    {
        // Attendre un peu pour permettre l'animation de mort
        yield return new WaitForSeconds(1f);
        
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}