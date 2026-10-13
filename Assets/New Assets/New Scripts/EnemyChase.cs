using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;

    [Header("Attack")]
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 20;

    [Header("SFX")]
    public AudioClip swordClip;   
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;

    private bool isChasing = false;
    private float lastAttackTime = 0f;

    private PlayerStats playerStats; 
    private Damage PlayerDamage;
    private EnemyStats enemyStats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PlayerDamage = FindObjectOfType<Damage>();
        enemyStats = gameObject.GetComponent<EnemyStats>();
        // گرفتن یا ساختن AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (enemyStats.isDead) return;
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (hit != null)
        {
            player = hit.transform;
            playerStats = player.GetComponent<PlayerStats>(); 

            
            if (IsPlayerDead())
            {
                isChasing = false;
                animator.SetBool("Running", false);
                return;
            }

            isChasing = true;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && Time.time - lastAttackTime > attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            isChasing = false;
            player = null;
            playerStats = null;
        }

        animator.SetBool("Running", isChasing);
    }

    void FixedUpdate()
    {
        
        if (IsPlayerDead())
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_Gate_Keeper"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        if (isChasing && player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

            
            if (direction.x > 0)
                transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-4.5f, 4.5f, 4.5f);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Attack()
    {
        if (IsPlayerDead()) return;

        animator.SetTrigger("Attack");

        
        if (swordClip != null && audioSource != null)
            audioSource.PlayOneShot(swordClip);

        
        if (playerStats != null)
        {
            PlayerDamage.TakeDamage(attackDamage);
            
            playerStats.currentHealth = Mathf.Max(playerStats.currentHealth, 0);

            var bars = FindObjectOfType<StatsBars>();
            if (bars != null) bars.SetHealth(playerStats.currentHealth);

            Debug.Log("Player damaged: " + attackDamage);
        }
    }

    bool IsPlayerDead()
    {
        return (playerStats == null) || (playerStats.currentHealth <= 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
