using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiComportement : MonoBehaviour
{
    protected void KillPlayer(Collider other)
    {
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
