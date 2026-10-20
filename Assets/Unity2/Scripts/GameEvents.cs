using System;

public static class GameEvents
{
    public static event Action GameStart;

    public static void TriggerGameStart()
    {
        GameStart?.Invoke();
    }
}