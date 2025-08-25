using UnityEngine;

using UnityEngine;

public class Slurp : EnnemiComportement
{
    private void OnTriggerEnter(Collider other)
    {
        // Si le collider des pieds du joueur touche Slurp
        if (other.name == "ColliderPlayer")
        {
            Debug.Log("Slurp détruit par un saut du joueur !");
            gameObject.SetActive(false);
            Destroy(gameObject);
            return; // Important : évite le Game Over
        }

        else
        {
            KillPlayer(other);
        }
    }
    
}