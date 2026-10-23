using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 500f;
    public float currentHealth;
    public float speed = 5.5f;
    public float height;
    public int attackDamage;
    public float maxMana = 100f;
    public float currentMana;

    private void Awake()
    {
        if (currentHealth <= 0f)
            currentHealth = maxHealth;

        if (currentMana <= 0f)
            currentMana = maxMana;
    }
}