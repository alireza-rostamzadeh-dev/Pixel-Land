using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPoint : MonoBehaviour
{
    public GameObject wall_1;
    public GameObject wall_2;
    void OnTriggerEnter2D(Collider2D collision)
    {   
        Debug.Log("Triggered with: " + collision.name);
        wall_1.SetActive(true);
        wall_2.SetActive(true);
    }
}
