using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string homeSceneName = "Home"; // Nom de ta scène de menu
    public string gamePlaySceneName = "GamePlay"; // Nom de ta scène de jeu
    
    [Header("Loading Settings")]
    public bool useAsyncLoading = true;
    
    // Méthode appelée par le bouton "Jouer" sur la scène Home
    public void StartGame()
    {
        LoadScene(gamePlaySceneName);
    }
    
    // Méthode pour retourner au menu principal
    public void GoToHome()
    {
        // Reset du score avant de retourner au menu
        CoinController.score = 0;
        LoadScene(homeSceneName);
    }
    
    // Méthode pour redémarrer la partie
    public void RestartGame()
    {
        // Reset du score
        CoinController.score = 0;
        LoadScene(gamePlaySceneName);
    }
    
    // Méthode pour quitter le jeu
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    // Méthode générique pour charger une scène
    private void LoadScene(string sceneName)
    {
        if (useAsyncLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    
    // Chargement asynchrone pour éviter les freezes
    private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        if (asyncLoad != null)
        {
            while (!asyncLoad.isDone)
            {
                // Ici tu peux ajouter une barre de chargement plus tard
                // float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                yield return null;
            }
        }
    }
    
    // Méthode statique pour accès facile depuis d'autres scripts
    public static void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
