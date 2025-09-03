using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiComportement : MonoBehaviour
{
    protected void KillPlayer(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // Récupère le composant Health du joueur
            var health = player.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(20); // exemple : 20 de dégâts
            }
            else
            {
                Debug.LogWarning("Pas de script Health trouvé sur le joueur !");
            }
        }
    }

}
