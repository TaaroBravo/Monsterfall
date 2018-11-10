using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootDashHability : IHability
{

    ParticleSystem shootFire;
    Collider shootRange;
    float dashingTime;
    float power;

    public ShootDashHability(PlayerController _player, Collider shootRange, ParticleSystem shootFire, float power, float dashingTime, float _timerCoolDown)
    {
        player = _player;
        this.shootRange = shootRange;
        this.shootFire = shootFire;
        this.power = power;
        this.dashingTime = dashingTime;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Hability()
    {
        if (timerCoolDown < 0 && !player.usingHability)
        {
            ShootDash();
            player.usingHability = true;
        }
    }

    void ShootDash()
    {
        player.canMove = false;
        Shoot();
        float direction = Mathf.Sign(player.transform.localScale.z);
        player.moveVector.x = power * -direction;
        player.verticalVelocity = 0;
        player.moveVector.y = player.verticalVelocity;
        player.controller.Move(player.moveVector * Time.deltaTime);
        player.StartCoroutine(IsDashingTimer(dashingTime));
    }

    void Shoot()
    {
        shootFire.Play();
        player.myAnim.Play("ShootHability");
        Collider[] cols = Physics.OverlapBox(shootRange.bounds.center, shootRange.bounds.extents * 2f, shootRange.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in cols)
        {
            if (CheckParently(c.transform))
                continue;
            PlayerController target = TargetScript(c.transform);
            if (target != null)
            {
                if(target is Berserk)
                {
                    target.SetStun(0.3f);
                    target.DisableAll();
                }
                else
                    target.ReceiveImpact(new Vector3(Mathf.Sign(player.transform.localScale.z) * 50, 0, 0), player, false);
                target.SetDamage(10);
                target.WhoHitedMe(player);
                player.whoIHited = target;
            }
        }
    }

    IEnumerator IsDashingTimer(float x)
    {
        while (true)
        {
            player.isDashing = true;
            player.myAnim.SetBool("Dashing", true);
            yield return new WaitForSeconds(x);
            player.isDashing = false;
            player.myAnim.SetBool("Dashing", false);
            ResetValues();
            break;
        }
    }

    void ResetValues()
    {
        timerCoolDown = coolDown;
        player.canMove = true;
        player.usingHability = false;
        player.myAnim.SetBool("Dashing", false);
    }

    #region Extra Calculation
    public bool CheckParently(Transform t)
    {
        if (t.parent == null)
            return false;
        if (t.parent == player.transform)
            return true;
        return CheckParently(t.parent);
    }

    public PlayerController TargetScript(Transform t)
    {
        if (t.GetComponent<PlayerController>())
            return t.GetComponent<PlayerController>();
        if (t.parent == null)
            return null;
        return TargetScript(t.parent);
    }
    #endregion
}
