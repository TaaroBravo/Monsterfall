﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Knight : PlayerController {

    public float punchCooldown;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    void KnightHability()
    {
        hability["PunchHability"].Hability();
    }

    void SetHabilities()
    {
        myHability = KnightHability;
        hability.Add(typeof(PunchHability).ToString(), new PunchHability(this, punchCooldown));
    }
}
