using UnityEngine;

public class Turtle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Déclenche le Game Over uniquement si on touche le joueur
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            var gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                Debug.Log("Turtle: collision avec le joueur, déclenche Game Over");
                gm.TriggerGameOver();
            }
            else
            {
                Debug.LogWarning("Turtle: GameManager introuvable dans la scène");
            }
        }
    }
}
