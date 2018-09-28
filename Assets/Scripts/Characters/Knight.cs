using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Knight : PlayerController {

    public float powerOfPunch;
    public float punchCooldown;

    public ParticleSystem ps_Hability;

    public float forcedJumpForce;
    public float forcedJumpCooldown;

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

    void MovementHability()
    {
        hability["ForcedJump"].Hability();
    }

    void SetHabilities()
    {
        hability.Add(typeof(PunchHability).ToString(), new PunchHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), ps_Hability, powerOfPunch, punchCooldown));
        hability.Add(typeof(ForcedJump).ToString(), new ForcedJump(this, forcedJumpForce, forcedJumpCooldown));
        myHability = KnightHability;
        movementHability = MovementHability;
    }
}
