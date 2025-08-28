using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public static int score = 0;
    

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score : " + ScoreUI.score;
    }
    
    // Méthode statique pour ramasser une coin
    public static void IncrementScore(int amount)
    {
        score+= amount;
        Debug.Log($"Coin collectée ! Score: {score}");
    }
    
    // Méthode statique pour obtenir le score
    public static int GetScore()
    {
        return score;
    }
    
    
    // Réinitialiser le score
    public static void ResetScore()
    {
        score = 0;
    }
}
