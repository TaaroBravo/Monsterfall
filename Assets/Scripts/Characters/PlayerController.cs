using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public float myLife;
    public CharacterController controller;
    public PlayerContrains contrains;

    #region Movement Variables
    public bool canMove;

    public Vector3 moveVector;
    public float moveSpeed;
    public float slowSpeedCharge;
    public float maxSpeedChargeTimer;
    public float verticalVelocity;

    public float gravity;

    public ParticleSystem PS_Jump;
    public float jumpForce = 20;
    public bool isJumping;
    public bool isFalling;
    public bool canJump;
    public bool delayJump;

    public bool isFallingOff;
    public float fallOffSpeed;

    public Transform landedPlatform;

    public bool coyoteBool;
    public float coyoteTime;
    #endregion

    #region Attack Variables
    public float weaponExtends;

    public float impactVelocityNormal;
    public float defaultAttackNormal;
    public float normalAttackCoolDown;
    public float influenceOfMovementNormal;


    public float impactVelocityUp;
    public float defaultAttackUp;
    public float upAttackCoolDown;
    public float influenceOfMovementUp;

    public float impactVelocityDown;
    public float defaultAttackDown;
    public float downAttackCoolDown;
    public float influenceOfMovementDown;
    #endregion

    #region Dash Variables
    public ParticleSystem PS_Dash;
    public float dashSpeed = 50;
    public float dashDistance = 7;
    public float dashCoolDown;
    public bool isDashing;
    public bool canDash = true;
    #endregion

    #region Impact Variables
    public ParticleSystem PS_Impact;
    public Vector3 impactVelocity;

    public float impactSpeed = 20;
    public float maxImpactToInfinitStun;
    public float impactStunMaxTimer;
    public float currentImpactStunTimer;
    public float residualStunImpact;

    public float hitHeadReject;
    public float maxNoStunVelocityLimit;
    public float maxStunVelocityLimit;

    public float impactMarked;

    bool canAttack;

    public ParticleSystem hitParticles;
    #endregion

    #region Hability Variables
    public bool usingHability;

    #endregion

    private IMove _iMove;

    public Collider attackColliders;

    public PlayerController whoHitedMe;
    public PlayerController whoIHited;
    public bool isDead;
    public bool stunnedByHit;
    public bool playerMarked;

    public float chargedEffect;
    public bool isCharged;
    public bool hitCharged;

    public PlayerHPHud myLifeUI;

    public ParticleSystem PS_Stunned;
    public ParticleSystem PS_Marked;
    public ParticleSystem PS_Charged;
    public ParticleSystem PS_Fall;
    public ParticleSystem PS_LitOnFire;
    public ParticleSystem PS_LitOnFire2;

    public Animator myAnim;

    public delegate void CallHability();
    public CallHability myHability = delegate { };
    public CallHability movementHability = delegate { };

    public bool dashRogue;

    public bool touchingWall;
    public Transform hookChosenPosition;

    public PlayerController lastOneWhoHittedMe;

    Tuple<bool, Vector3> rejectWall = new Tuple<bool, Vector3>(false, Vector3.zero);

    #region Ray Borders
    private Transform rBottomPos;
    private Transform rTopPos;
    private Transform[] rDownPos;
    private Transform[] rUpPos;
    #endregion

    #region Dictionary
    private Dictionary<string, IMove> myMoves = new Dictionary<string, IMove>();
    public Dictionary<string, IAttack> attacks = new Dictionary<string, IAttack>();
    public Dictionary<string, IHability> hability = new Dictionary<string, IHability>();
    #endregion

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        myAnim = GetComponent<Animator>();
        contrains = GetComponent<PlayerContrains>();
    }

    void SetRayPos()
    {
        rBottomPos = contrains.rBottomPos;
        rTopPos = contrains.rTopPos;
        rDownPos = contrains.rDownPos;
        rUpPos = contrains.rUpPos;
    }

    public virtual void Start()
    {
        contrains.OnTeleportPlayer += (x, y) => OnTeleported();
        coyoteTime = 0.1f;
        SetMovements();
        SetAttacks();
        SetHabilities();
        StartCoroutine(CanAttack(0.25f));
        SetRayPos();
    }

    public virtual void Update()
    {
        PhysicsOptions();
        Move();
        UpdateHabilities();
        Attack();
        StunAndMark();

        if (isDead)
            moveVector = new Vector3(0, -10, 0);
        controller.Move(moveVector * Time.deltaTime);
    }

    #region Moves & Jump
    public void Move()
    {
        if (stunnedByHit)
        {
            moveVector.x = impactVelocity.x;
            if (moveVector.x > 10)
            {
                moveVector.y = 0;
            }
            else
                moveVector.y = impactVelocity.y;
        }
        else
        {
            foreach (var m in myMoves.Values)
                m.Update();
            if ((canJump && controller.isGrounded && !isFallingOff) || (delayJump && controller.isGrounded) || coyoteBool && canJump && !isFallingOff)
            {
                myMoves["Jump"].Move();
                canJump = false;
                delayJump = false;
                coyoteBool = false;
            }
            else if (!isDashing && !isFallingOff)
                myMoves["HorizontalMovement"].Move();
        }
    }

    public void Jump()
    {
        canJump = true;
        if (isFalling && IsCloseToGround())
            delayJump = true;
    }
    #endregion

    #region PhysicsOptions

    #region PhysicsChecks
    void PhysicsOptions()
    {
        PhysicsChecks();
        if (rejectWall.Item1)
            AggresiveHitReflect();
        if (controller.isGrounded)
            StartCoroutine(CoyoteTime(coyoteTime));
    }

    void PhysicsChecks()
    {
        if (verticalVelocity < 0 && !controller.isGrounded)
        {
            isFalling = true;
            landedPlatform = null;
        }
        else
            isFalling = false;
    }

    public bool IsCloseToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, GetComponent<Collider>().bounds.extents.y + 0.5f))
        {
            if (hit.collider.isTrigger)
            {
                delayJump = false;
                return false;
            }
            landedPlatform = hit.transform;
            return true;
        }
        return false;
    }

    public bool IsTouchingWalls()
    {
        RaycastHit hit;
        var layerMaskIgnore1 = 1 << 8;
        var layerMaskIgnore2 = 1 << 9;
        var layerMaskIgnore3 = 1 << 13;
        var layerMaskIgnore4 = 1 << 18;
        var layerMask = layerMaskIgnore1 | layerMaskIgnore2 | layerMaskIgnore3 | layerMaskIgnore4;
        layerMask = ~layerMask;
        Debug.Log("Entre 1");
        if (rBottomPos)
        {
            if (Physics.Raycast(rBottomPos.position, (rUpPos[0].position - rBottomPos.position).normalized, out hit, 2f, layerMask))
            {
                Debug.Log("Entre 2");
                return true;
            }
            if (Physics.Raycast(rBottomPos.position, (rUpPos[1].position - rBottomPos.position).normalized, out hit, 2f, layerMask))
            {
                Debug.Log("Entre 2");
                return true;
            }
            if (Physics.Raycast(rTopPos.position, (rDownPos[0].position - rTopPos.position).normalized, out hit, 2f, layerMask))
            {
                Debug.Log("Entre 2");
                return true;
            }
            if (Physics.Raycast(rTopPos.position, (rDownPos[1].position - rTopPos.position).normalized, out hit, 2f, layerMask))
            {
                Debug.Log("Entre 2");
                return true;
            }
        }
        return false;
    }

    IEnumerator CoyoteTime(float timer)
    {
        while (true)
        {
            bool grounded = controller.isGrounded;
            yield return new WaitForSeconds(timer);
            if (grounded != controller.isGrounded && !isJumping && isFalling)
            {
                coyoteBool = true;
                yield return new WaitForSeconds(timer * 2);
                coyoteBool = false;
            }
            break;
        }
    }

    #endregion


    public void SmoothHitRefleject()
    {
        moveVector = Vector3.zero;
        myAnim.Play("HitOnWall");
    }

    void AggresiveHitReflect()
    {
        impactVelocity = new Vector3(rejectWall.Item2.x * 30, 0, 0);
        myAnim.Play("GetHitDown Bot");
    }

    IEnumerator HitReflectTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            rejectWall = Tuple.Create(false, Vector3.zero);
            stunnedByHit = false;
            impactVelocity = Vector3.zero;
            myAnim.SetBool("Stunned", false);
            break;
        }
    }
    #endregion

    #region Stun & Mark
    void StunAndMark()
    {
        if (stunnedByHit)
        {
            StunUpdate();
        }
    }

    public void SetStun(float x)
    {
        StartCoroutine(StunCoroutine(x));
    }

    IEnumerator StunCoroutine(float x)
    {
        while (true)
        {
            canMove = false;
            myAnim.SetBool("Stunned", true);
            yield return new WaitForSeconds(x);
            canMove = true;
            myAnim.SetBool("Stunned", false);
            break;
        }
    }

    void StunUpdate()
    {
        currentImpactStunTimer += Time.deltaTime;
        if (currentImpactStunTimer > impactStunMaxTimer || (IsTouchingWalls() && impactVelocity.x != 0))
        {
            if (rejectWall.Item1)
                return;
            if ((transform.position - lastOneWhoHittedMe.transform.position).magnitude < 5f)
            {
                Vector3 dir = (lastOneWhoHittedMe.transform.position - transform.position).normalized;
                impactVelocity = Vector3.zero;
                rejectWall = Tuple.Create(true, dir);
                StartCoroutine(HitReflectTime());
                return;
            }
            whoHitedMe = null;
            stunnedByHit = false;
            playerMarked = false;
            myAnim.SetBool("Stunned", false);
            currentImpactStunTimer = 0;
            impactVelocity = Vector3.zero;
        }
    }

    public void WhoHitedMe(PlayerController pl)
    {
        whoHitedMe = pl;
    }

    public void ResetVelocity()
    {
        moveVector = Vector3.zero;
    }

    public void DisableStun()
    {
        stunnedByHit = false;
        canMove = true;
    }

    public void DisableAll()
    {
        isDashing = false;
        canDash = false;
        isFallingOff = false;
        ResetVelocity();
    }
    #endregion

    #region Attacks
    void Attack()
    {
        if (isCharged)
            Charged();
        foreach (var a in attacks.Values)
            a.Update();
    }

    public void AttackNormal(string state)
    {
        if (!stunnedByHit && canAttack)
        {
            if (state == "Realese")
            {
                GetComponent<ParticlePuños>().PuñoAActivar("recto");
                attacks["NormalAttack"].Attack(attackColliders);
                PS_Charged.Stop();
                PS_Charged.gameObject.SetActive(false);
                StartCoroutine(CanAttack(0.1f));
            }
            else
            {
                PS_Charged.gameObject.SetActive(true);
                attacks["NormalAttack"].Pressed();
            }
        }
    }

    public void AttackDown(string state)
    {
        if (!stunnedByHit && canAttack)
        {
            if (state == "Realese")
            {
                GetComponent<ParticlePuños>().PuñoAActivar("abajo");
                attacks["DownAttack"].Attack(attackColliders);
                PS_Charged.Stop();
                PS_Charged.gameObject.SetActive(false);
                StartCoroutine(CanAttack(0.1f));
            }
            else
            {
                PS_Charged.gameObject.SetActive(true);
                attacks["DownAttack"].Pressed();
            }
        }
    }

    public void AttackUp(string state)
    {
        if (!stunnedByHit && canAttack)
        {
            if (state == "Realese")
            {
                GetComponent<ParticlePuños>().PuñoAActivar("arriba");
                attacks["UpAttack"].Attack(attackColliders);
                PS_Charged.Stop();
                PS_Charged.gameObject.SetActive(false);
                StartCoroutine(CanAttack(0.1f));
            }
            else
            {
                PS_Charged.gameObject.SetActive(true);
                attacks["UpAttack"].Pressed();
            }
        }
    }

    IEnumerator CanAttack(float x)
    {
        while (true)
        {
            canAttack = false;
            yield return new WaitForSeconds(x);
            canAttack = true;
            break;
        }
    }

    void UpdateHabilities()
    {
        foreach (var h in hability.Values)
            h.Update();
    }
    #endregion

    #region Habilities

    public void Dash()
    {
        if (canDash && !isFallingOff && !isDashing)
            movementHability();
        else if (!canDash && !isDashing)
            canDash = true;
        //hability["Dash"].Hability();
    }

    IEnumerator DashCoolDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            isDashing = false;
            canDash = true;
            break;
        }
    }

    public void FallOff()
    {
        if (!controller.isGrounded && !stunnedByHit && !isDashing && !isFallingOff)
            myMoves["FallOff"].Move();
    }

    void OnTeleported()
    {
        isFallingOff = false;
    }

    public void Hability()
    {
        if (!isFallingOff && !stunnedByHit && !isDashing && !usingHability)
            myHability();
    }

    void Charged()
    {
        if (!PS_Charged.isPlaying)
            PS_Charged.Play();
    }

    private void LateUpdate()
    {
        if (hitCharged)
        {
            isCharged = false;
            hitCharged = false;
            PS_Charged.Stop();
        }

        if (myLife <= 0 && !isDead)
        {
            myAnim.Play("Death");
            isDead = true;
            controller.enabled = false;
        }
        else if (myLife <= 0)
        {
            myAnim.SetBool("Running", false);
        }
    }

    public bool onFire;
    void MoveToCancelFire(IEffect effect, float maxTime)
    {
        StartCoroutine(CancelFireCoroutine(effect, maxTime));
        onFire = true;
    }

    IEnumerator CancelFireCoroutine(IEffect effect, float maxTime)
    {
        int counts = 0;
        while (true)
        {
            float input = GetComponent<PlayerInput>().MainHorizontal();
            yield return new WaitUntil(() => Mathf.Sign(GetComponent<PlayerInput>().MainHorizontal()) != Mathf.Sign(input));
            counts++;
            if (counts > 5)
            {
                CancelFire(effect, maxTime);
                break;
            }
        }
    }

    void CancelFire(IEffect effect, float maxTime)
    {
        StopCoroutine(Effect(effect, maxTime));
        effect.DisableEffect(this);
        onFire = false;
    }

    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(attackColliders.bounds.center, attackColliders.bounds.extents * 1.5f);
    }

    #region Effects

    public void ApplyEffect(IEffect _effect)
    {
        if (_effect != null)
        {
            float maxTime = _effect.GetMaxTimer();
            if (_effect is IFireEffect)
            {
                onFire = true;
                MoveToCancelFire(_effect, maxTime);
            }
            StartCoroutine(Effect(_effect, maxTime));
        }
    }

    IEnumerator Effect(IEffect effect, float maxTime)
    {
        while (maxTime > 0)
        {
            if (effect is IFireEffect && !onFire)
                break;
            effect.Effect(this);
            maxTime -= effect.GetDelayTimer();
            yield return new WaitForSeconds(effect.GetDelayTimer());
        }

        effect.DisableEffect(this);
        if (effect is IFireEffect)
            StopCoroutine(CancelFireCoroutine(effect, maxTime));
    }

    #endregion

    #region ReceiveDamage
    public void SetLastOneWhoHittedMe(PlayerController killer)
    {
        lastOneWhoHittedMe = killer;
    }

    public void ReceiveImpact(Vector3 impact, PlayerController killer, bool marked = false, bool hittedByBerserk = false)
    {
        Vector3 impactRelax = Vector3.zero;
        Vector3 impactNormalized = new Vector3(impact.x == 0 ? 0 : Mathf.Sign(impact.x), impact.y == 0 ? 0 : Mathf.Sign(impact.y), 0);
        SetLastOneWhoHittedMe(killer);

        if (marked)
        {
            playerMarked = true;
            PS_Marked.gameObject.SetActive(true);
            PS_Marked.Play();
        }

        if (stunnedByHit && currentImpactStunTimer > 0.25f)
        {
            if (marked)
                impactVelocity = impactNormalized * impactMarked * 1.5f;
            else
            {
                impactRelax = (impactVelocity.magnitude / residualStunImpact) * impact;
                impactRelax = new Vector3(Mathf.Abs(impactRelax.x) > maxStunVelocityLimit ? Mathf.Sign(impactRelax.x) * maxStunVelocityLimit : impactRelax.x, Mathf.Abs(impactRelax.y) > maxStunVelocityLimit ? Mathf.Sign(impactRelax.y) * maxStunVelocityLimit : impactRelax.y, 0);
                impactVelocity = impactRelax;
            }
            SetImpacts();
        }
        else
        {
            if (marked)
                impactVelocity = impactNormalized * impactMarked;
            else
            {
                impactRelax = impact;
                if (impact.magnitude >= maxNoStunVelocityLimit)
                    impactRelax = new Vector3(Mathf.Abs(impactRelax.x) != 0 ? Mathf.Sign(impactRelax.x) * maxNoStunVelocityLimit : 0, Mathf.Abs(impactRelax.y) != 0 ? Mathf.Sign(impactRelax.y) * maxNoStunVelocityLimit : 0, 0);
                impactVelocity = impactRelax;
            }
            SetImpacts();
            stunnedByHit = true;
            myAnim.SetBool("Stunned", true);
        }

        if (hittedByBerserk)
        {
            impactVelocity = impact;
            stunnedByHit = true;
            SetImpacts();
        }

        if (myLife > 0)
        {
            if (impact.x != 0)
                myAnim.Play("GetHit");
            else if (impact.y < 0)
            {
                if (!myAnim.GetBool("Grounded"))
                    myAnim.Play("GetHitDown");
                else
                    myAnim.Play("GetHitDown Bot");

            }
            else if (impact.y > 0)
                myAnim.Play("GetHitUp");
        }
        currentImpactStunTimer = 0;
    }

    #endregion

    #region Collisions, Colliders, Triggers

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var dir = Vector3.Dot(transform.up, hit.normal);
        if (!controller.isGrounded && dir == -1)
        {
            if (!stunnedByHit)
            {
                verticalVelocity = -hitHeadReject;
                moveVector.y = verticalVelocity;
            }
            else
            {
                verticalVelocity = -hitHeadReject;
                moveVector.y = verticalVelocity;
                stunnedByHit = false;
                playerMarked = false;
                myAnim.SetBool("Stunned", false);
            }
        }
        else if (Vector3.Angle(transform.up, hit.normal) >= 90)
        {
            touchingWall = true;
            if (stunnedByHit)
            {
                SmoothHitRefleject();
                stunnedByHit = false;
                playerMarked = false;
                myAnim.SetBool("Stunned", false);
            }
        }
        else
        {
            touchingWall = false;
            landedPlatform = hit.transform;
            canJump = false;
            if (stunnedByHit && impactVelocity.y < 0)
            {
                Debug.Log("A");
                impactVelocity = Vector3.zero;
                moveVector = Vector3.zero;
                myAnim.SetBool("Stunned", false);
            }
            else if (!stunnedByHit)
                impactVelocity = Vector3.zero;
            if (isFallingOff)
                isFallingOff = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PowerUp"))
        {
            chargedEffect = other.GetComponent<IPowerUp>().PowerUp();
            isCharged = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("DoorWarp"))
        {
            canJump = false;
            coyoteBool = false;
        }
    }
    #endregion

    #region UpdateLife
    public void SetDamage(float damage)
    {
        if (isDead)
            return;
        myLife -= Mathf.RoundToInt(damage);
        myLifeUI.TakeDamage(Mathf.RoundToInt(damage));
        if (myLife <= 0 && !isDead)
        {
            GameManager.Instance.SetKills(lastOneWhoHittedMe.GetComponent<PlayerInput>().player_number);
            canMove = false;
            myAnim.StopPlayback();
            myAnim.SetTrigger("Death");
            myAnim.Play("Death");
            this.enabled = false;
            controller.enabled = false;
            Destroy(gameObject, 1.1f); // estaba en 1.1f
            isDead = true;
        }
    }

    IEnumerator Death(float x)
    {
        while (true)
        {
            yield return new WaitForSeconds(x - 0.1f);
            isDead = true;
        }
    }

    #endregion

    #region Sets()
    private void SetMovements()
    {
        myMoves.Add(typeof(HorizontalMovement).ToString(), new HorizontalMovement(this));
        myMoves.Add(typeof(Jump).ToString(), new Jump(this));
        myMoves.Add(typeof(FallOff).ToString(), new FallOff(this));
    }

    private void SetAttacks()
    {
        //attacks.Add(typeof(NormalAttack).ToString(), new NormalAttack(this, normalAttackCoolDown));
        //attacks.Add(typeof(UpAttack).ToString(), new UpAttack(this, upAttackCoolDown));
        //attacks.Add(typeof(DownAttack).ToString(), new DownAttack(this, downAttackCoolDown));
        StartCoroutine(CanAttack(0.25f));
    }

    private void SetHabilities()
    {
        //hability.Add(typeof(Dash).ToString(), new Dash(this, dashCoolDown));
        //hability.Add(typeof(HookHability).ToString(), new HookHability(this, transform.ChildrenWithComponent<Hook>().First(), 1));
    }

    private void SetImpacts()
    {
        impactSpeed = Mathf.Abs(impactVelocity.magnitude);
        if (impactSpeed >= maxImpactToInfinitStun)
            impactStunMaxTimer = 100;
        else
            impactStunMaxTimer = impactSpeed / maxImpactToInfinitStun;
    }
    #endregion

}
