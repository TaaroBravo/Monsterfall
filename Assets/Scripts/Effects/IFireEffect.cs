using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFireEffect : IEffect
{
    public int dmg;
    public float maxTime;
    public float delayTime;

    public IFireEffect(int dmg, float maxTime, float delayTime)
    {
        this.dmg = dmg;
        this.maxTime = maxTime;
        this.delayTime = delayTime;
    }

    public void Effect(PlayerController player)
    {
        player.SetDamage(dmg);
    }

    public float GetMaxTimer()
    {
        return maxTime;
    }

    public float GetDelayTimer()
    {
        return delayTime;
    }
}
