using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{   
    private PlayerMovment test;
    public int newSpeed;

    
    void OnTriggerEnter2D(Collider2D other){    

        if (other.CompareTag("Player")){
            StartCoroutine (Pickup(other));
        }


    }


   IEnumerator Pickup(Collider2D player){

    PlayerMovment test = player.GetComponent<PlayerMovment>();
    test.speed += newSpeed;

    GetComponent<SpriteRenderer>().enabled = false;
    GetComponent<Collider2D>().enabled = false;
    yield return new WaitForSeconds(10);
    test.speed -= newSpeed; 
    Destroy(gameObject);

   }




    
}
