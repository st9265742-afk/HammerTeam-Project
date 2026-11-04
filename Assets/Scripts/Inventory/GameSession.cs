using UnityEngine;

public static class GameSession
{
    public static InventoryState Inventory { get; private set; } = new InventoryState();

    private static InventoryState _levelStart;

    public static void StartNewRun()
    {
        Inventory = new InventoryState();
        _levelStart = Inventory.Clone();
    }

    public static void CommitLevelStart()
    {
        _levelStart = Inventory.Clone();
    }

    public static void RestartLevel()
    {
        if (_levelStart == null)
        {
            StartNewRun();
            return;
        }

        Inventory = _levelStart.Clone();
    }
}