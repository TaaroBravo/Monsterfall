using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAttack : IAttack
{
    ParticleSystem ps;

    public UpAttack(PlayerController pl, IEffect _effect = null, float _timerCoolDown = 0)
    {
        player = pl;
        effect = _effect;
        timerCoolDownAttack = _timerCoolDown;
        coolDownAttack = _timerCoolDown;
        weaponExtends = player.weaponExtends;
        impactVelocity = player.impactVelocityUp;
        defaultAttack = player.defaultAttackUp;
        influenceOfMovement = player.influenceOfMovementUp;
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
            player.myAnim.SetBool("ReleaseAUp", true);
            if (player.myAnim.GetBool("Grounded"))
                player.myAnim.Play("AttackUp");
            else
                player.myAnim.Play("HitUpAir");
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents * 1.5f, col.transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
                PlayerController target = TargetScript(c.transform);
                player.hitParticles.Play();
                if (target != null)
                {
                    if (!(player is Berserk))
                        target.ReceiveImpact(new Vector3(0, CalculateImpact(currentPressed), 0), player, currentPressed >= maxPressed);
                    else
                        target.ReceiveImpact(new Vector3(0, CalculateImpact(currentPressed), 0), player, false, true);
                    
                    if (!(player is Rogue) && !(player is Berserk))
                    {
                        target.SetDamage(10);
                        target.ApplyEffect(effect);
                    }
                    else
                        target.ApplyEffect(effect);
                    ps.transform.up = -Vector3.up;
                    ps.Play();
                    player.whoIHited = target;
                    player.myAnim.SetBool("ReleaseAUp", true);
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
        player.myAnim.SetBool("ReleaseAUp", false);
        player.myAnim.Play("ChargingAUp");
        isPressing = true;
    }
}
