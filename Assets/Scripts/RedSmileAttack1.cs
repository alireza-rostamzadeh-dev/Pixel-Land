using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSmileAttack1 : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerDeath player;
    public int redAttack;

    void Attack(){
        player = FindObjectOfType<PlayerDeath>();
        player.TakeDamage(redAttack);
    }
}
