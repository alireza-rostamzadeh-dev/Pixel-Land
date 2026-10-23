
using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 100;

    [SerializeField] private int currentHealth;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Color originalColor;
    public bool isFlashing;
    public bool isDead;

    private Rigidbody2D rb;
    private EnemyChase enemyChase;

    private void Awake()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();
        enemyChase = GetComponent<EnemyChase>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        GameEvents.OnEnemyDamaged.Invoke(gameObject, damage);

        if (spriteRenderer != null)
            StartCoroutine(FlashWhite());

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        GameEvents.OnEnemyDied.Invoke(gameObject);

        if (animator != null)
        {
            animator.SetBool("Death", true);
            animator.SetTrigger("Death");
        }

        if (rb != null)
            rb.velocity = Vector2.zero;

        if (enemyChase != null)
            enemyChase.enabled = false;

        StartCoroutine(Death());
    }

    public IEnumerator FlashWhite()
    {
        if (isFlashing || spriteRenderer == null)
            yield break;

        isFlashing = true;
        spriteRenderer.color = new Color32(135, 135, 135, 255);

        yield return new WaitForSeconds(0.3f);

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        isFlashing = false;
    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
