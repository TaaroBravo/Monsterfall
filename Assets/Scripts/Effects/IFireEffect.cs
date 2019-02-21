using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFireEffect : IEffect
{
    public float dmg;
    public float maxTime;
    public float delayTime;

    public IFireEffect(float dmg, float maxTime, float delayTime)
    {
        this.dmg = dmg;
        this.maxTime = maxTime;
        this.delayTime = delayTime;
    }

    public void Effect(PlayerController player)
    {
        if (!player)
            return;
        if (!player.GetComponent<FireParticle>().fire1.isPlaying)
        {
            player.GetComponent<FireParticle>().fire1.Play();
            player.GetComponent<FireParticle>().fire2.Play();
        }
        player.SetDamage(dmg);
    }

    public void DisableEffect(PlayerController player)
    {
        player.onFire = false;
        player.GetComponent<FireParticle>().fire1.Stop();
        player.GetComponent<FireParticle>().fire2.Stop();
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
