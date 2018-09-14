using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Knight : PlayerController {

    public Punch punch;
    public float powerOfPunch;
    public float activeTime;
    public float punchCooldown;

    public ParticleSystem ps_Hability;

    public override void Start()
    {
        base.Start();
        SetHabilities();
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
        hability.Add(typeof(PunchHability).ToString(), new PunchHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), ps_Hability, punch, powerOfPunch, activeTime, punchCooldown));
    }
}
