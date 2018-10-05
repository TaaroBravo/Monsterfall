using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHook : IHability
{

    Hook _hook;
    Transform _hookPosition;

    public MovementHook(PlayerController p, Hook hook, Transform hookPosition, float _timerCoolDown = 0)
    {
        player = p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _hookPosition = hookPosition;
        _hook = hook;
        _hook.OnReachedPoint += () => ResetValues();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            player.canMove = false;
            _hook.gameObject.SetActive(true);
            player.usingHability = true;
            _hook.Fire(player.hookChosenPosition);
        }
    }

    void ResetValues()
    {
        player.canMove = true;
        player.usingHability = false;
        timerCoolDown = coolDown;
        _hook.gameObject.SetActive(false);
    }
}
