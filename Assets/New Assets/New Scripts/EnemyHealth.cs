
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;

    [SerializeField] private float currentHealth;
    public Animator animator;

    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead || amount <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);

        if (currentHealth <= 0f)
        {
            isDead = true;

            if (animator != null)
                animator.SetTrigger("Death");

            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
