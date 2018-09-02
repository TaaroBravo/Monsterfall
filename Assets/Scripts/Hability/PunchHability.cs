using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PunchHability : IHability
{

    public PunchHability(PlayerController p, float _timerCoolDown = 0)
    {
        player = p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            player.usingHability = true;
        }
    }
}
