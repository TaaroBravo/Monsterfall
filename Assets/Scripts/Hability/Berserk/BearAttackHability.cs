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

    public BearAttackHability(Berserk _player, float power, float damage, float activeTime)
    {
        player = _player;
        berserkPlayer = _player;
        timerActive = activeTime;
        maxTimer = activeTime;
        _power = power;
        _damage = damage;
    }

    public override void Update()
    {
        base.Update();
        if (berserkPlayer.recovery)
            ResetValues();

        if (player.usingHability && !failed)
        {
            if (_target && !hasTarget)
            {
                _target.canMove = false;
                AttackingFeedback();
                player.StartCoroutine(AttackTimer());
                hasTarget = true;
            }
            else if (!_target)
                _target = Physics.OverlapSphere(player.transform.position, 1f, 1 << 9).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player).Where(x => !x.isDead).FirstOrDefault();
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            ResetValues();
            timerActive = maxTimer;
            _dir = Mathf.Sign(player.transform.localScale.z);
            JumpAttack();
            player.canMove = false;
            player.usingHability = true;
        }
    }

    void JumpAttack()
    {
        player.GetComponent<KnightFeedbackController>().fireEstela.Play();
        player.moveVector.x = _power * Mathf.Sign(player.transform.localScale.z) * 2;
        player.verticalVelocity = player.jumpForce / 3;
        player.moveVector.y = player.verticalVelocity;
        player.controller.Move(player.moveVector * Time.deltaTime);
        player.myAnim.SetBool("Jumping", true);
    }

    void AttackingFeedback()
    {
        //Animaciones
    }

    IEnumerator IsGrounded()
    {
        while (true)
        {
            yield return new WaitUntil(() => player.controller.isGrounded);
            if (!_target)
                FailAttack();
            break;
        }
    }

    IEnumerator AttackTimer()
    {
        while (timerActive > 0)
        {
            yield return new WaitForSeconds(0.3f);
            AttackTarget();
            timerActive -= 0.3f;
        }
        ResetValues();
    }

    void AttackTarget()
    {
        _target.SetDamage(_damage);
    }

    void FailAttack()
    {
        failed = true;
        //Darle play a animación
    }

    void ResetValues()
    {
        if (_target)
        {
            _target.transform.parent = null;
            _target.canMove = true;
        }
        berserkPlayer.recovery = false;
        failed = false;
        hasTarget = false;
        _target = null;
        timerActive = 0;
        timerCoolDown = coolDown;
        player.canMove = true;
        player.usingHability = false;
    }
}
