
using UnityEngine;
using UnityEngine.UI;

public class StatsBars : MonoBehaviour
{
    [SerializeField] private Slider health_slider;

    private void OnEnable()
    {
        GameEvents.OnHealthChanged -= UpdateHealth;
        GameEvents.OnHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        GameEvents.OnHealthChanged -= UpdateHealth;
    }

    private void Start()
    {
        if (health_slider == null)
        {
            Debug.LogError("Health Slider is not assigned in the Inspector!", this);
            return;
        }

        PlayerStats stats = FindObjectOfType<PlayerStats>();

        if (stats != null)
            SetHealth(stats.currentHealth, stats.maxHealth);
    }

    public void SetMaxHealth(float health)
    {
        if (health_slider == null)
            return;

        health_slider.maxValue = health;
    }

    public void SetHealth(float health)
    {
        if (health_slider == null)
            return;

        health_slider.value = Mathf.Clamp(
            health,
            health_slider.minValue,
            health_slider.maxValue
        );
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        SetMaxHealth(maxHealth);
        SetHealth(currentHealth);
    }

    private void UpdateHealth(float currentHealth, float maxHealth)
    {
        SetHealth(currentHealth, maxHealth);
    }
}
