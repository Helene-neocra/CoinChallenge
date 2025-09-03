using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float gameTime = 60f; // 1 minute en secondes
    public TMP_Text timerText; // Référence au texte UI
    public Light directionalLight; // Optionnel : assigner dans l'inspecteur
    
    private float currentTime;
    private bool isGameRunning = false; // Changé à false pour ne pas démarrer automatiquement
    private bool hasStarted = false; // Nouveau flag pour éviter les démarrages multiples
    private Coroutine lightTransitionCoroutine;
    
    public delegate void TimeUpDelegate();
    public static event TimeUpDelegate OnTimeUp;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = gameTime;
        UpdateTimerDisplay();
        hasStarted = false; // Réinitialiser le flag à chaque nouvelle scène
        SetDirectionalLightIntensity(1f); // Intensité à 1 au démarrage
        
        // S'abonner à l'événement du FloorGenerator pour démarrer le timer
        FloorGenerator floorGen = FindObjectOfType<FloorGenerator>();
        if (floorGen != null)
        {
            floorGen.OnFloorGenerated += OnFloorReady;
        }
    }
    
    void OnFloorReady(float minX, float minZ, float maxX, float maxZ)
    {
        // Le floor est prêt, on peut maintenant détecter quand le joueur y entre
        // Cette méthode sera appelée quand le floor est généré
    }
    
    public void StartTimer()
    {
        Debug.Log("[Timer] StartTimer appelé");
        if (!hasStarted)
        {
            isGameRunning = true;
            hasStarted = true;
            StartLightTransition(0.5f, 1f); // Transition douce vers 0.2 sur 1 seconde
        }
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
    
    private void SetDirectionalLightIntensity(float intensity)
    {
        Light lightToUse = directionalLight;
        if (lightToUse == null)
        {
            // Chercher explicitement le Directional Light
            var allLights = GameObject.FindObjectsOfType<Light>();
            foreach (var l in allLights)
            {
                if (l.type == LightType.Directional)
                {
                    lightToUse = l;
                    Debug.Log($"[Timer] Directional Light trouvé automatiquement : {l.name}");
                    break;
                }
            }
            if (lightToUse == null)
            {
                Debug.LogWarning("[Timer] Aucun Directional Light trouvé dans la scène !");
                return;
            }
        }
        else
        {
            Debug.Log($"[Timer] Light assigné dans l'inspecteur : {lightToUse.name}, type={lightToUse.type}");
        }
        lightToUse.intensity = intensity;
       
    }
    
    private void StartLightTransition(float targetIntensity, float duration)
    {
        if (lightTransitionCoroutine != null)
            StopCoroutine(lightTransitionCoroutine);
        lightTransitionCoroutine = StartCoroutine(LightIntensityTransition(targetIntensity, duration));
    }

    private IEnumerator LightIntensityTransition(float targetIntensity, float duration)
    {
        Light lightToUse = directionalLight;
        if (lightToUse == null)
        {
            var allLights = GameObject.FindObjectsOfType<Light>();
            foreach (var l in allLights)
            {
                if (l.type == LightType.Directional)
                {
                    lightToUse = l;
                    break;
                }
            }
            if (lightToUse == null)
                yield break;
        }
        float startIntensity = lightToUse.intensity;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            lightToUse.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            yield return null;
        }
        lightToUse.intensity = targetIntensity;
    }
}
