
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Knockback : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float knockbackDuration = 0.15f;

    private Rigidbody2D rb;
    private Coroutine knockbackCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        ApplyKnockback(attackerPosition, force);
    }

    private void ApplyKnockback(
        Vector2 attackerPosition,
        float force)
    {
        Vector2 direction =
            (Vector2)transform.position - attackerPosition;

        if (direction.sqrMagnitude < 0.001f)
            direction = Vector2.right;
        else
            direction.Normalize();

        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        float appliedForce =
            force > 0f ? force : knockbackForce;

        knockbackCoroutine = StartCoroutine(
            KnockbackRoutine(direction * appliedForce)
        );
    }

    private IEnumerator KnockbackRoutine(Vector2 impulse)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(impulse, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        knockbackCoroutine = null;
    }
}
