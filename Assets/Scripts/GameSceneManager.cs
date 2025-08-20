using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // Méthode pour le bouton "Jouer" sur la scène Home
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }
    
    // Méthode pour aller à la scène Winner (appelée quand le joueur gagne)
    public void GoToWinner()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Winner");
    }
    
    // Méthode pour le bouton "Rejouer" sur la scène Winner
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }
    
    // Méthode pour retourner au menu depuis Winner ou GamePlay
    public void GoToHome()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}
