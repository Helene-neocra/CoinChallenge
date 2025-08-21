using UnityEngine;

using UnityEngine;

public class Slurp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Si le collider des pieds du joueur touche Slurp
        if (other.name == "ColliderPlayer")
        {
            Debug.Log("Slurp détruit par un saut du joueur !");
            Destroy(gameObject);
            return; // Important : évite le Game Over
        }

        // Si le joueur (corps principal) touche Slurp
        if (other.GetComponent<PlayerController>() != null)
        {
            var gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                Debug.Log("Slurp: collision avec le joueur, déclenche Game Over");
                gm.TriggerGameOver();
            }
            else
            {
                Debug.LogWarning("Slurp: GameManager introuvable dans la scène");
            }
        }
    }
}