using System;
using UnityEngine;

[Serializable]
public class GameState
{
    public string currentScene;
    public Vector3 playerPosition;
    public float currentHealth;
    public float currentMana;
    public int gold;
    public int healthPotions;
    public int manaPotions;

    public GameState()
    {
        currentScene = "";
        playerPosition = Vector3.zero;
        currentHealth = 500f;
        gold = 0;
        healthPotions = 2;
    }
}
