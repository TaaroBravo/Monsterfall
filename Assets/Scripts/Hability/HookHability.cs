using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HookHability : IHability
{
    Hook hook;
    Vector3 dir;

    public HookHability(PlayerController p, Hook _hook, float _timerCoolDown = 0)
    {
        player = p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        hook = _hook;
        hook.OnFailedFire += () => FailedFire();
        hook.OnReachedTarget += t => ReachedTarget();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            hook.gameObject.SetActive(true);
            float x = player.GetComponent<PlayerInput>().MainHorizontal();
            float y = player.GetComponent<PlayerInput>().MainVertical();
            if (x + y == 0)
                x = Mathf.Sign(player.transform.forward.x);

            hook.Fire(new Vector3(x, y, 0));
            hook.OnReachedTarget += t => t.StartStun(1f);
            player.usingHability = true;
        }
    }

    public void ReachedTarget()
    {
        ResetValues();
    }

    public void FailedFire()
    {
        ResetValues();
    }

    void ResetValues()
    {
        hook.gameObject.SetActive(false);
        timerCoolDown = coolDown;
        player.usingHability = false;
    }

}
