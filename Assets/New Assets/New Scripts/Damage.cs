using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private PlayerStats stats;
    private Animator animator;
    private Rigidbody2D rb;
    public float TrapDamage = 50;

    private bool isDead = false;
    public StatsBars statsBar;

    [Header("Game Over Settings")]
    public GameObject gameOverPanel;
    public AudioSource gameOverMusic;

    public SpriteRenderer spriteRenderer;
    public Color originalColor;
    public bool isFlashing = false;
    public GameObject bloodParticle;

    void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        statsBar = FindObjectOfType<StatsBars>();

        statsBar.SetMaxHealth(stats.maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {

        if (!isDead && stats.currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Debug.Log("Hit the Trap!");
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        stats.currentHealth -= damage;
        stats.currentHealth = Mathf.Max(stats.currentHealth, 0);
        bloodParticle = Instantiate(bloodParticle, transform.position, transform.rotation, transform);

        statsBar.SetHealth(stats.currentHealth);
        StartCoroutine(FlashWhite());
        animator.SetTrigger("hurt");
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        stats.currentHealth = 0f;
        statsBar.SetHealth(stats.currentHealth);

        animator.ResetTrigger("hurt");
        animator.SetTrigger("death");

        rb.velocity = Vector2.zero;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;


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
    if (isFlashing) yield break;

    isFlashing = true;
    spriteRenderer.color = new Color32(0x87, 0x87, 0x87, 0xFF);
    yield return new WaitForSeconds(0.3f);
    spriteRenderer.color = originalColor;
    isFlashing = false;
}



}
