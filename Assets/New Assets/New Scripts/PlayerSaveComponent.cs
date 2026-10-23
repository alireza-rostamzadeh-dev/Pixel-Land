using UnityEngine;

public class PlayerSaveComponent : SaveComponent
{
    private PlayerStats stats;
    private PlayerItems items;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        items = GetComponent<PlayerItems>();
    }

    public override void Save(GameState state)
    {
        if (stats != null)
        {
            state.playerPosition = transform.position;
            state.currentHealth = stats.currentHealth;
            state.currentMana = stats.currentMana;
        }

        if (items != null)
        {
            state.healthPotions = items.CurrentHealthPotions;
            state.manaPotions = items.CurrentManaPotions;
        }
    }

    public override void Load(GameState state)
    {
        if (stats != null)
        {
            transform.position = state.playerPosition;

            stats.currentHealth =
                Mathf.Clamp(state.currentHealth, 0f, stats.maxHealth);

            stats.currentMana =
                Mathf.Clamp(state.currentMana, 0f, stats.maxMana);
        }

        if (items != null)
        {
            items.SetPotionCounts(
                state.healthPotions,
                state.manaPotions);
        }
    }
}