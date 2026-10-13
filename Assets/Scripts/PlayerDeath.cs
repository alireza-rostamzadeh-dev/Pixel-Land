using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{   
    private Animator anim;
    private Rigidbody2D rb;
    private Transform tr;

    //private PlayerMovment disabler;
    
    public GameObject blood;

    public int maxHealth = 300;
    public int currentHealth = 300;

    public int TrapDamage = 50;

    public HealthBar healthbar;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
       // disabler = FindObjectOfType<PlayerMovment>(); 
        
        healthbar = FindObjectOfType<HealthBar>();

        currentHealth = maxHealth;   
        healthbar.SetMaxHealth(maxHealth);
    }

    void Update(){

       healthbar.SetHealth(currentHealth);
       Invoke("HealthRegeneration" , 5);


    }

    private void OnCollisionEnter2D(Collision2D collision){
    if (collision.gameObject.CompareTag("Trap"))
    {  
    
         TakeDamage(TrapDamage);
        

        if (currentHealth <= 0){
            Die();
        }
    }
    if (collision.gameObject.CompareTag("Red"))
    {  
    
         TakeDamage(TrapDamage);
        

        if (currentHealth <= 0){
            Die();
        }
    }


    if (collision.gameObject.CompareTag("DTrap"))
    {  
         Die();
    }
        
    }

    private void Die(){
        
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<PlayerMovment>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
       // anim.SetTrigger("death");
        anim.SetBool("death" , true);
        Instantiate(blood , transform.position , transform.rotation );
        Invoke("positionChange" , 0.47f);
        Invoke("RestartLevel" , 4f);
      
    }

    

    void positionChange(){
        tr.position = new Vector3 (transform.position.x, 7.5f , transform.position.z);
}
    private void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    
    public void TakeDamage(int Damage){
        currentHealth -= Damage;
        healthbar.SetHealth(currentHealth);
        anim.SetTrigger("hurt");
        GameObject clone = Instantiate(blood , transform.position , transform.rotation );

       //the second parameter of destroy is the time that u set for Destroying the game object
       Destroy(clone , 2f);
    }
    // void HealthRegeneration(){
    //     // StartCoroutine (Wait());
        

    //     if(currentHealth < maxHealth){
    //         currentHealth += 1;
            
    //     }
    // }
    // IEnumerator Wait(){
    //     yield return new WaitForSeconds(10);
    // }
    
}

