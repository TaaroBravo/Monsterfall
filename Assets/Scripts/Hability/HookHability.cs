using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HookHability : IHability
{
    Hook hook;
    Vector3 dir;

    Hook grabHook;

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
        grabHook = Physics.OverlapSphere(player.transform.position, 10f).Select(x => x.GetComponent<Hook>()).Where(x => x != null).FirstOrDefault();
        //Va a necesitar un cooldown o un feedback de cuándo puede agarrarlo.
        if (grabHook)
            grabHook.SetHookGrabbed(player);

        if (timerCoolDown < 0 && !grabHook)
        {
            hook.gameObject.SetActive(true);
            float x = player.GetComponent<PlayerInput>().MainHorizontal();
            float y = player.GetComponent<PlayerInput>().MainVertical();
            if (x + y == 0)
                x = Mathf.Sign(player.transform.localScale.z);

            hook.Fire(new Vector3(x, y, 0));
            hook.OnReachedTarget += t => t.StartStun(0.5f);
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
