using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class WinnerUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text finalScoreText;
    public TMP_Text congratulationsText;
    public GameObject scorePanel;
    public Button playAgainButton;
    public Button homeButton;
    
    [Header("Animation Settings")]
    public float animationDelay = 1f;
    public float scoreCountDuration = 2f;
    
    private int _targetScore;
    
    void Start()
    {
        // Récupère le score final depuis ScoreUI
        _targetScore = ScoreUI.GetScore();
        
        // Initialise l'interface
        InitializeUI();
        
        // Lance les animations d'entrée
        StartCoroutine(StartWinnerAnimations());
    }
    
    void InitializeUI()
    {
        // Initialise le texte de score à 0
        if (finalScoreText != null)
            finalScoreText.text = "Score Final: 0";
            
        // Cache initialement certains éléments pour l'animation
        if (scorePanel != null)
            scorePanel.transform.localScale = Vector3.zero;
            
        if (playAgainButton != null)
            playAgainButton.gameObject.SetActive(false);
            
        if (homeButton != null)
            homeButton.gameObject.SetActive(false);
    }
    
    IEnumerator StartWinnerAnimations()
    {
        // Animation du texte de félicitations
        if (congratulationsText != null)
        {
            StartCoroutine(BounceAnimation(congratulationsText.transform, 1.2f, 0.5f));
        }
        
        // Attendre avant d'animer le panneau de score
        yield return new WaitForSeconds(animationDelay);
        
        // Animation du panneau de score
        if (scorePanel != null)
        {
            StartCoroutine(ScaleInAnimation(scorePanel.transform, 0.8f));
        }
        
        // Lance le comptage animé du score
        StartCoroutine(AnimateScoreCount());
        
        // Affiche les boutons après l'animation du score
        yield return new WaitForSeconds(scoreCountDuration + 0.5f);
        StartCoroutine(ShowButtons());
    }
    
    IEnumerator BounceAnimation(Transform target, float maxScale, float duration)
    {
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * maxScale;
        
        float elapsedTime = 0f;
        
        // Animation aller (agrandissement)
        while (elapsedTime < duration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (duration / 2f);
            
            // Courbe d'easing bounce out
            float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);
            target.localScale = Vector3.Lerp(originalScale, targetScale, easedProgress);
            
            yield return null;
        }
        
        elapsedTime = 0f;
        
        // Animation retour (rétrécissement)
        while (elapsedTime < duration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (duration / 2f);
            
            float easedProgress = Mathf.Pow(progress, 2f);
            target.localScale = Vector3.Lerp(targetScale, originalScale, easedProgress);
            
            yield return null;
        }
        
        target.localScale = originalScale;
    }
    
    IEnumerator ScaleInAnimation(Transform target, float duration)
    {
        Vector3 targetScale = new Vector3(0.5f, 0.5f, 0.5f);
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            
            // Courbe d'easing elastic out
            float easedProgress = Mathf.Sin(progress * Mathf.PI * 0.5f);
            target.localScale = Vector3.Lerp(Vector3.zero, targetScale, easedProgress);
            
            yield return null;
        }
        
        target.localScale = targetScale;
    }
    
    IEnumerator AnimateScoreCount()
    {
        if (finalScoreText == null) yield break;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < scoreCountDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / scoreCountDuration;
            
            // Interpolation avec courbe d'easing
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(0, _targetScore, easedProgress));
            
            finalScoreText.text = $"Score Final: {currentScore}";
            
            yield return null;
        }
        
        // S'assurer que le score final est affiché
        finalScoreText.text = $"Score Final: {_targetScore}";
        
        // Animation de pulse sur le score final
        StartCoroutine(PulseAnimation(finalScoreText.transform, 1.1f, 0.6f));
    }
    
    IEnumerator PulseAnimation(Transform target, float maxScale, float duration)
    {
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * maxScale;
        
        float elapsedTime = 0f;
        
        // Animation aller
        while (elapsedTime < duration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (duration / 2f);
            
            target.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.Sin(progress * Mathf.PI * 0.5f));
            yield return null;
        }
        
        elapsedTime = 0f;
        
        // Animation retour
        while (elapsedTime < duration / 2f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (duration / 2f);
            
            target.localScale = Vector3.Lerp(targetScale, originalScale, Mathf.Sin(progress * Mathf.PI * 0.5f));
            yield return null;
        }
        
        target.localScale = originalScale;
    }
    
    IEnumerator ShowButtons()
    {
        // Anime l'apparition du bouton Rejouer
        if (playAgainButton != null)
        {
            playAgainButton.gameObject.SetActive(true);
            playAgainButton.transform.localScale = Vector3.zero;
            StartCoroutine(ScaleInAnimation(playAgainButton.transform, 0.5f));
        }
        
        // Délai avant le deuxième bouton
        yield return new WaitForSeconds(0.2f);
        
        // Anime l'apparition du bouton Menu
        if (homeButton != null)
        {
            homeButton.gameObject.SetActive(true);
            homeButton.transform.localScale = Vector3.zero;
            StartCoroutine(ScaleInAnimation(homeButton.transform, 0.5f));
        }
    }
    
    // Méthode pour remettre à zéro le score avant de rejouer
    public void OnPlayAgain()
    {
        ScoreUI.ResetScore();
        
        // Trouve le GameSceneManager et relance le jeu
        GameSceneManager sceneManager = FindObjectOfType<GameSceneManager>();
        if (sceneManager != null)
        {
            sceneManager.RestartGame();
        }
    }
    
    // Méthode pour retourner au menu
    public void OnGoHome()
    {
        ScoreUI.ResetScore();
        
        // Trouve le GameSceneManager et va au menu
        GameSceneManager sceneManager = FindObjectOfType<GameSceneManager>();
        if (sceneManager != null)
        {
            sceneManager.GoToHome();
        }
    }
}
