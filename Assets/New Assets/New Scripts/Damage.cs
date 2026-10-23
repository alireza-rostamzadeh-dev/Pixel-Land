
using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private PlayerStats stats;
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private CapsuleCollider2D playerCollider;
    private bool isDead;

    public float TrapDamage = 50f;
    public StatsBars statsBar;
    public GameObject gameOverPanel;
    public AudioSource gameOverMusic;
    public SpriteRenderer spriteRenderer;
    public Color originalColor;
    public bool isFlashing;
    public GameObject bloodParticle;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (statsBar == null)
            statsBar = FindObjectOfType<StatsBars>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        if (stats != null)
            GameEvents.HealthChanged(stats.currentHealth, stats.maxHealth);
    }

    private void Update()
    {
        if (!isDead && stats != null && stats.currentHealth <= 0f)
            Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
            TakeDamage(TrapDamage);
    }

    public void TakeDamage(float damage)
    {
        if (isDead || stats == null || damage <= 0f)
            return;

        stats.currentHealth = Mathf.Max(
            0f,
            stats.currentHealth - damage
        );

        GameEvents.HealthChanged(stats.currentHealth, stats.maxHealth);
        GameEvents.OnPlayerDamaged.Invoke(damage);

        if (bloodParticle != null)
            Instantiate(bloodParticle, transform.position, transform.rotation);

        if (stats.currentHealth <= 0f)
        {
            Die();
            return;
        }

        if (animator != null)
            animator.SetTrigger("hurt");

        if (spriteRenderer != null)
            StartCoroutine(FlashWhite());
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (stats != null)
        {
            stats.currentHealth = 0f;
            GameEvents.HealthChanged(stats.currentHealth, stats.maxHealth);
        }

        GameEvents.OnPlayerDied.Invoke();
        GameEvents.OnGameOver.Invoke();

        if (animator != null)
        {
            animator.ResetTrigger("hurt");
            animator.SetTrigger("death");
        }

        if (rb != null)
            rb.velocity = Vector2.zero;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerCollider != null)
            playerCollider.enabled = false;

        if (playerCombat != null)
            playerCombat.enabled = false;

        AudioListener.pause = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverMusic != null)
        {
            gameOverMusic.ignoreListenerPause = true;
            gameOverMusic.Play();
        }
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
}
