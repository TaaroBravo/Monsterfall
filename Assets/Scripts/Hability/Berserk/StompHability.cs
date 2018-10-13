using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class StompHability : IHability
{

    float _power;
    Vector3 damage;
    bool active;

    List<PlayerController> playersHitted = new List<PlayerController>();
    List<PlayerController> playersStunned = new List<PlayerController>();

    public StompHability(Berserk p, CdHUDChecker _cooldownHUD, float power, float _timerCoolDown = 0)
    {
        player = p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        cooldownHUD = _cooldownHUD;
        _power = power;
        p.OnStompHability += () => StompHabilityCall();
    }

    public override void Update()
    {
        base.Update();
        if (active)
        {
            if (playersHitted.Count() != 0 && playersHitted.Count() == playersStunned.Count())
                ResetValues();

            Vector3 center = player.transform.position + (Vector3.up * player.GetComponent<Collider>().bounds.extents.y);
            Collider[] cols = Physics.OverlapSphere(center, player.GetComponent<Collider>().bounds.extents.x * 10f, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
                PlayerController target = TargetScript(c.transform);
                if (target != null && !playersHitted.Contains(target) && target != player)
                {
                    Vector3 dir = (player.transform.position - target.transform.position).normalized;

                    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                        damage.x = dir.x;
                    else if (dir.y > 0)
                        damage.y = dir.y;
                    else
                        damage.y = dir.y;
                    
                    playersHitted.Add(target);
                    Debug.Log(target.gameObject.name);
                    target.ReceiveDamage(damage * _power, false);
                    player.whoIHited = target;
                }
            }
            CheckPositions();
        }
    }

    void StompHabilityCall()
    {
        active = true;
    }

    IEnumerator ResetValuesTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            ResetValues();
            break;
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            player.myAnim.Play("Skill");
            //active = true;
            player.canMove = false;
            player.ResetVelocity();
            //player.usingHability = true;
            player.StartCoroutine(ResetValuesTimer());
        }
        else
        {
            cooldownHUD.UseSkill(coolDown);
        }
    }

    void CheckPositions()
    {
        var stunned = playersHitted.Where(x => x != player).Where(x => !playersStunned.Contains(x)).Where(x => (x.transform.position - player.transform.position).magnitude < 2f).FirstOrDefault();
        if (stunned)
        {
            stunned.DisableStun();
            stunned.ResetVelocity();
            playersStunned.Add(stunned);
            stunned.SetStun(0.2f);
        }
    }

    void ResetValues()
    {
        damage = Vector3.zero;
        player.canMove = true;
        player.usingHability = false;
        active = false;
        playersHitted.Clear();
        playersStunned.Clear();
        timerCoolDown = coolDown;
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
