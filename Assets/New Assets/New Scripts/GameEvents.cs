
using System;
using UnityEngine;

public static class GameEvents
{
    public static Action GameStart = delegate { };

    public static Action<GameObject, Vector2, float> OnKnockbackRequested = delegate { };

    public static Action<float, float> OnHealthChanged = delegate { };
    public static Action<float> OnPlayerDamaged = delegate { };
    public static Action OnPlayerDied = delegate { };
    public static Action OnPlayerRespawned = delegate { };

    public static Action<int> OnCoinsCollected = delegate { };
    public static Action<GameObject, int> OnItemCollected = delegate { };
    public static Action<int> OnPotionUsed = delegate { };
    public static Action<int> OnItemCountChanged = delegate { };

    public static Action<GameObject, int> OnEnemyDamaged = delegate { };
    public static Action<GameObject> OnEnemyDied = delegate { };

    public static Action OnGamePaused = delegate { };
    public static Action OnGameResumed = delegate { };
    public static Action OnGameOver = delegate { };
    public static Action OnLevelCompleted = delegate { };

    public static Action<GameObject> OnCampfireActivated = delegate { };

    public static Action OnSceneLoaded = delegate { };

    public static void HealthChanged(float currentHealth, float maxHealth)
    {
        OnHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public static void RequestKnockback(
    GameObject target,
    Vector2 attackerPosition,
    float force)
{
    OnKnockbackRequested.Invoke(target, attackerPosition, force);
}
}
