using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Berserk : PlayerController
{
    public event Action OnStompHability = delegate { };

    public float power;
    public float stompCooldown;

    public float chargeSpeed;
    public float chargeCooldown;

    public override void Start()
    {
        base.Start();
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

    void SetHabilities()
    {
        hability.Add(typeof(StompHability).ToString(), new StompHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), power, stompCooldown));
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
