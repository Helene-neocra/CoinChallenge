using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float gameTime = 60f; // 1 minute en secondes
    public TMP_Text timerText; // Référence au texte UI
    
    private float currentTime;
    private bool isGameRunning = true;
    
    public delegate void TimeUpDelegate();
    public static event TimeUpDelegate OnTimeUp;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = gameTime;
        UpdateTimerDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
            currentTime -= Time.deltaTime;
            
            if (currentTime <= 0)
            {
                currentTime = 0;
                isGameRunning = false;
                OnTimeUp?.Invoke(); // Déclenche l'événement de fin de jeu
            }
            
            UpdateTimerDisplay();
        }
    }
    
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    
    public void StopTimer()
    {
        isGameRunning = false;
    }
    
    public bool IsGameRunning()
    {
        return isGameRunning;
    }
}
