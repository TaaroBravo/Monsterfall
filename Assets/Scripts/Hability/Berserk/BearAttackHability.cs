using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BearAttackHability : IHability
{
    float _power;
    float _dir;

    PlayerController _target;

    float maxTimer;
    float timerActive;

    float _damage;
    bool hasTarget;

    bool failed;

    Berserk berserkPlayer;
    bool isGrounded;

    public BearAttackHability(Berserk _player, float power, float damage, float activeTime, float _cooldown)
    {
        player = _player;
        berserkPlayer = _player;
        timerActive = activeTime;
        maxTimer = activeTime;
        _power = power;
        _damage = damage;
        timerCoolDown = _cooldown;
        coolDown = _cooldown;
        berserkPlayer.OnRecovery += ResetValues;
    }

    public override void Update()
    {
        base.Update();
        if (!berserkPlayer.chargeAttack && berserkPlayer.recovery && !berserkPlayer.usingBearHability)
            ResetValues();

        if (player.usingHability && !failed && !berserkPlayer.chargeAttack && berserkPlayer.usingBearHability)
        {
            if (!isGrounded)
                JumpAttack();
            if (_target && !hasTarget)
            {
                _target.canInteract = false;
                AttackingFeedback();
                player.StartCoroutine(AttackTimer());
                hasTarget = true;
            }
            else if (!_target)
                _target = Physics.OverlapSphere(player.transform.position, 1f, 1 << 9).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player).Where(x => !x.isDead).FirstOrDefault();
            if (_target && hasTarget)
            {
                _target.transform.position = player.transform.position;
                _target.myAnim.Play("GetHitDown");
            }
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            ResetValues();
            AudioManager.Instance.CreateSound("ChargingBerserk");
            //_dir = Mathf.Sign(player.transform.localScale.z);
            player.verticalVelocity = player.jumpForce;
            player.canInteract = false;
            player.usingHability = true;
            berserkPlayer.usingBearHability = true;
            timerActive = maxTimer;
            player.myAnim.Play("SkillLoad");
            player.StartCoroutine(LoadAttack());
            player.StartCoroutine(IsGrounded());
            timerCoolDown = coolDown;
            player.lifeHUD.ActivateSkillCD();
            player.StartCoroutine(CanNotAttack());
            //JumpAttack();
        }
    }

    IEnumerator LoadAttack()
    {
        yield return new WaitForSeconds(0.2f);
        AudioManager.Instance.CreateSound("JumpingBerserk");
        JumpAttack();
    }

    void JumpAttack()
    {
        player.moveVector.x = _power * Mathf.Sign(player.transform.localScale.z);
        //player.verticalVelocity = player.jumpForce;
        player.moveVector.y = player.verticalVelocity;
        player.controller.Move(player.moveVector * Time.deltaTime);
        player.myAnim.SetBool("Jumping", true);
    }

    void AttackingFeedback()
    {
        //player.myAnim.Play("SkillAttack");
        AudioManager.Instance.CreateSound("AttackingAbilityBerserk");
        _target.myAnim.Play("GetHitDown");
        _target.SetStun(3);
    }

    IEnumerator CanNotAttack()
    {
        yield return new WaitForSeconds(3f);
        ResetValuesForTime();
    }

    IEnumerator IsGrounded()
    {
            yield return new WaitForSeconds(0.7f);
            yield return new WaitUntil(() => player.controller.isGrounded/* || player.IsCloseToGround()*/);
            player.ResetVelocity();
            if (!_target)
                FailAttack();
    }

    IEnumerator AttackTimer()
    {
        while (timerActive > 0)
        {
            player.myAnim.Play("SkillAttack");
            yield return new WaitUntil(() => player.controller.isGrounded /*|| player.IsCloseToGround() || player.verticalVelocity > -0.6 && player.verticalVelocity < 0*/);
            yield return new WaitForSeconds(0.3f);
            player.ResetVelocity();
            isGrounded = true;
            AttackTarget();
            var slash = GameObject.Instantiate(player.GetComponent<BerserkerParticlesManager>().scratch);
            slash.transform.position = player.GetComponent<Collider>().bounds.center - new Vector3(0, 0, 5);
            timerActive -= 0.3f;
        }
        player.myAnim.SetTrigger("SkillAttackOut");
        player.canInteract = true;
        player.usingHability = false;
        berserkPlayer.recovery = false;
        player.StartCoroutine(BackTargetToNormal());
    }

    IEnumerator BackTargetToNormal()
    {
        if (_target)
            _target.SetStun(1.5f);
        yield return new WaitForSeconds(1.5f);
        ResetValues();
    }

    void AttackTarget()
    {
        if (_target)
        {
            _target.SetDamage(_damage);
            _target.SetLastOneWhoHittedMe(player);
        }
    }

    void FailAttack()
    {
        failed = true;
        player.myAnim.Play("SkillLanding");
    }

    void ResetValuesForTime()
    {
        if (_target)
        {
            _target.transform.parent = null;
            _target.canInteract = true;
            _target.canMove = true;
        }
        berserkPlayer.recovery = false;
        failed = false;
        hasTarget = false;
        _target = null;
        timerActive = 0;
        timerCoolDown = coolDown;
        player.canInteract = true;
        player.usingHability = false;
        player.canMove = true;
        berserkPlayer.usingBearHability = false;
        isGrounded = false;
    }

    void ResetValues()
    {
        if (_target)
        {
            _target.transform.parent = null;
            _target.canInteract = true;
        }
        berserkPlayer.recovery = false;
        failed = false;
        hasTarget = false;
        _target = null;
        timerActive = 0;
        timerCoolDown = coolDown;
        player.canInteract = true;
        player.canMove = true;
        player.usingHability = false;
        berserkPlayer.usingBearHability = false;
        isGrounded = false;
        player.StopCoroutine(CanNotAttack());
    }

    public override void Release()
    {

    }
}
