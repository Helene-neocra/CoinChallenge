using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public Button restartButton;
    public GameObject gameOverPanel;
    
    [Header("Pause Settings")]
    public bool pauseOnGameOver = true; // Option pour activer/désactiver la pause

    private bool gameEnded = false;
    private PlayerController player;
    private Timer timer;
    private float originalTimeScale; // Sauvegarder la timeScale originale

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        timer = FindObjectOfType<Timer>();
        originalTimeScale = Time.timeScale; // Sauvegarder la timeScale au démarrage
    }
    
    private void Start()
    {
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            var health = player.GetComponent<Health>();
            if (health != null)
            {
                health.OnDied += TriggerGameOver;
            }
        }
    }

    void OnEnable() => Timer.OnTimeUp += EndGame;
    void OnDisable() => Timer.OnTimeUp -= EndGame;

    // Permet aux autres scripts de déclencher le Game Over
    public void TriggerGameOver()
    {
        EndGame();
    }

    void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        // Mettre en pause le jeu si l'option est activée
        if (pauseOnGameOver)
        {
            Time.timeScale = 0f;
        }

        player?.ForceStop();
        if (player != null)
            player.enabled = false;

        timer?.StopTimer();
        
        ShowGameOverUI();
    }

    void ShowGameOverUI()
    {
        
        if (restartButton != null)
        {
            Debug.Log("Activating restart button");
            restartButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Restart button is null! Check GameManager inspector.");
        }
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.Log("No game over panel assigned - using button only");
        }
    }
    
    public void RestartGame()
    {
        // Remettre la timeScale normale avant de redémarrer
        Time.timeScale = originalTimeScale;
        ScoreUI.ResetScore();
        
        var sceneManager = FindObjectOfType<GameSceneManager>();
        if (sceneManager != null)
            sceneManager.RestartGame();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
    }
    
    // Méthode publique pour reprendre le jeu (si vous voulez ajouter un bouton "Resume" plus tard)
    public void ResumeGame()
    {
        if (gameEnded) return; // Ne pas reprendre si le jeu est terminé
        Time.timeScale = originalTimeScale;
    }
    
    // Propriété pour vérifier si le jeu est en pause
    public bool IsGamePaused => Time.timeScale == 0f;
}
