using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : IAttack
{
    ParticleSystem ps;
    public NormalAttack(PlayerController pl, float _timerCoolDown = 0)
    {
        player = pl;
        timerCoolDownAttack = _timerCoolDown;
        coolDownAttack = _timerCoolDown;
        weaponExtends = player.weaponExtends;
        impactVelocity = player.impactVelocityNormal;
        defaultAttack = player.defaultAttackNormal;
        influenceOfMovement = player.influenceOfMovementNormal;
        chargedEffect = player.chargedEffect;
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
            player.myAnim.SetBool("ReleaseAForward", true);
            if (player.myAnim.GetBool("Grounded"))
                player.myAnim.Play("AttackForward");
            else
                player.myAnim.Play("HitForwardAir");

            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents * 2, col.transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
                PlayerController target = TargetScript(c.transform);
                player.hitParticles.Play();
                if (target != null)
                {
                    target.ReceiveDamage(new Vector3(Mathf.Sign(player.transform.localScale.z) * CalculateImpact(currentPressed * 2), 0, 0), currentPressed >= maxPressed);
                    ps.transform.up = -Vector3.right * player.transform.localScale.z;
                    ps.Play();
                    target.WhoHitedMe(player);
                    player.whoIHited = target;
                    player.myAnim.SetBool("ReleaseAForward", false);
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
        player.myAnim.SetBool("ReleaseAForward", false);
        player.myAnim.Play("ChargingAForward");
        isPressing = true;
    }
}
