using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoerUp : MonoBehaviour
{   
    public int newHealth;

    void OnTriggerEnter2D(Collider2D other){    

        if (other.CompareTag("Player")){
            Pickup(other);
        }


    }


   void Pickup(Collider2D player){

    PlayerDeath test = player.GetComponent<PlayerDeath>();
    test.currentHealth += newHealth;

    Destroy(gameObject);

   }
}
