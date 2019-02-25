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

    public bool forcedJumping;

    public Collider particleSphereCollider;

    public override void Start()
    {
        base.Start();
        _fireEffect = new IFireEffect(1f, 4f, 0.4f);
        lifeHUD.Set(punchCooldown, forcedJumpCooldown, myLife);
        SetAttacks();
        SetHabilities();
    }

    public override void Update()
    {
        base.Update();
        if (!canInteract)
            StartCoroutine(TimerIsTouchingWalls());
    }

    public override void ResetAll()
    {
        base.ResetAll();
        forcedJumping = false;
    }


    public void CallJumpHabilityCoroutine()
    {
        StartCoroutine(IsForcedJumping());
    }

    IEnumerator IsForcedJumping()
    {
        while(forcedJumping)
        {
            yield return new WaitForSeconds(0.1f);
            CreateNewCollider();
        }
    }

    void CreateNewCollider()
    {
        Collider tempCol = Instantiate(particleSphereCollider);
        tempCol.transform.position = new Vector3(transform.position.x, transform.position.y + GetComponent<Collider>().bounds.extents.y + transform.position.z);
        tempCol.isTrigger = true;
        //tempCol.tag = "ParticleCollider";
        Destroy(tempCol.gameObject, 2f);
    }

    IEnumerator TimerIsTouchingWalls()
    {
        while(true)
        {
            var state = false;
            if (IsTouchingWalls())
                state = true;
            yield return new WaitForSeconds(1f);
            if (state && !canInteract && IsTouchingWalls())
                canInteract = true;
            StopCoroutine(TimerIsTouchingWalls());
            break;
        }
    }

    public void DisableDashHability()
    {
        DisableAll();
    }

    void KnightHability(string state)
    {
        if (state == "Realese")
        {
            hability["PunchHability"].Release();
        }
        else
        {
            hability["PunchHability"].Hability();
        } 
    }

    void MovementHability(string state)
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
