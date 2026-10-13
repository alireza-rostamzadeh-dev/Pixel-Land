using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    //Gold Variable cant be Static because we cant accest it from other scripts if u want to acces it u need to remove the static type
   public static int Gold = 0;
   [SerializeField] private Text goldtext ;
    private void OnTriggerEnter2D(Collider2D collision){

        if(collision.gameObject.CompareTag("Money")){

        Destroy(collision.gameObject);
        Gold = Gold + 100;
        goldtext.text = "Gold:" + Gold;
        
        }
        if(collision.gameObject.CompareTag("Coin")){

        Destroy(collision.gameObject);
        Gold = Gold + 50;
        goldtext.text = "Gold:" + Gold;
        
        }
        }
}
