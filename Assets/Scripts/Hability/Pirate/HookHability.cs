using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class HookHability : IHability
{
    private ChainPart _chainPrefab;
    private ChainManager _chainManager;

    Hook _hook;
    Vector3 dir;

    Hook grabHook;
    Vector3 _point;
    Vector3 _forward;

    bool isReturning;

    bool activeRoundHability;
    bool readyToActive;

    public HookHability(PlayerController p, ChainPart prefab, CdHUDChecker _cooldownHUD, Hook hook, float _timerCoolDown = 0)
    {
        player = p;
        _chainPrefab = prefab;
        _chainManager = new ChainManager(ChainPartFactory, hook);
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        cooldownHUD = _cooldownHUD;
        _hook = hook;
        _hook.OnFailedFire += () => FailedFire();
        _hook.OnReachedTarget += t => ReachedTarget();
        _hook.OnReachedPoint += () => ResetValues();
        _hook.OnReturning += () => ReturningHook();
    }

    public override void Update()
    {
        base.Update();
        if (activeRoundHability && !readyToActive)
        {
            activeRoundHability = false;
            _hook.ReturnHookSecondState();
        }
        _chainManager.Update(_hook.spawnPoint.position, _hook.transform.position);
    }

    public override void Hability()
    {
        if (!player.stunnedByHit)
        {
            grabHook = Physics.OverlapSphere(player.transform.position, 10f).Select(x => x.GetComponent<Hook>()).Where(x => x != null).FirstOrDefault();
            if (grabHook)
                grabHook.SetHookGrabbed(player);
        }
        if (isReturning && readyToActive)
        {
            activeRoundHability = true;
            _hook.ActiveSecondState();
        }
        else if (timerCoolDown < 0 && !grabHook && !player.usingHability)
        {
            AudioManager.Instance.CreateSound("ThrowHook");
            _hook.gameObject.SetActive(true);
            float x = player.GetComponent<PlayerInput>().MainHorizontal();
            float y = player.GetComponent<PlayerInput>().MainVertical();
            if (x + y == 0)
                x = Mathf.Sign(player.transform.localScale.z);

            _hook.Fire(new Vector3(x, y, 0));
            _hook.OnReachedTarget += t => t.SetStun(0.5f);
            player.usingHability = true;
            isPressing = true;
            isReturning = false;
        }
    }

    public void ReturningHook()
    {
        isReturning = true;
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
        player.lifeHUD.ActivateSkillCD();
        activeRoundHability = false;
        readyToActive = false;
        isReturning = false;
    }

    ChainPart ChainPartFactory()
    {
        return GameObject.Instantiate(_chainPrefab);
    }

    public override void Release()
    {
        if (activeRoundHability)
            readyToActive = false;
        else
            readyToActive = true;
        isPressing = false;
    }

}
