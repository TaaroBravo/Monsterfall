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
            Collider[] cols = Physics.OverlapBox(center, player.GetComponent<Collider>().bounds.extents * 5f, player.transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
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
                    target.ReceiveDamage(damage * _power, player);
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
            _hability.Play();
            player.myAnim.Play("GameStart");
            active = true;
            timerCoolDown = coolDown;
            player.usingHability = true;
        }
        else
        {
            cooldownHUD.UseSkill(coolDown);
        }
    }

    void ResetValues()
    {
        damage = Vector3.zero;
        player.usingHability = false;
        active = false;
        playersHitted.Clear();
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
