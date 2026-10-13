using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   public Slider slider;
   public Gradient gradient;
   public Image fill;

   public void SetMaxHealth(int health){
    slider.maxValue = health;
    slider.value = health;
    //the number of gradient goes from 0 to 1
   fill.color = gradient.Evaluate(1f);
   }

   public void SetHealth(int health){
    slider.value = health;
    //cause our value goes higher than 1 we need to use this code for gradient number
    fill.color = gradient.Evaluate(slider.normalizedValue);
   }







}
