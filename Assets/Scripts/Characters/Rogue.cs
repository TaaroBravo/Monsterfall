using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Rogue : PlayerController
{
    public event Action OnTeleportRogue = delegate { };

    public Immobilizer immobilizer;
    public float immobilizerCooldown;

    public Collider hitArea;
    public float speedOfRogueDash;
    public float powerOfDash;
    public float dashingTime;
    public float rogueDashCooldown;

    public ParticleSystem ps_DashRogue;
    IEffect markEffect;
    public bool onMark;

    bool enter;

    public override void Start()
    {
        base.Start();
        markEffect = new IRogueMark(this, 10, 8);
        lifeHUD.Set(3, rogueDashCooldown/2, myLife);
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

    public void CooldownMark(Action<PlayerController> callback)
    {
        if (!onMark)
            StartCoroutine(CooldownMarkCoroutine(callback));
        else
        {
            StopCoroutine(CooldownMarkCoroutine(callback));
            StartCoroutine(CooldownMarkCoroutine(callback));
        }
    }

    IEnumerator CooldownMarkCoroutine(Action<PlayerController> callback)
    {
        while (true)
        {
            onMark = true;
            yield return new WaitForSeconds(10f);
            callback(this);
            onMark = false;
            break;
        }
    }

    void RogueHability(string state)
    {
        if (state == "Realese")
        {
            hability["ImmobilizerTrapHability"].Release();
        }
        else
        {
            hability["ImmobilizerTrapHability"].Hability();
        }
    }

    void MovementHability(string state)
    {
        hability["RogueDashHability"].Hability();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DoorWarp"))
        {
            canJump = false;
            coyoteBool = false;
            OnTeleportRogue();
        }
    }

    private void SetAttacks()
    {
        attacks.Add(typeof(NormalAttack).ToString(), new NormalAttack(this, markEffect, normalAttackCoolDown));
        attacks.Add(typeof(UpAttack).ToString(), new UpAttack(this, markEffect, upAttackCoolDown));
        attacks.Add(typeof(DownAttack).ToString(), new DownAttack(this, markEffect, downAttackCoolDown));
    }

    void SetHabilities()
    {
        hability.Add(typeof(ImmobilizerTrapHability).ToString(), new ImmobilizerTrapHability(this, immobilizer, 6f));
        hability.Add(typeof(RogueDashHability).ToString(), new RogueDashHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), hitArea, ps_DashRogue, powerOfDash, dashingTime, speedOfRogueDash, rogueDashCooldown));
        myHability = RogueHability;
        movementHability = MovementHability;
    }
}
