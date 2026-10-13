using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemCollect : MonoBehaviour
{
private int Gold;
[SerializeField] private TextMeshProUGUI Gold_Text;

   void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.gameObject.CompareTag("Coin"))
    {
        
        CoinSound cs = collision.GetComponent<CoinSound>();
        if (cs != null)
            cs.PlaySound(); 

        
        Destroy(collision.gameObject, 0.2f);

        Gold += 50;
        Gold_Text.text = "" + Gold;
    }
}



}
