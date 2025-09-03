using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;

    public int CurrentHealth { get; private set; }

    // (current, max)
    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;

    private void Awake()
    {
        CurrentHealth = Mathf.Max(1, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    /// <summary>Change la vie max. Option pour remplir à fond.</summary>
    public void SetMaxHealth(int value, bool fillToMax = true)
    {
        maxHealth = Mathf.Max(1, value);
        if (fillToMax) CurrentHealth = maxHealth;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0) return;
        int before = CurrentHealth;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        if (CurrentHealth != before) OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0) return;
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        if (CurrentHealth <= 0) OnDied?.Invoke();
    }

    public void Kill()
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth = 0;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        OnDied?.Invoke();
    }

    // Petites commandes pour tester depuis l'Inspector (clic ⋮ du composant)
    [ContextMenu("Debug: Damage 10")] private void _dbgDamage10() => TakeDamage(10);
    [ContextMenu("Debug: Heal 10")]   private void _dbgHeal10()   => Heal(10);
    [ContextMenu("Debug: Kill")]      private void _dbgKill()      => Kill();
}

