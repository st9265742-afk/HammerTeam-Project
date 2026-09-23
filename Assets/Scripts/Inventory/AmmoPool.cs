using System;
using System.Collections.Generic;

public class AmmoPool
{
    private readonly Dictionary<AmmoType, int> _ammo = new Dictionary<AmmoType, int>();

    public int Get(AmmoType type)
    {
        return _ammo.TryGetValue(type, out int amount) ? amount : 0;
    }

    public void Add(AmmoType type, int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        _ammo[type] = Get(type) + amount;
    }

    public int Take(AmmoType type, int requested)
    {
        int taken = Math.Min(requested, Get(type));

        if (taken <= 0)
        {
            return 0;
        }

        _ammo[type] -= taken;
        return taken;
    }
}