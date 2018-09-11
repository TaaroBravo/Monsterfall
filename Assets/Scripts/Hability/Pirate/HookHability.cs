using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HookHability : IHability
{
    Hook _hook;
    Vector3 dir;

    Hook grabHook;

    public HookHability(PlayerController p, Hook hook, float _timerCoolDown = 0)
    {
        player = p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _hook = hook;
        _hook.OnFailedFire += () => FailedFire();
        _hook.OnReachedTarget += t => ReachedTarget();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Hability()
    {
        if(!player.stunnedByHit)
        {
            grabHook = Physics.OverlapSphere(player.transform.position, 10f).Select(x => x.GetComponent<Hook>()).Where(x => x != null).FirstOrDefault();
            if (grabHook)
                grabHook.SetHookGrabbed(player);
        }

        if (timerCoolDown < 0 && !grabHook)
        {
            _hook.gameObject.SetActive(true);
            float x = player.GetComponent<PlayerInput>().MainHorizontal();
            float y = player.GetComponent<PlayerInput>().MainVertical();
            if (x + y == 0)
                x = Mathf.Sign(player.transform.localScale.z);

            _hook.Fire(new Vector3(x, y, 0));
            _hook.OnReachedTarget += t => t.StartStun(0.5f);
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
        _hook.gameObject.SetActive(false);
        timerCoolDown = coolDown;
        player.usingHability = false;
    }

}
