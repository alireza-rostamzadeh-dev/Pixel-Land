
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 25;
    public float attackRate = 2f;
    public float knockbackForce = 8f;

    [Header("Attack Movement Lock")]
    [SerializeField] private float attackLockDuration = 0.5f;

    [Header("Sound Effects")]
    public AudioClip swordSound;

    private float nextAttackTime;
    private AudioSource audioSource;
    private PlayerMovement playerMovement;
    private bool isAttacking;
    private Coroutine attackCoroutine;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f || isAttacking || Time.time < nextAttackTime)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
            nextAttackTime = Time.time + 1f / Mathf.Max(attackRate, 0.01f);
        }
    }

    private void Attack()
    {
        isAttacking = true;

        if (playerMovement != null)
            playerMovement.SetAttacking(true);

        if (animator != null)
            animator.SetTrigger("Attack");

        if (swordSound != null && audioSource != null)
            audioSource.PlayOneShot(swordSound);

        attackCoroutine = StartCoroutine(AttackLockRoutine());

        if (attackPoint == null)
            return;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyStats enemyStats = enemy.GetComponentInParent<EnemyStats>();

            if (enemyStats == null || enemyStats.isDead)
                continue;

            enemyStats.TakeDamage(attackDamage);

            if (!enemyStats.isDead)
            {
                GameEvents.RequestKnockback(
                    enemyStats.gameObject,
                    transform.position,
                    knockbackForce
                );
            }
        }
    }

    private IEnumerator AttackLockRoutine()
    {
        yield return new WaitForSeconds(attackLockDuration);

        if (playerMovement != null)
            playerMovement.SetAttacking(false);

        isAttacking = false;
        attackCoroutine = null;
    }

    private void OnDisable()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        isAttacking = false;

        if (playerMovement != null)
            playerMovement.SetAttacking(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
