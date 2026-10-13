using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{


   private void OnTriggerEnter2D (Collider2D collision){

        if(collision.gameObject.name == "Player"){
           Invoke ("CompleteLevel" , 2f);
           CompleteLevel();
        }
   }
    private void CompleteLevel(){
        if (1 > SceneManager.GetActiveScene().buildIndex && SceneManager.GetActiveScene().buildIndex >= 0){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    else if (SceneManager.GetActiveScene().buildIndex == 1){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}

}