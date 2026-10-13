using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            animator.SetTrigger("Death");
            // Invoke("Die", 2f);
        }
    }

    // void Die()
    // {
        
    //     Destroy(gameObject);
    // }
}
