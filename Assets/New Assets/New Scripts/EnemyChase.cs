
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    public float stopDistance = 0.8f;
    public float knockbackRecoveryTime = 0.2f;

    [Header("Attack")]
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 20;
    public float knockbackForce = 8f;

    [Header("SFX")]
    public AudioClip swordClip;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private Transform player;
    private Animator animator;
    private PlayerStats playerStats;
    private Damage playerDamage;
    private EnemyStats enemyStats;

    private bool isChasing;
    private float lastAttackTime = -Mathf.Infinity;
    private float knockbackEndTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyStats = GetComponent<EnemyStats>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameEvents.OnKnockbackRequested += OnKnockbackRequested;
    }

    private void OnDisable()
    {
        GameEvents.OnKnockbackRequested -= OnKnockbackRequested;
    }

    private void OnKnockbackRequested(
        GameObject target,
        Vector2 attackerPosition,
        float force)
    {
        if (target != gameObject)
            return;

        knockbackEndTime = Time.time + knockbackRecoveryTime;
    }

    private void Update()
    {
        if (enemyStats != null && enemyStats.isDead)
        {
            StopChasing();
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            detectionRadius,
            playerLayer
        );

        if (hit == null)
        {
            StopChasing();
            return;
        }

        PlayerStats targetStats =
            hit.GetComponentInParent<PlayerStats>();

        if (targetStats == null || targetStats.currentHealth <= 0f)
        {
            StopChasing();
            return;
        }

        playerStats = targetStats;
        player = targetStats.transform;
        playerDamage = targetStats.GetComponent<Damage>();
        isChasing = true;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRange &&
            Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            Attack();
        }

        if (animator != null)
        {
            animator.SetBool(
                "Running",
                isChasing &&
                distance > stopDistance &&
                Time.time >= knockbackEndTime
            );
        }
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        if (enemyStats != null && enemyStats.isDead)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        if (Time.time < knockbackEndTime)
            return;

        if (IsAttacking() ||
            !isChasing ||
            player == null ||
            IsPlayerDead())
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        float directionX = player.position.x - transform.position.x;

        if (Mathf.Abs(directionX) <= stopDistance)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);

            if (animator != null)
                animator.SetBool("Running", false);

            return;
        }

        rb.velocity = new Vector2(
            Mathf.Sign(directionX) * speed,
            rb.velocity.y
        );

        if (animator != null)
            animator.SetBool("Running", true);

        if (Mathf.Abs(directionX) > 0.01f)
        {
            transform.localScale = directionX > 0f
                ? new Vector3(4.5f, 4.5f, 4.5f)
                : new Vector3(-4.5f, 4.5f, 4.5f);
        }
    }

    private void Attack()
    {
        if (IsPlayerDead())
            return;

        if (animator != null)
        {
            animator.SetBool("Running", false);
            animator.SetTrigger("Attack");
        }

        if (swordClip != null && audioSource != null)
            audioSource.PlayOneShot(swordClip);

        float previousHealth = playerStats.currentHealth;

        if (playerDamage != null)
        {
            playerDamage.TakeDamage(attackDamage);
        }
        else
        {
            playerStats.currentHealth = Mathf.Max(
                0f,
                playerStats.currentHealth - attackDamage
            );

            GameEvents.HealthChanged(
                playerStats.currentHealth,
                playerStats.maxHealth
            );
        }

        bool damageApplied =
            playerStats.currentHealth < previousHealth;

        if (damageApplied)
        {
            GameEvents.RequestKnockback(
                playerStats.gameObject,
                transform.position,
                knockbackForce
            );
        }
    }

    private bool IsPlayerDead()
    {
        return playerStats == null ||
               playerStats.currentHealth <= 0f;
    }

    private bool IsAttacking()
    {
        if (animator == null)
            return false;

        AnimatorStateInfo state =
            animator.GetCurrentAnimatorStateInfo(0);

        return state.IsName("Attack_Gate_Keeper") &&
               state.normalizedTime < 1f;
    }

    private void StopChasing()
    {
        isChasing = false;
        player = null;
        playerStats = null;
        playerDamage = null;

        if (animator != null)
            animator.SetBool("Running", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
