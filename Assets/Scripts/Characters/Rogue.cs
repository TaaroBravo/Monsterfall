using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rogue : PlayerController
{

    public Collider hitArea;
    public float speedOfRogueDash;
    public float powerOfDash;
    public float dashingTime;
    public float rogueDashCooldown;

    public ParticleSystem ps_DashRogue;

    bool enter;

    public override void Start()
    {
        base.Start();
        SetHabilities();
        enter = true;
    }

    public override void Update()
    {
        base.Update();
        foreach (var h in hability.Values)
            h.Update();

        if (GameManager.Instance.finishedGame && enter)
        {
            enter = false;
            transform.Rotate(0, -90, 0);
        }
    }

    void RogueHability()
    {
        hability["RogueDashHability"].Hability();
    }

    void SetHabilities()
    {
        myHability = RogueHability;
        hability.Add(typeof(RogueDashHability).ToString(), new RogueDashHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), hitArea, ps_DashRogue, powerOfDash, dashingTime, speedOfRogueDash, rogueDashCooldown));
    }
}
