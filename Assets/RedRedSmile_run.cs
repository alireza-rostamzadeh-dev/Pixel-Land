using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRedSmile_run : StateMachineBehaviour
{   
    Transform player;
    Rigidbody2D rb;
    float Speed = 2.5f;
   public float attackRange = 3f;
    ReSmileFllip boss;
    Animator anim;

    PlayerDeath playerHealth;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       rb = animator.GetComponent<Rigidbody2D>();
       boss = animator.GetComponent<ReSmileFllip>();
      // anim = animator.GetComponent<Animator>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
       boss.LookAtPlayer();
       Vector2 target = new Vector2(player.position.x , rb.position.y);
       Vector2 newPos = Vector2.MoveTowards(rb.position , target , Speed * Time.fixedDeltaTime);
       rb.MovePosition(newPos);

      if (Vector2.Distance(player.position , rb.position ) <= attackRange){
         // if(playerHealth.currentHealth > 0){
         animator.SetTrigger("Attack");
   //   }

      // if(playerHealth.currentHealth <= 0){
      //   anim.SetBool("Idle" , true);
      // }
      
    }
      }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

   
}
