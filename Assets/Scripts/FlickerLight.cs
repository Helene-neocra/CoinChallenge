using UnityEngine;
public class SimpleFlicker : MonoBehaviour
{
    private Light pointLight;
    
    void Start()
    {
        pointLight = GetComponent<Light>();
    }
    
    void Update()
    {
        if (pointLight != null)
        {
            // Scintillement simple
            pointLight.intensity = 10f + Mathf.Sin(Time.time * 8f) * 6f;
        }
    }
}