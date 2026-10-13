using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour
{
    public int maxHealthPotions = 2;
    public int maxManaPotions = 2;

    private int currentHealthPotions;
    private int currentManaPotions;

    public Image healthPotionImage;
    public Sprite healthPotionFullSprite;
    public Sprite healthPotionEmptySprite;

    public Image manaPotionImage;
    public Sprite manaPotionFullSprite;
    public Sprite manaPotionEmptySprite;

    private PlayerStats stats;
    public StatsBars statsBar;
    public TextMeshProUGUI healthPotionNum;  
    public TextMeshProUGUI manaPotionNum;

    void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        statsBar = FindObjectOfType<StatsBars>();
        currentHealthPotions = maxHealthPotions;
        currentManaPotions = maxManaPotions;
        healthPotionNum.text = currentHealthPotions.ToString();
        manaPotionNum.text = currentManaPotions.ToString();
        UpdatePotionUI();
        
    }



public void UseHealthPotion()
{
    if (currentHealthPotions > 0)
    {
        currentHealthPotions--;
        stats.currentHealth += 250f;
        statsBar.SetHealth(stats.currentHealth);
        
        
            if (stats.currentHealth > stats.maxHealth)
                stats.currentHealth = stats.maxHealth;

        UpdatePotionUI();

        
        
    }
}



public void UseManaPotion()
{
    if (currentManaPotions > 0)
    {
        currentManaPotions--;
        stats.currentMana += 100f;
        statsBar.SetMana(stats.currentMana);
        
        if (stats.currentMana > stats.maxMana)
                stats.currentMana = stats.maxMana;

        UpdatePotionUI();
        
    }
}



    public void ResetPotions()
    {
        currentHealthPotions = maxHealthPotions;
        currentManaPotions = maxManaPotions;
        UpdatePotionUI();
    }

    private void UpdatePotionUI()
    {
        manaPotionNum.text = currentManaPotions.ToString();
        healthPotionNum.text = currentHealthPotions.ToString();

        if (currentHealthPotions > 0)
            healthPotionImage.sprite = healthPotionFullSprite;
        else
            healthPotionImage.sprite = healthPotionEmptySprite;

        
        if (currentManaPotions > 0)
            manaPotionImage.sprite = manaPotionFullSprite;
        else
            manaPotionImage.sprite = manaPotionEmptySprite;
    }
}
