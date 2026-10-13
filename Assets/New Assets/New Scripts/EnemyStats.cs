using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Color originalColor;
    public bool isFlashing = false;
    public bool isDead = false;
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashWhite());
        Debug.Log(gameObject.name + " took " + damage + " damage. Remaining HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            animator.SetBool("Death", true);
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
   

        Debug.Log(gameObject.name + " died!");
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        StartCoroutine(Death());
        GetComponent<EnemyChase>().enabled = false;
       
        
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
    public IEnumerator Death()
    {
    if (isDead) yield break;
    isDead = true;
    yield return new WaitForSeconds(3f);
    Destroy(gameObject);
    }

}
