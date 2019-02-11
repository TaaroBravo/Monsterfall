using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeti : PlayerController
{

    public override void Start()
    {
        base.Start();
        lifeHUD.Set(7, 5, myLife);
        SetAttacks();
        SetHabilities();
    }

    public override void Update()
    {
        base.Update();
    }

    void YetiAbility(string state)
    {
        if (state == "Realese")
        {
            //hability["MissileHability"].Release();
        }
        else
        {
            //hability["MissileHability"].Hability();
        }
    }

    void YetiMovementAbility(string state)
    {
        if (state == "Realese")
        {
            //hability["TeleportHability"].Release();
        }
        else
        {
            //hability["TeleportHability"].Hability();
        }
    }

    private void SetAttacks()
    {
        attacks.Add(typeof(NormalAttack).ToString(), new NormalAttack(this, null, normalAttackCoolDown));
        attacks.Add(typeof(UpAttack).ToString(), new UpAttack(this, null, upAttackCoolDown));
        attacks.Add(typeof(DownAttack).ToString(), new DownAttack(this, null, downAttackCoolDown));
    }

    void SetHabilities()
    {
        //hability.Add(typeof(MissileHability).ToString(), new MissileHability(this, missilePrefab, 14f));
        //hability.Add(typeof(TeleportHability).ToString(), new TeleportHability(this, missileTeleportPrefab, 3f));
        myHability = YetiAbility;
        movementHability = YetiMovementAbility;
    }
}
