using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBerserkPunch : IEffect
{
    Berserk _player;
    int damage;

    public IBerserkPunch(Berserk player, int damage)
    {
        _player = player;
        this.damage = damage;
    }

    public void Effect(PlayerController player)
    {
        player.SetDamage(damage * _player.buffedPower);
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
