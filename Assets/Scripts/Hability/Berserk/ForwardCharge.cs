using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ForwardCharge : IHability
{

    float _speed;
    float _dir;

    PlayerController _target;

    float distance;
    float timerActive;

    public ForwardCharge(PlayerController p, float speed, float _timerCoolDown = 0)
    {
        player = p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _speed = speed;
    }

    public override void Update()
    {
        base.Update();

        if (player.usingHability)
        {
            if (player.IsTouchingWalls())
            {
                Debug.Log("HNA");
                if (_target)
                    DamageTarget();
                ResetValues();
                return;
            }
            timerActive += Time.deltaTime;
            player.moveVector.x = _dir * _speed;
            distance = _speed * timerActive;
            if (_target)
                Charge();
            else
                _target = Physics.OverlapSphere(player.transform.position, 1f, 1 << 9).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player).Where(x => !x.isDead).FirstOrDefault();
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            timerActive = 0;
            distance = 0;
            _dir = Mathf.Sign(player.transform.localScale.x);
            player.canMove = false;
            player.usingHability = true;
        }
    }

    void Charge()
    {
        _target.transform.SetParent(player.transform);
        _target.transform.position = player.transform.position + (Vector3.right * Mathf.Sign(player.transform.localScale.x));

        var enemies = Physics.OverlapSphere(player.transform.position, 1f, 1 << 9).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player && x != _target).Where(x => !x.isDead);
        foreach (var _enemy in enemies)
            _enemy.ReceiveDamage((Vector3.right * Mathf.Sign(player.transform.localScale.x) * 30));
    }

    void DamageTarget()
    {
        _target.transform.SetParent(null);
        _target.SetStun(0.2f);
        _target.SetDamage(distance * 3);
    }

    void ResetValues()
    {
        _target = null;
        timerActive = 0;
        distance = 0;
        timerCoolDown = coolDown;
        player.canMove = true;
        player.usingHability = false;
    }
}
