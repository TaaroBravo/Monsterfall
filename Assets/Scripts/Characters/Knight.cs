using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Knight : PlayerController {

    IEffect _fireEffect;
    public float powerOfPunch;
    public float punchCooldown;

    public ParticleSystem ps_Hability;

    public float forcedJumpForce;
    public float forcedJumpCooldown;

    public override void Start()
    {
        base.Start();
        _fireEffect = new IFireEffect(1, 4f, 0.4f);
        SetAttacks();
        SetHabilities();
    }

    public override void Update()
    {
        base.Update();
        if (!canMove)
            StartCoroutine(TimerIsTouchingWalls());

    }

    IEnumerator TimerIsTouchingWalls()
    {
        while(true)
        {
            var state = false;
            if (IsTouchingWalls())
                state = true;
            yield return new WaitForSeconds(1f);
            if (state && !canMove && IsTouchingWalls())
                canMove = true;
            StopCoroutine(TimerIsTouchingWalls());
            break;
        }
    }

    public void DisableDashHability()
    {
        DisableAll();

    }

    void KnightHability()
    {
        hability["PunchHability"].Hability();
    }

    void MovementHability()
    {
        hability["ForcedJump"].Hability();
    }

    private void SetAttacks()
    {
        attacks.Add(typeof(NormalAttack).ToString(), new NormalAttack(this, _fireEffect, normalAttackCoolDown));
        attacks.Add(typeof(UpAttack).ToString(), new UpAttack(this, _fireEffect, upAttackCoolDown));
        attacks.Add(typeof(DownAttack).ToString(), new DownAttack(this, _fireEffect, downAttackCoolDown));
    }

    void SetHabilities()
    {
        hability.Add(typeof(PunchHability).ToString(), new PunchHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), ps_Hability, powerOfPunch, punchCooldown));
        hability.Add(typeof(ForcedJump).ToString(), new ForcedJump(this, forcedJumpForce, forcedJumpCooldown));
        myHability = KnightHability;
        movementHability = MovementHability;
    }
}
