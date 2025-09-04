using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Slider slider;       // Le Slider UI
    [SerializeField] private Health targetHealth; // Le joueur à suivre

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();
        if (targetHealth != null)
        {
            slider.maxValue = targetHealth.CurrentHealth;
            slider.value = targetHealth.CurrentHealth;
        }
    }

    private void Update()
    {
        if (targetHealth != null)
        {
            slider.maxValue = 100; // ou targetHealth.maxHealth si exposé
            slider.value = targetHealth.CurrentHealth;
        }
    }
}
