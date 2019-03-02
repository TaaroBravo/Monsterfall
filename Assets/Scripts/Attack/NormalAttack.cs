﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NormalAttack : IAttack
{
    ParticleSystem ps;
    GameObject myAudioClip;
    MultipleTargetCamera cam;
    public NormalAttack(PlayerController pl, IEffect _effect = null, float _timerCoolDown = 0)
    {
        player = pl;
        effect = _effect;
        timerCoolDownAttack = _timerCoolDown;
        coolDownAttack = _timerCoolDown;
        weaponExtends = player.weaponExtends;
        impactVelocity = player.impactVelocityNormal;
        defaultAttack = player.defaultAttackNormal;
        influenceOfMovement = player.influenceOfMovementNormal;
        chargedEffect = player.chargedEffect;
        currentPressed = 1;
        maxPressed = 2.5f;
        minImpact = 17;
        cam = GameObject.FindObjectOfType<MultipleTargetCamera>();
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
            //ps = player.PS_Impact;
            player.canMove = true;
            AudioManager.Instance.CreateSound("Punch");
            player.GetComponent<GeneralFeedback>().PlaySlash();
            AudioManager.Instance.FadeOut(myAudioClip, 0.1f);
            player.myAnim.SetBool("RealeaseAForward", true);
            player.myAnim.SetTrigger("ReleaseAForward");
            if (player.myAnim.GetBool("Grounded"))
                player.myAnim.Play("AttackForward");
            else
                player.myAnim.Play("HitForwardAir");
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents * 2);
            List<PlayerController> hitPlayers = new List<PlayerController>();
            //Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents * 2, col.transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (c.GetComponent<Ice>())
                    c.GetComponent<Ice>().DestroyObject();
                if (c.transform.gameObject.layer == LayerMask.GetMask("Hitbox"))
                    continue;
                if (CheckParently(c.transform))
                    continue;
                PlayerController target = TargetScript(c.transform);
                if (target == player)
                    continue;
                ps = player.PS_Impact;
                if (target != null && !hitPlayers.Contains(target))
                {
                    cam.StartShaking();
                    AudioManager.Instance.CreateSound("HitPlayer");
                    player.hitParticles.Play();
                    hitPlayers.Add(target);
                    if (player.GetComponent<BerserkerParticlesManager>()) player.GetComponent<BerserkerParticlesManager>().PlayAttackParticle();
                    if (!(player is Berserk))
                        target.ReceiveImpact(new Vector3(Mathf.Sign(player.transform.localScale.z) * CalculateImpact(currentPressed * 2), 0, 0), player, currentPressed >= maxPressed);
                    else
                        target.ReceiveImpact(new Vector3(Mathf.Sign(player.transform.localScale.z) * CalculateImpact(3), 0, 0), player, false, true);

                    if (!(player is Rogue) && !(player is Berserk) && !(player is Pirate) && !(player is Elf))
                    {
                        if (player is Yeti && ((Yeti)player).frozenCharacter.Contains(target))
                        {
                            target.SetDamage(20 * player.buffedPower);
                        }
                        else
                            target.SetDamage(10 * player.buffedPower);
                        target.ApplyEffect(effect);
                    }
                    else if (player is Pirate)
                    {
                        target.SetDamage(14 * player.buffedPower);
                        target.ApplyEffect(effect);
                    }
                    else if (player is Elf)
                    {
                        target.SetDamage(14 * player.buffedPower);
                        target.ApplyEffect(effect);
                    }
                    else
                        target.ApplyEffect(effect);
                    ps.transform.up = -Vector3.right * player.transform.localScale.z;
                    ps.Play();
                    target.WhoHitedMe(player);
                    player.whoIHited = target;
                    player.myAnim.SetBool("ReleaseAForward", false);
                }
            }
        }
        isPressing = false;
        currentPressed = 1;
        timerCoolDownAttack = coolDownAttack;
        player.PS_Charged.Stop();
        player.myAnim.ResetTrigger("ReleaseAForward");
    }

    public override void Pressed()
    {
        player.myAnim.SetBool("ReleaseAForward", false);
        if (!isPressing)
            player.myAnim.Play("ChargingAForward");
        myAudioClip = AudioManager.Instance.CreateSound("ChargingAttack", 100);
        isPressing = true;
        player.canMove = false;
    }
}
