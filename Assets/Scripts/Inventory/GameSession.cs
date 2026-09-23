using UnityEngine;

public static class GameSession
{
    public static InventoryState Inventory { get; private set; } = new InventoryState();

    public static void StartNewRun()
    {
        Inventory = new InventoryState();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlayModeEnter()
    {
        StartNewRun();
    }
}