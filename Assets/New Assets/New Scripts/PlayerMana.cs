using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    private PlayerStats stats;
    public StatsBars manaBar;
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        manaBar = FindObjectOfType<StatsBars>();
        manaBar.SetMaxMana(stats.maxMana);
    }

   
    void Update()
    {
        
    }
}
