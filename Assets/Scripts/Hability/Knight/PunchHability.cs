using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PunchHability : IHability
{
    ParticleSystem _hability;
    float _power;

    bool active;

    Vector3 spawnPos;
    Vector3 damage;

    Vector3 spawnPoint;

    List<PlayerController> playersHitted = new List<PlayerController>();


    public PunchHability(PlayerController p, CdHUDChecker _cooldownHUD, ParticleSystem hability, float power, float _timerCoolDown = 0)
    {
        player = p;
        _power = power;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        cooldownHUD = _cooldownHUD;
        _hability = hability;
    }

    public override void Update()
    {
        base.Update();
        if (active)
        {
            Vector3 center = player.transform.position + (Vector3.up * player.GetComponent<Collider>().bounds.extents.y);
            var layerMask1 = LayerMask.GetMask("Hitbox");
            var layerMask2 = LayerMask.GetMask("Hook");
            var layerMask = layerMask1 | layerMask2;
            Collider[] cols = Physics.OverlapBox(center, player.GetComponent<Collider>().bounds.extents * 6f, player.transform.rotation, layerMask);
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
                if(c.GetComponent<Hook>())
                {
                    c.GetComponent<Hook>().returnFail = true;
                    continue;
                }
                PlayerController target = TargetScript(c.transform);
                if (target != null && !playersHitted.Contains(target))
                {
                    Vector3 dir = (target.transform.position - player.transform.position).normalized;

                    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                        damage.x = dir.x;
                    else if (dir.y > 0)
                        damage.y = dir.y;
                    else
                        damage.y = dir.y;
                    playersHitted.Add(target);
                    target.ReceiveImpact(damage * _power, player);
                    target.SetDamage(15); // rico nerf
                    player.whoIHited = target;
                }
            }
            active = false;
            ResetValues();
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            player.usingHability = true;
            _hability.Play();
            player.myAnim.Play("GameStart");
            active = true;
            timerCoolDown = coolDown;
            player.lifeHUD.ActivateSkillCD();
            AudioManager.Instance.CreateSound("ExplosionKnight");
        }
    }

    void ResetValues()
    {
        damage = Vector3.zero;
        player.usingHability = false;
        active = false;
        playersHitted.Clear();
    }

    public override void Release()
    {

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
