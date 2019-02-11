using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttack : IAttack
{
    ParticleSystem ps;
    public DownAttack(PlayerController pl, IEffect _effect = null, float _timerCoolDown = 0)
    {
        player = pl;
        effect = _effect;
        timerCoolDownAttack = _timerCoolDown;
        coolDownAttack = _timerCoolDown;
        weaponExtends = player.weaponExtends;
        impactVelocity = player.impactVelocityDown;
        defaultAttack = player.defaultAttackDown;
        influenceOfMovement = player.influenceOfMovementDown;
        currentPressed = 1;
        maxPressed = 2.5f;
        minImpact = 17; //estaba en 30
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
            player.myAnim.SetBool("ReleaseADown", true);
            if (player.myAnim.GetBool("Grounded"))
                player.myAnim.Play("AttackDown");
            else
                player.myAnim.Play("HitDownAir");

            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents * 2);
            List<PlayerController> hitPlayers = new List<PlayerController>();
            //Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents * 2, col.transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
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
                    player.hitParticles.Play();
                    hitPlayers.Add(target);
                    if (player.GetComponent<BerserkerParticlesManager>()) player.GetComponent<BerserkerParticlesManager>().PlayAttackParticle();
                    if (!(player is Berserk))
                        target.ReceiveImpact(new Vector3(0, -CalculateImpact(currentPressed), 0), player, currentPressed >= maxPressed);
                    else
                        target.ReceiveImpact(new Vector3(0, -CalculateImpact(currentPressed), 0), player, false, true);

                    if (!(player is Rogue) && !(player is Berserk) && !(player is Pirate) && !(player is Elf))
                    {
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
        player.myAnim.ResetTrigger("ReleaseADown");
        }
    }

    public override void Pressed()
    {
        player.myAnim.SetBool("ReleaseADown", false);
        player.myAnim.Play("ChargingADown");
        isPressing = true;
    }
}
