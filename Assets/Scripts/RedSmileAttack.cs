using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class RedSmileAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    private Animator anim;
    public int attackDamage = 40;
    public LayerMask playerLayers;
    public int maxHealth;
    private int currentHealth;
    private PlayerDeath playerHealth;


    void Start()
       {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;   
       }

    void Update(){
        Attack(); 
        // Death();
        
    }


     void Attack(){
        anim.SetTrigger("Attack");
        Collider2D[] hitplayer = Physics2D.OverlapCircleAll(attackPoint.position , attackRange , playerLayers);

        foreach (Collider2D player in hitplayer)
        {
            // Debug.Log("We hit " + enemy.name);
           TakeDamage(attackDamage);
        }

        void TakeDamage(int damage){
        currentHealth -= damage;

        // if (currentHealth <= 0){
        //     Die();
        }
    }

    private void OnDrawGizmosSelected(){
        if (attackPoint == null)
            return;

    Gizmos.DrawWireSphere(attackPoint.position , attackRange);
    }

    // void Death(){
    //     if (playerHealth.currentHealth <= 0){
    //       Destroy(gameObject);
    //   }
    // }

    }













