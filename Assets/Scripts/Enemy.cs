using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public GameObject Money;
 
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;   
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;

        if (currentHealth <= 0){
            Die();
        }
    }

    void Die(){
        
        Debug.Log("Enemy Died"); 
        Destroy(gameObject);
        Instantiate(Money , transform.position , transform.rotation);
    }

}
