using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si c'est bien le Player qui entre
        if (other.CompareTag("Player"))
        {
            WinGame();
        }
    }

    void WinGame()
    {
        // Trouve le GameSceneManager et charge la scène Winner
        GameSceneManager sceneManager = FindObjectOfType<GameSceneManager>();
        if (sceneManager != null)
        {
            sceneManager.GoToWinner();
        }
        else
        {
            Debug.LogError("GameSceneManager introuvable ! Impossible de charger la scène Winner.");
        }
    }
}
