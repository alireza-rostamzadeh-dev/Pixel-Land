using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickyplatform : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision){

        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }

    }   

    
    private void OnCollisionExit2D(Collision2D collision){

        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(null);
            Invoke("DestroyPlat",5f);
        }

    }
    private void DestroyPlat(){
        Destroy(gameObject);
    }

}
