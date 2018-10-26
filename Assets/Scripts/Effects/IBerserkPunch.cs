using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBerserkPunch : IEffect
{
    int damage;

    public IBerserkPunch(int damage)
    {
        this.damage = damage;
    }

    public void Effect(PlayerController player)
    {
        player.SetDamage(damage);
    }

    public void DisableEffect(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public float GetDelayTimer()
    {
        return 1;
    }

    public float GetMaxTimer()
    {
        return 1;
    }
}
