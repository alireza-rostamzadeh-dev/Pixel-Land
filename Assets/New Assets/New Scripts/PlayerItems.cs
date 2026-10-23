
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour
{
    public const int HealthPotionId = 0;
    public const int ManaPotionId = 1;

    public int maxHealthPotions = 2;
    public int maxManaPotions = 2;

    [SerializeField] private int currentHealthPotions;
    [SerializeField] private int currentManaPotions;

    public int CurrentHealthPotions => currentHealthPotions;
    public int CurrentManaPotions => currentManaPotions;

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

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();

        if (statsBar == null)
            statsBar = FindObjectOfType<StatsBars>();

        if (currentHealthPotions == 0 && currentManaPotions == 0)
        {
            currentHealthPotions = maxHealthPotions;
            currentManaPotions = maxManaPotions;
        }

        ClampPotionCounts();
        UpdatePotionUI();
    }

    public void UseHealthPotion()
    {
        if (stats == null)
            return;

        if (currentHealthPotions <= 0)
            return;

        if (stats.currentHealth >= stats.maxHealth)
            return;

        currentHealthPotions--;

        stats.currentHealth = Mathf.Min(
            stats.currentHealth + 250f,
            stats.maxHealth
        );

        GameEvents.HealthChanged(
            stats.currentHealth,
            stats.maxHealth
        );

        GameEvents.OnPotionUsed.Invoke(HealthPotionId);
        GameEvents.OnItemCountChanged.Invoke(currentHealthPotions);

        UpdatePotionUI();
    }

    public void UseManaPotion()
    {
        if (stats == null)
            return;

        if (currentManaPotions <= 0)
            return;

        if (stats.currentMana >= stats.maxMana)
            return;

        currentManaPotions--;

        stats.currentMana = Mathf.Min(
            stats.currentMana + 100f,
            stats.maxMana
        );

        GameEvents.OnPotionUsed.Invoke(ManaPotionId);
        GameEvents.OnItemCountChanged.Invoke(currentManaPotions);

        UpdatePotionUI();
    }

    public void ResetPotions()
    {
        currentHealthPotions = maxHealthPotions;
        currentManaPotions = maxManaPotions;

        ClampPotionCounts();
        UpdatePotionUI();

        GameEvents.OnItemCountChanged.Invoke(currentHealthPotions);
        GameEvents.OnItemCountChanged.Invoke(currentManaPotions);
    }

    public void SetPotionCounts(int health, int mana)
    {
        currentHealthPotions = health;
        currentManaPotions = mana;

        ClampPotionCounts();
        UpdatePotionUI();

        GameEvents.OnItemCountChanged.Invoke(currentHealthPotions);
        GameEvents.OnItemCountChanged.Invoke(currentManaPotions);
    }

    private void ClampPotionCounts()
    {
        currentHealthPotions = Mathf.Clamp(
            currentHealthPotions,
            0,
            maxHealthPotions
        );

        currentManaPotions = Mathf.Clamp(
            currentManaPotions,
            0,
            maxManaPotions
        );
    }

    private void UpdatePotionUI()
    {
        if (healthPotionNum != null)
            healthPotionNum.text = currentHealthPotions.ToString();

        if (manaPotionNum != null)
            manaPotionNum.text = currentManaPotions.ToString();

        if (healthPotionImage != null)
        {
            healthPotionImage.sprite = currentHealthPotions > 0
                ? healthPotionFullSprite
                : healthPotionEmptySprite;
        }

        if (manaPotionImage != null)
        {
            manaPotionImage.sprite = currentManaPotions > 0
                ? manaPotionFullSprite
                : manaPotionEmptySprite;
        }
    }
}
