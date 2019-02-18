using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Yeti : PlayerController
{

    public IcePlatform icePlatfromPrefab;
    public IceSpikes spikesPrefab;
    public Ice icePrefab;
    public ParticleSystem explotePS;

    IEffect iceEffect;
    public bool onMark;

    public List<PlayerController> frozenCharacter = new List<PlayerController>();

    public override void Start()
    {
        base.Start();
        iceEffect = new IIceEffect(this, 4, icePrefab);
        lifeHUD.Set(11, 5, myLife);
        SetAttacks();
        SetHabilities();
    }

    public override void Update()
    {
        base.Update();
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
        onMark = true;
        yield return new WaitForSeconds(10f);
        callback(this);
        onMark = false;
    }

    void YetiAbility(string state)
    {
        if (state == "Realese")
        {
            hability["IceSpikesHability"].Release();
        }
        else
        {
            hability["IceSpikesHability"].Hability();
        }
    }

    void YetiMovementAbility(string state)
    {
        if (state == "Realese")
        {
            hability["IcePlatformHability"].Release();
        }
        else
        {
            hability["IcePlatformHability"].Hability();
        }
    }

    private void SetAttacks()
    {
        attacks.Add(typeof(NormalAttack).ToString(), new NormalAttack(this, iceEffect, normalAttackCoolDown));
        attacks.Add(typeof(UpAttack).ToString(), new UpAttack(this, iceEffect, upAttackCoolDown));
        attacks.Add(typeof(DownAttack).ToString(), new DownAttack(this, iceEffect, downAttackCoolDown));
    }

    void SetHabilities()
    {
        hability.Add(typeof(IceSpikesHability).ToString(), new IceSpikesHability(this, spikesPrefab, 11f));
        hability.Add(typeof(IcePlatformHability).ToString(), new IcePlatformHability(this, icePlatfromPrefab, 5f));
        myHability = YetiAbility;
        movementHability = YetiMovementAbility;
    }
}
