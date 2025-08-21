using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public Button restartButton;
    public GameObject gameOverPanel;
    
    private bool gameEnded = false;
    private PlayerController player;
    private Timer timer;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        timer = FindObjectOfType<Timer>();
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

        Debug.Log($"Temps écoulé ! Score final : {ScoreUI.score}");

        player?.ForceStop();
        if (player != null)
            player.enabled = false;

        timer?.StopTimer();
        ShowGameOverUI();
    }

    void ShowGameOverUI()
    {
        Debug.Log("ShowGameOverUI called!");
        
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
            Debug.Log("Activating game over panel");
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.Log("No game over panel assigned - using button only");
        }
        
        Debug.Log($"Game Over! Score final : {ScoreUI.score}");
    }
    
    public void RestartGame()
    {
        var sceneManager = FindObjectOfType<GameSceneManager>();
        if (sceneManager != null)
            sceneManager.RestartGame();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
    }
}
