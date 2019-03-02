using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Berserk : PlayerController
{
    public event Action OnStompHability = delegate { };
    public event Action OnRecovery = delegate { };

    IEffect superPunch;
    public float power;
    public float stompCooldown;

    public float chargeSpeed;
    public float chargeCooldown;
    public bool chargeAttack;
    public bool recovery;

    public bool usingBearHability;
    public bool usingChargeHability;

    public override void Start()
    {
        base.Start();
        superPunch = new IBerserkPunch(this, 14);
        lifeHUD.Set(8, chargeCooldown + 2f, myLife);
        SetAttacks();
        SetHabilities();
        StartCoroutine(ResetMovement());
    }

    public override void ResetAll()
    {
        base.ResetAll();
        chargeAttack = false;
        usingBearHability = false;
        usingChargeHability = false;
        recovery = true;
    }

    public override void Update()
    {
        base.Update();
    }

    IEnumerator ResetMovement()
    {
        while (true)
        {
            yield return new WaitUntil(() => stunnedByHit);
            yield return new WaitForSeconds(3f);
            if (stunnedByHit)
            {
                DisableAll();
                myAnim.SetBool("Stunned", false);
                stunnedByHit = false;
                canInteract = true;
            }
        }
    }

    void BerserkHability(string state)
    {
        if (controller.isGrounded)
        {
            if (state == "Realese")
            {
                hability["BearAttackHability"].Release();
            }
            else
            {
                hability["BearAttackHability"].Hability();
            }
        }
    }

    void MovementHability(string state)
    {
        hability["ForwardCharge"].Hability();
    }

    public void HabilityRecovery()
    {
        recovery = true;
        usingBearHability = false;
        OnRecovery();
    }

    private void SetAttacks()
    {
        attacks.Add(typeof(NormalAttack).ToString(), new NormalAttack(this, superPunch, normalAttackCoolDown));
        attacks.Add(typeof(UpAttack).ToString(), new UpAttack(this, superPunch, upAttackCoolDown));
        attacks.Add(typeof(DownAttack).ToString(), new DownAttack(this, superPunch, downAttackCoolDown));
    }

    void SetHabilities()
    {
        hability.Add(typeof(BearAttackHability).ToString(), new BearAttackHability(this, 15, 5f, 1.4f, 5f));
        hability.Add(typeof(ForwardCharge).ToString(), new ForwardCharge(this, chargeSpeed, chargeCooldown));
        myHability = BerserkHability;
        movementHability = MovementHability;
    }

    public void BerserkStompHability()
    {

    }
}
