using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject coinPrefab;
    public int coinCount = 50;
    public float minX = 0f;
    public float maxX = 100f;
    public float minZ = 0f;
    public float maxZ = 100f;
    [Header("Game Over UI")]
    public Button restartButton; // Référence au bouton Rejouer
    public GameObject gameOverPanel; // Panel contenant l'UI de fin de partie (optionnel)
    
    private bool gameEnded = false;

    void OnEnable()
    {
        Timer.OnTimeUp += EndGame;
    }

    void OnDisable()
    {
        Timer.OnTimeUp -= EndGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Génère simplement les coins dans une zone fixe
        CoinController.SpawnRandomCoins(coinCount, coinPrefab, minX, maxX, minZ, maxZ);
    }

    // Update is called once per frame
    void Update()
    {
        // Vérification si le jeu est terminé
        if (gameEnded)
        {
            // Désactive les contrôles du joueur et force l'arrêt
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                // Force l'arrêt du mouvement
                player.ForceStop();
                // Désactive le script après avoir forcé l'arrêt
                player.enabled = false;
            }
        }
    }

    void EndGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            Debug.Log($"Temps écoulé ! Score final : {CoinController.score}");

            // Arrête le timer
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.StopTimer();
            }

            // Ici on pourra ajouter le changement de scène plus tard
            ShowGameOverUI();
        }
    }

    void ShowGameOverUI()
    {
        Debug.Log("ShowGameOverUI called!");
        
        // Affiche le bouton Rejouer
        if (restartButton != null)
        {
            Debug.Log("Activating restart button");
            restartButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Restart button is null! Check GameManager inspector.");
        }
        
        // Panel optionnel (seulement si assigné)
        if (gameOverPanel != null)
        {
            Debug.Log("Activating game over panel");
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.Log("No game over panel assigned - using button only");
        }
        
        Debug.Log($"Game Over! Score final : {CoinController.score}");
    }
    
    // Méthode appelée par le bouton Rejouer
    public void RestartGame()
    {
        GameSceneManager sceneManager = FindObjectOfType<GameSceneManager>();
        if (sceneManager != null)
        {
            sceneManager.RestartGame();
        }
        else
        {
            // Fallback si pas de GameSceneManager
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
