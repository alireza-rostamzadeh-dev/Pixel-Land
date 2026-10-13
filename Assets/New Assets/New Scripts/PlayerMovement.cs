using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStats stats;
    private Rigidbody2D player;
    private Transform flip;
    private CapsuleCollider2D coll2D;
    [SerializeField] private LayerMask[] jumpableGrounds;
    private Animator animator;
    public PhysicsMaterial2D withFriction;
    public PhysicsMaterial2D withoutFriction;
    private PlayerItems playerItems;
    private bool canDoubleJump;

    void Awake()
    {
        stats = FindObjectOfType<PlayerStats>();
        player = GetComponent<Rigidbody2D>();
        flip = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        coll2D = GetComponent<CapsuleCollider2D>();
        playerItems = GetComponent<PlayerItems>();
    }

    
    void Update()
    {
        if (Time.timeScale == 0)
            return;

        float dirx = Input.GetAxisRaw("Horizontal");
        player.velocity = new Vector2(dirx * stats.speed, player.velocity.y);


        if (dirx == 0)
        {
            animator.SetBool("running", false);
            player.sharedMaterial = withFriction;
        }

        if (dirx > 0f)
        {
            flip.localScale = new Vector3(3.5f, 3.5f, 3.5f);
            player.sharedMaterial = withoutFriction;
            animator.SetBool("running", true);
        }


        else if (dirx < 0f)
        {
            flip.localScale = new Vector3(-3.5f, 3.5f, 3.5f);
            player.sharedMaterial = withoutFriction;
            animator.SetBool("running", true);
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

        void Jump()
    {
        player.velocity = new Vector2(player.velocity.x, stats.height);
        animator.SetTrigger("jumping");
    }

        // if(Input.GetButtonDown("Jump") && IsGrounded()) {
        //     player.AddForce(new Vector2(player.velocity.x , stats.height));
        // }

        if (Input.GetKeyDown(KeyCode.R) && stats.currentHealth < stats.maxHealth)
        {
            playerItems.UseHealthPotion();
        }
        if (Input.GetKeyDown(KeyCode.Q) && stats.currentMana < stats.maxMana)
        {
            playerItems.UseManaPotion();
        }
        
        }
  bool IsGrounded()
    {
        foreach (LayerMask ground in jumpableGrounds)
        {
            if (Physics2D.CapsuleCast(coll2D.bounds.center, coll2D.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, 0.2f, ground))
            {
                return true;
            }
        }
        return false;
    }






    

    }

    


