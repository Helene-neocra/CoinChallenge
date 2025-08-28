using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // Méthode pour le bouton "Jouer" sur la scène Home
    public void StartGame()
    {
        // S'assurer que le temps est normal avant de commencer
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }
    
    // Méthode pour aller à la scène Winner (appelée quand le joueur gagne)
    public void GoToWinner()
    {
        // S'assurer que le temps est normal avant la transition
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Winner");
    }
    
    // Méthode pour le bouton "Rejouer" sur la scène Winner
    public void RestartGame()
    {
        // S'assurer que le temps est normal avant de redémarrer
        Time.timeScale = 1f;
        ScoreUI.ResetScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }
    
    // Méthode pour retourner au menu depuis Winner ou GamePlay
    public void GoToHome()
    {
        // S'assurer que le temps est normal avant de retourner au menu
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}
