using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueDashHability : IHability
{
    Collider _hitArea;

    float _speed;
    float _power;

    float _currentTime;
    float _activeTime;

    bool active;

    Vector3 damage;

    List<PlayerController> playersHitted = new List<PlayerController>();

    public RogueDashHability(PlayerController p, Collider hitArea, float power, float dashingTime, float speed, float _timerCoolDown = 0)
    {
        player = p;
        _speed = speed;
        _power = power;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _activeTime = dashingTime;
    }

    IEnumerator IsDashingTimer(float x)
    {
        while (true)
        {
            active = true;
            player.myAnim.SetBool("Dashing", true);
            yield return new WaitForSeconds(x);
            active = false;
            ResetValues();
            break;
        }
    }

    public override void Update()
    {
        base.Update();
        if (active)
        {
            Collider[] cols = Physics.OverlapBox(_hitArea.bounds.center, _hitArea.bounds.extents, _hitArea.transform.rotation, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
                PlayerController target = TargetScript(c.transform);
                if (target != null && !playersHitted.Contains(target))
                {
                    playersHitted.Add(target);
                    target.ReceiveDamage(damage * _power, false);
                    player.whoIHited = target;
                }
            }
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            float x = player.GetComponent<PlayerInput>().MainHorizontal();
            float y = player.GetComponent<PlayerInput>().MainVertical();
            if (x + y == 0)
                x = Mathf.Sign(player.transform.localScale.z);
            Vector3 dir = new Vector3(x, y, 0);
            damage = dir;
            player.canMove = false;
            player.controller.enabled = false;
            player.StartCoroutine(IsDashingTimer(timerCoolDown));
            timerCoolDown = coolDown;
            player.usingHability = true;
        }
    }

    void ResetValues()
    {
        player.canMove = true;
        player.myAnim.SetBool("Dashing", false);
        player.controller.enabled = true;
        player.usingHability = false;
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
