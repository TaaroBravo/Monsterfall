using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : PlayerController {

    public Collider hitArea;
    public float speedOfRogueDash;
    public float powerOfDash;
    public float dashingTime;
    public float rogueDashCooldown;

    public override void Start()
    {
        base.Start();
        SetHabilities();
    }

    public override void Update()
    {
        base.Update();
    }

    void RogueHability()
    {
        hability["RogueDashHability"].Hability();
    }

    void SetHabilities()
    {
        myHability = RogueHability;
        hability.Add(typeof(RogueDashHability).ToString(), new RogueDashHability(this, hitArea, powerOfDash, dashingTime, speedOfRogueDash, rogueDashCooldown));
    }
}
