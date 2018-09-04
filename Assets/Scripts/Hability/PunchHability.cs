using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PunchHability : IHability
{

    Punch _punch;
    float _power;
    float _activeTime;

    bool active;


    public PunchHability(PlayerController p, Punch punch, float power, float activeTime, float _timerCoolDown = 0)
    {
        player = p;
        _punch = punch;
        _power = power;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _activeTime = activeTime;
    }

    public override void Update()
    {
        base.Update();
    }

    IEnumerator ActiveTime(float x)
    {
        while(true)
        {
            _punch.gameObject.SetActive(true);
            yield return new WaitForSeconds(x);
            _punch.gameObject.SetActive(false);
            _punch.transform.position = player.transform.position;
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            Collider col = _punch.GetComponent<Collider>();
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
                PlayerController target = TargetScript(c.transform);
                if (target != null)
                {
                    float x = player.GetComponent<PlayerInput>().MainHorizontal();
                    float y = player.GetComponent<PlayerInput>().MainVertical();
                    if (x + y == 0)
                        x = Mathf.Sign(player.transform.localScale.z);
                    Vector3 dir = new Vector3(x, y, 0);
                    Vector3 damage = Vector3.zero;
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        damage.x = x;
                        player.myAnim.SetBool("ReleaseAForward", true);
                    }
                    else if (y > 0)
                    {
                        damage.y = y;
                        player.myAnim.SetBool("ReleaseAUp", true);
                    }
                    else
                    {
                        damage.y = y;
                        player.myAnim.SetBool("ReleaseADown", true);
                    }
                    _punch.Hit(damage);
                    target.ReceiveDamage(damage * _power, false);
                    player.whoIHited = target;
                }
            }
            timerCoolDown = coolDown;
            player.usingHability = true;
        }
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
