using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{   private Rigidbody2D player;
    public float height;
    public float speed;
    private float dirx = 0f;
    private Transform flip;
    private Animator anim;
    private CapsuleCollider2D coll; 
    [SerializeField] private LayerMask jumpableGround;
    //[SerializeField] private GameObject Grounded;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;
    public float attackRate = 1f;
    float nextattackTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
        flip = GetComponent<Transform>();
        coll = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    { 
    //    if (Input.GetKeyDown("space")){
    //         player.velocity = new Vector2(player.velocity.x , height);
    //     }

    //     if (Input.GetKey(KeyCode.Mouse0)){
    //         player.velocity = new Vector2(speed , player.velocity.y);
            
    //     }

    //     if (Input.GetKey(KeyCode.A)){
    //         player.velocity = new Vector3(-speed , player.velocity.y);
    //         flip.localScale = new Vector3(-1 , 1 , 1);
    //     }

        
        
         dirx = Input.GetAxisRaw("Horizontal");
         player.velocity = new Vector2(dirx * speed  , player.velocity.y);
         
         if(dirx > 0f){
            flip.localScale = new Vector3(3 , 3 , 3);
            anim.SetBool("running" , true);
         }
         else if(dirx < 0f){
            flip.localScale = new Vector3(-3 , 3 , 3);
            anim.SetBool("running" , true);
         }

         else {
            anim.SetBool("running" , false);
         }

            
        if (Input.GetButtonDown("Jump") && IsGrounded() && !IsCeiling()){
            player.velocity = new Vector2(player.velocity.x , height);
            anim.SetBool("running" , false);
            anim.SetBool("jumping" , true);

        }

        
        
        // else if (Input.GetButtonDown("Jump") && IsGrounded() && IsInSky()){
        //     anim.SetBool("jumping" , true);
        // }

        else
        {
            anim.SetBool("jumping", false);

        }

        if(Time.time >= nextattackTime){
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            
            Attack();
            nextattackTime = Time.time + 1f/attackRate;
         }
        }
        // else {
        //     anim.SetTrigger("attack");
        // }

        bool IsGrounded(){
            return Physics2D.CapsuleCast(coll.bounds.center , coll.bounds.size ,CapsuleDirection2D.Vertical, 0f , Vector2.down , 0.05f , jumpableGround);
        }

         bool IsCeiling(){
            return Physics2D.CapsuleCast(coll.bounds.center , coll.bounds.size ,CapsuleDirection2D.Vertical, 0f , Vector2.up , 1f , jumpableGround);
        }

    } 





    void Attack(){
        anim.SetTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position , attackRange , enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

    }
    
    private void OnDrawGizmosSelected(){
        if (attackPoint == null)
            return;

    Gizmos.DrawWireSphere(attackPoint.position , attackRange);
    }
    
}
