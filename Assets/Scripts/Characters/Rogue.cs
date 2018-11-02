using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rogue : PlayerController
{
    public Immobilizer immobilizer;
    public float immobilizerCooldown;

    public Collider hitArea;
    public float speedOfRogueDash;
    public float powerOfDash;
    public float dashingTime;
    public float rogueDashCooldown;

    public ParticleSystem ps_DashRogue;
    IEffect markEffect;

    bool enter;

    public override void Start()
    {
        base.Start();
        markEffect = new IRogueMark(this, 7, 8);
        SetAttacks();
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
        hability["ImmobilizerTrapHability"].Hability();
    }

    void MovementHability()
    {
        hability["RogueDashHability"].Hability();
    }

    private void SetAttacks()
    {
        attacks.Add(typeof(NormalAttack).ToString(), new NormalAttack(this, markEffect, normalAttackCoolDown));
        attacks.Add(typeof(UpAttack).ToString(), new UpAttack(this, markEffect, upAttackCoolDown));
        attacks.Add(typeof(DownAttack).ToString(), new DownAttack(this, markEffect, downAttackCoolDown));
    }

    void SetHabilities()
    {
        hability.Add(typeof(ImmobilizerTrapHability).ToString(), new ImmobilizerTrapHability(this, immobilizer, 3f));
        hability.Add(typeof(RogueDashHability).ToString(), new RogueDashHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), hitArea, ps_DashRogue, powerOfDash, dashingTime, speedOfRogueDash, rogueDashCooldown));
        myHability = RogueHability;
        movementHability = MovementHability;
    }
}
