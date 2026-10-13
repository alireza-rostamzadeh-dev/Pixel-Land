using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBars : MonoBehaviour
{
    public Slider health_slider;
    public Slider mana_slider;

    void Start()
{
    if (mana_slider == null)
        Debug.LogError("Mana Slider is not assigned in the inspector!");
}


    public void SetMaxHealth(float health)
    {

        health_slider.maxValue = health;
        health_slider.value = health;
    }

    public void SetHealth(float health)
    {

        health_slider.value = health;
    }

    public void SetMaxMana(float mana)
    {

        mana_slider.maxValue = mana;
        mana_slider.value = mana;
    }
    public void SetMana(float mana)
    {

        mana_slider.value = mana;
    }
    
}
