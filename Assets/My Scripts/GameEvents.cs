using System;
public static class GameEvents
{
    public static event Action ONLevelComplete;
    public static event Action ONCollidedWithObstacles;
    public static event Action ONLevelContinue;
    public static event Action OnNewRingUnlock;

    public static void LevelComplete()
    {
        ONLevelComplete?.Invoke();
    }
    public static void CollidedWithObstacles()
    {
        ONCollidedWithObstacles?.Invoke();
    }
    
    public static void LevelContinue()
    {
        ONLevelContinue?.Invoke();
    }

    public static void NewRingUnlock()
    {
        OnNewRingUnlock?.Invoke();
    }
}
