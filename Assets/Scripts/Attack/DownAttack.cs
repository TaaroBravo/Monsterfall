﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttack : IAttack
{
    ParticleSystem ps;
    public DownAttack(PlayerController pl, float _timerCoolDown = 0)
    {
        player = pl;
        timerCoolDownAttack = _timerCoolDown;
        coolDownAttack = _timerCoolDown;
        weaponExtends = player.weaponExtends;
        impactVelocity = player.impactVelocityDown;
        defaultAttack = player.defaultAttackDown;
        influenceOfMovement = player.influenceOfMovementDown;
        currentPressed = 1;
        maxPressed = 2.5f;
        minImpact = 30;
    }

    public override void Update()
    {
        base.Update();
        if (isPressing)
        {
            if (!player.PS_Charged.isPlaying)
                player.PS_Charged.Play();
            if (currentPressed <= maxPressed)
                currentPressed += Time.deltaTime * 2;
        }
    }

    public override void Attack(Collider col)
    {
        if (timerCoolDownAttack < 0)
        {
            ps = player.PS_Impact;
            player.myAnim.SetBool("ReleaseADown", true);
            if (player.myAnim.GetBool("Grounded"))
                player.myAnim.Play("AttackDown");
            else
                player.myAnim.Play("HitDownAir");

            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
                PlayerController target = TargetScript(c.transform);
                player.hitParticles.Play();
                if (target != null)
                {
                    target.ReceiveDamage(new Vector3(0, -CalculateImpact(currentPressed), 0), player, currentPressed >= maxPressed);
                    ps.transform.up = -Vector3.down;
                    ps.Play();
                    player.whoIHited = target;
                    player.myAnim.SetBool("ReleaseADown", true);
                }
            }
            isPressing = false;
            currentPressed = 1;
            timerCoolDownAttack = coolDownAttack;
            player.PS_Charged.Stop();
        }
    }

    public override void Pressed()
    {
        player.myAnim.SetBool("ReleaseADown", false);
        player.myAnim.Play("ChargingADown");
        isPressing = true;
    }
}
