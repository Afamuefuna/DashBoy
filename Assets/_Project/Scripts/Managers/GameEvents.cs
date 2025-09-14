using System;

public static class GameEvents
{
    public static Action<int> OnScoreChanged;
    public static Action<int> OnPlayerDamaged;
    public static Action OnPlayerDied;
    public static Action<float> OnDashUsed;
    public static Action<string> OnPickupCollected;
}
