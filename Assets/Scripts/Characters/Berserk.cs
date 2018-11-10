using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Berserk : PlayerController
{
    public event Action OnStompHability = delegate { };

    IEffect superPunch;
    public float power;
    public float stompCooldown;

    public float chargeSpeed;
    public float chargeCooldown;
    public bool recovery;

    public override void Start()
    {
        base.Start();
        superPunch = new IBerserkPunch(this, 45);
        lifeHUD.Set(3, chargeCooldown, myLife);
        SetAttacks();
        SetHabilities();
    }

    public override void Update()
    {
        base.Update();
    }

    void BerserkHability()
    {
        if (controller.isGrounded)
            hability["StompHability"].Hability();
    }

    void MovementHability()
    {
        hability["ForwardCharge"].Hability();
    }

    public void HabilityRecovery()
    {
        recovery = true;
    }

    private void SetAttacks()
    {
        attacks.Add(typeof(NormalAttack).ToString(), new NormalAttack(this, superPunch, normalAttackCoolDown));
        attacks.Add(typeof(UpAttack).ToString(), new UpAttack(this, superPunch, upAttackCoolDown));
        attacks.Add(typeof(DownAttack).ToString(), new DownAttack(this, superPunch, downAttackCoolDown));
    }

    void SetHabilities()
    {
        hability.Add(typeof(StompHability).ToString(), new StompHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), power, 3f));
        hability.Add(typeof(ForwardCharge).ToString(), new ForwardCharge(this, chargeSpeed, chargeCooldown));
        myHability = BerserkHability;
        movementHability = MovementHability;
    }

    public void BerserkStompHability()
    {
        //si no estoy stuneado.
        if (!stunnedByHit)
            OnStompHability();
    }
}
