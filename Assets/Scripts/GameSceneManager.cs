using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // Méthode pour le bouton "Jouer" sur la scène Home
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }
    
    // Méthode pour le bouton "Rejouer" sur la scène GamePlay
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlay");
    }
    
    // Méthode pour retourner au menu (optionnel)
    public void GoToHome()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
}
