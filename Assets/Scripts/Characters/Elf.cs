using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Elf : PlayerController
{

    public event Action<PlayerController> OnDisableEffect = delegate { };
    public event Action OnCleanTargets = delegate { };

    public List<PlayerController> targets = new List<PlayerController>();

    public Transform[] randomPositions;
    IEffect markEffect;

    //Falta asignarlos
    public Missile missilePrefab;
    public MissileTeleport missileTeleportPrefab;

    public override void Start()
    {
        base.Start();
        lifeHUD.Set(14, 3, myLife);
        markEffect = new IElfMark(this, AddTarget);
        OnDisableEffect += x => DisablePlayerEffect(x);
        randomPositions = GameObject.FindGameObjectsWithTag("PositionsElf").Select(x => x.transform).ToArray();
        SetAttacks();
        SetHabilities();
    }

    public override void Update()
    {
        base.Update();
    }

    void ElfHability(string state)
    {
        if (state == "Realese")
        {
            hability["MissileHability"].Release();
        }
        else
        {
            hability["MissileHability"].Hability();
        }
    }

    void MovementHability(string state)
    {
        if (state == "Realese")
        {
            hability["TeleportHability"].Release();
        }
        else
        {
            hability["TeleportHability"].Hability();
        }
    }

    public void AddTarget(PlayerController p)
    {
        if (!targets.Contains(p))
        {
            targets.Add(p);
            StartCoroutine(RealeseMark(p));
        }
        else
        {
            StopCoroutine(RealeseMark(p));
            StartCoroutine(RealeseMark(p));
        }
    }

    public void CleanTargets()
    {
        targets.Clear();
        OnCleanTargets();
    }

    IEnumerator RealeseMark(PlayerController p)
    {
        while (true)
        {
            yield return new WaitForSeconds(14f);
            //OnDisableEffect(p);
            break;
        }
    }

    public void DisablePlayerEffect(PlayerController player)
    {
        if (targets.Contains(player))
            targets.Remove(player);
        if (targets.Count == 0)
            CleanTargets();
    }

    public void DisableEffect(PlayerController player)
    {
        OnDisableEffect(player);
        CleanTargets();
    }

    private void SetAttacks()
    {
        attacks.Add(typeof(NormalAttack).ToString(), new NormalAttack(this, markEffect, normalAttackCoolDown));
        attacks.Add(typeof(UpAttack).ToString(), new UpAttack(this, markEffect, upAttackCoolDown));
        attacks.Add(typeof(DownAttack).ToString(), new DownAttack(this, markEffect, downAttackCoolDown));
    }

    void SetHabilities()
    {
        hability.Add(typeof(MissileHability).ToString(), new MissileHability(this, missilePrefab, 14f));
        hability.Add(typeof(TeleportHability).ToString(), new TeleportHability(this, missileTeleportPrefab, 3f));
        myHability = ElfHability;
        movementHability = MovementHability;
    }
}
