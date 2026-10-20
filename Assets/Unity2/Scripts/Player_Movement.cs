
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private PlayerStats stats;
    private Rigidbody2D player;
    private Transform flip;
    private CapsuleCollider2D coll2D;

    [SerializeField] private LayerMask[] jumpableGrounds;

    private Animator animator;

    public PhysicsMaterial2D withFriction;
    public PhysicsMaterial2D withoutFriction;

    private bool canDoubleJump;

    void Awake()
    {
        stats = FindObjectOfType<PlayerStats>();
        player = GetComponent<Rigidbody2D>();
        flip = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        coll2D = GetComponent<CapsuleCollider2D>();

        GameEvents.GameStart += GameStart;
    }

    void OnDestroy()
{
    GameEvents.GameStart -= GameStart;
}

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        float dirx = Input.GetAxisRaw("Horizontal");

        player.velocity = new Vector2(
            dirx * stats.speed,
            player.velocity.y
        );

        animator.SetBool("running", dirx != 0);

        if (dirx == 0)
        {
            player.sharedMaterial = withFriction;
        }
        else
        {
            player.sharedMaterial = withoutFriction;
        }

        if (dirx > 0)
        {
            flip.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        }
        else if (dirx < 0)
        {
            flip.localScale = new Vector3(-3.5f, 3.5f, 3.5f);
        }

        if (IsGrounded())
        {
            canDoubleJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                Jump();
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }
    }

    void Jump()
    {
        player.velocity = new Vector2(
            player.velocity.x,
            stats.height
        );

        animator.SetTrigger("jumping");
    }

    bool IsGrounded()
    {
        foreach (LayerMask ground in jumpableGrounds)
        {
            if (Physics2D.CapsuleCast(
                coll2D.bounds.center,
                coll2D.bounds.size,
                CapsuleDirection2D.Vertical,
                0f,
                Vector2.down,
                0.2f,
                ground))
            {
                return true;
            }
        }

        return false;
    }
}
