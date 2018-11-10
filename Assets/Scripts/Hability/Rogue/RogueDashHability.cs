using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RogueDashHability : IHability
{

    Collider _hitArea;

    float _speed;
    float _power;

    float _currentTime;
    float _activeTime;

    bool active;

    Vector3 damage;
    Vector3 finalPos;
    Vector3 _dir;

    Vector3 startPos;

    ParticleSystem _hability;

    List<PlayerController> playersHitted = new List<PlayerController>();

    public RogueDashHability(PlayerController p, CdHUDChecker _cooldownHUD, Collider hitArea, ParticleSystem hability, float power, float dashingTime, float speed, float _timerCoolDown = 0)
    {
        player = p;
        _speed = speed;
        _power = power;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _activeTime = dashingTime;
        cooldownHUD = _cooldownHUD;
        _hitArea = hitArea;
        _hability = hability;
        startPos = player.transform.position;
    }

    IEnumerator IsDashingTimer(float x)
    {
        while (true)
        {
            active = true;
            player.isDashing = true;
            player.myAnim.Play("Dash");
            player.myAnim.SetBool("Dashing", true);
            yield return new WaitForSeconds(x);
            player.isDashing = false;
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
            player.transform.position += _dir * _speed * Time.deltaTime;
            Collider[] cols = Physics.OverlapSphere(_hitArea.bounds.center, _hitArea.bounds.extents.y, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
                PlayerController target = TargetScript(c.transform);
                if (target != null && !playersHitted.Contains(target))
                {
                    playersHitted.Add(target);
                    target.SetDamage(10);
                    target.ReceiveImpact(damage * _power, player);
                    player.whoIHited = target;
                }
            }
        }
        if ((finalPos - player.transform.position).magnitude < 3f)
        {
            player.transform.position = finalPos;
            ResetValues();
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
            float distance = _speed * _activeTime;
            _dir = new Vector3(x, y, 0);
            finalPos = CalculateFinalPos(_dir, distance);

            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, _dir, out hit, 30))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject.tag == "Borders" && (hit.point - player.transform.position).magnitude < 6f)
                {
                    GameManager.Instance.RegisterLastPos(player, hit.point);
                    finalPos = hit.point;
                }
            }

            damage = _dir;
            _hability.Play();
            player.controller.enabled = false;
            player.StartCoroutine(IsDashingTimer(_activeTime));
            timerCoolDown = coolDown;
            player.usingHability = true;
            player.lifeHUD.ActivateDashCD();
        }
        //else
        //{
        //    cooldownHUD.UseSkill(coolDown);
        //}
    }

    Vector3 CalculateFinalPos(Vector3 dir, float distance)
    {
        finalPos = player.transform.position + (dir * distance);
        return finalPos;
    }

    void ResetValues()
    {
        finalPos = Vector3.zero;
        player.moveVector.x = 0;
        player.StopCoroutine(IsDashingTimer(_activeTime));
        player.canMove = true;
        player.myAnim.SetBool("Dashing", false);
        player.controller.enabled = true;
        player.usingHability = false;
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
