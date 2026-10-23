
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStats stats;
    private Rigidbody2D player;
    private Transform flip;
    private CapsuleCollider2D coll2D;
    private Animator animator;
    private PlayerItems playerItems;

    [SerializeField] private LayerMask[] jumpableGrounds;
    [SerializeField] private float knockbackControlLock = 0.35f;
    [SerializeField] private float defaultKnockbackForce = 8f;
    [SerializeField] private float knockbackUpwardFactor = 0.45f;

    public PhysicsMaterial2D withFriction;
    public PhysicsMaterial2D withoutFriction;

    private bool canDoubleJump;
    private bool isKnockedBack;
    private bool isAttacking;
    private float knockbackEndTime;

    private StateMachine<PlayerMovement> stateMachine;

    private PlayerIdleState idleState;
    private PlayerRunningState runningState;
    private PlayerJumpingState jumpingState;
    private PlayerFallingState fallingState;

    private float horizontalInput;
    private bool grounded;
    private bool jumpedThisFrame;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        player = GetComponent<Rigidbody2D>();
        flip = transform;
        animator = GetComponent<Animator>();
        coll2D = GetComponent<CapsuleCollider2D>();
        playerItems = GetComponent<PlayerItems>();

        if (stats == null || player == null || animator == null || coll2D == null)
        {
            Debug.LogError("PlayerMovement is missing a required component.", this);
            enabled = false;
            return;
        }

        player.bodyType = RigidbodyType2D.Dynamic;
        player.constraints &= ~(RigidbodyConstraints2D.FreezePositionX |
                               RigidbodyConstraints2D.FreezePositionY);
    }

    private void OnEnable()
    {
        GameEvents.OnKnockbackRequested -= OnKnockbackRequested;
        GameEvents.OnKnockbackRequested += OnKnockbackRequested;
    }

    private void OnDisable()
    {
        GameEvents.OnKnockbackRequested -= OnKnockbackRequested;
    }

    private void Start()
    {
        if (!enabled)
            return;

        BuildStateMachine();
    }

    private void BuildStateMachine()
    {
        stateMachine = new StateMachine<PlayerMovement>();

        idleState = new PlayerIdleState(this, stateMachine);
        runningState = new PlayerRunningState(this, stateMachine);
        jumpingState = new PlayerJumpingState(this, stateMachine);
        fallingState = new PlayerFallingState(this, stateMachine);

        stateMachine.AddState(idleState);
        stateMachine.AddState(runningState);
        stateMachine.AddState(jumpingState);
        stateMachine.AddState(fallingState);

        stateMachine.AddTransition(idleState, runningState, "move");
        stateMachine.AddTransition(runningState, idleState, "idle");

        stateMachine.AddTransition(idleState, jumpingState, "jump");
        stateMachine.AddTransition(runningState, jumpingState, "jump");
        stateMachine.AddTransition(fallingState, jumpingState, "jump");
        stateMachine.AddTransition(jumpingState, jumpingState, "jump");

        stateMachine.AddTransition(idleState, fallingState, "fall");
        stateMachine.AddTransition(runningState, fallingState, "fall");
        stateMachine.AddTransition(jumpingState, fallingState, "fall");

        stateMachine.AddTransition(fallingState, idleState, "idle");
        stateMachine.AddTransition(fallingState, runningState, "move");

        stateMachine.AddTransition(jumpingState, idleState, "idle", IsGrounded);
        stateMachine.AddTransition(jumpingState, runningState, "move", IsGrounded);

        stateMachine.Start();
    }

    private void Update()
    {
        if (Time.timeScale == 0f || stateMachine == null)
            return;

        if (isKnockedBack)
        {
            if (Time.time < knockbackEndTime)
                return;

            isKnockedBack = false;
        }

        if (isAttacking)
        {
            horizontalInput = 0f;
            player.velocity = new Vector2(0f, player.velocity.y);
            animator.SetBool("running", false);
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        grounded = IsGrounded();
        jumpedThisFrame = false;

        if (grounded)
            canDoubleJump = true;

        HandleMovement();
        HandleJump();
        HandlePotion();

        UpdateMovementState();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (isKnockedBack && Time.time < knockbackEndTime)
        {
            player.sharedMaterial = withoutFriction;
        }
    }

    private void OnKnockbackRequested(
        GameObject target,
        Vector2 attackerPosition,
        float force)
    {
        if (target != gameObject || player == null || !enabled)
            return;

        float directionX = transform.position.x >= attackerPosition.x ? 1f : -1f;

        float appliedForce = force > 0f
            ? force
            : defaultKnockbackForce;

        Vector2 impulse = new Vector2(
            directionX * appliedForce,
            appliedForce * knockbackUpwardFactor
        );

        isKnockedBack = true;
        knockbackEndTime = Time.time + knockbackControlLock;

        player.velocity = Vector2.zero;
        player.sharedMaterial = withoutFriction;
        player.WakeUp();
        player.AddForce(impulse, ForceMode2D.Impulse);

        if (animator != null)
            animator.SetBool("running", false);
    }

    public void SetAttacking(bool value)
    {
        isAttacking = value;

        if (value)
        {
            horizontalInput = 0f;

            if (player != null)
                player.velocity = new Vector2(0f, player.velocity.y);

            if (animator != null)
                animator.SetBool("running", false);
        }
    }

    private void HandleMovement()
    {
        if (isKnockedBack && Time.time < knockbackEndTime)
        {
            animator.SetBool("running", false);
            return;
        }

        player.velocity = new Vector2(
            horizontalInput * stats.speed,
            player.velocity.y
        );

        bool moving = !Mathf.Approximately(horizontalInput, 0f);

        animator.SetBool("running", moving);

        if (moving)
        {
            flip.localScale = horizontalInput > 0f
                ? new Vector3(3.5f, 3.5f, 3.5f)
                : new Vector3(-3.5f, 3.5f, 3.5f);

            if (withoutFriction != null)
                player.sharedMaterial = withoutFriction;
        }
        else if (withFriction != null)
        {
            player.sharedMaterial = withFriction;
        }
    }

    private void HandleJump()
    {
        if (isAttacking || !Input.GetKeyDown(KeyCode.Space))
            return;

        if (grounded)
        {
            Jump();
            jumpedThisFrame = true;
        }
        else if (canDoubleJump)
        {
            Jump();
            canDoubleJump = false;
            jumpedThisFrame = true;
        }
    }

    private void HandlePotion()
    {
        if (Input.GetKeyDown(KeyCode.R) &&
            stats.currentHealth < stats.maxHealth &&
            playerItems != null)
        {
            playerItems.UseHealthPotion();
        }
    }

    private void Jump()
    {
        player.velocity = new Vector2(player.velocity.x, stats.height);
    }

    private void UpdateMovementState()
    {
        if (jumpedThisFrame)
        {
            stateMachine.SendMessage("jump");
            return;
        }

        if (grounded)
        {
            stateMachine.SendMessage(
                Mathf.Approximately(horizontalInput, 0f) ? "idle" : "move"
            );
        }
        else if (player.velocity.y <= 0f)
        {
            stateMachine.SendMessage("fall");
        }
    }

    private bool IsGrounded()
    {
        if (coll2D == null || jumpableGrounds == null)
            return false;

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

    internal Animator AnimatorComponent
    {
        get { return animator; }
    }

    internal Rigidbody2D RigidbodyComponent
    {
        get { return player; }
    }

    internal PlayerStats Stats
    {
        get { return stats; }
    }

    internal float HorizontalInput
    {
        get { return horizontalInput; }
    }

    internal bool Grounded
    {
        get { return grounded; }
    }
}

public sealed class PlayerIdleState : State<PlayerMovement>
{
    public PlayerIdleState(PlayerMovement player, StateMachine<PlayerMovement> machine)
        : base(player, machine) { }

    public override void Enter()
    {
        entity.AnimatorComponent.SetBool("running", false);
    }
}

public sealed class PlayerRunningState : State<PlayerMovement>
{
    public PlayerRunningState(PlayerMovement player, StateMachine<PlayerMovement> machine)
        : base(player, machine) { }

    public override void Enter()
    {
        entity.AnimatorComponent.SetBool("running", true);
    }
}

public sealed class PlayerJumpingState : State<PlayerMovement>
{
    public PlayerJumpingState(PlayerMovement player, StateMachine<PlayerMovement> machine)
        : base(player, machine) { }

    public override void Enter()
    {
        entity.AnimatorComponent.SetTrigger("jumping");
    }
}

public sealed class PlayerFallingState : State<PlayerMovement>
{
    public PlayerFallingState(PlayerMovement player, StateMachine<PlayerMovement> machine)
        : base(player, machine) { }

    public override void Enter()
    {
        entity.AnimatorComponent.SetBool("running", false);
    }
}
