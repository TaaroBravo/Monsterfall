using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ForwardCharge : IHability
{

    float _speed;
    float _dir;

    PlayerController _target;

    float distance;
    float timerActive;

    Berserk berserkPlayer;

    public ForwardCharge(PlayerController p, float speed, float _timerCoolDown = 0)
    {
        player = p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _speed = speed;
        berserkPlayer = (Berserk)p;
    }

    public override void Update()
    {
        base.Update();

        if (player.usingHability && berserkPlayer.chargeAttack)
        {
            if (player.IsTouchingWalls())
            {
                if (_target)
                    DamageTarget();
                ResetValues();
                return;
            }
            timerActive += Time.deltaTime;
            player.moveVector.x = _dir * _speed;
            distance = Mathf.RoundToInt(_speed * timerActive / 5) * 5;
            if (_target)
                Charge();
            else
                _target = Physics.OverlapSphere(player.transform.position, 1f, 1 << 9).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player).Where(x => !x.isDead).FirstOrDefault();
        }
        else if(!player.canMove && berserkPlayer.chargeAttack)
        {
            player.myAnim.Play("Stunned");
            ResetValues();
        }
        else
        {
            player.GetComponent<BerserkerParticlesManager>().StopCharge();
        }
    }

    IEnumerator TimeToDisable()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            if (_target)
                DamageTarget();
            ResetValues();
            break;
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            ResetValues();
            timerActive = 0;
            distance = 0;
            _dir = Mathf.Sign(player.transform.localScale.z);
            player.canMove = false;
            berserkPlayer.chargeAttack = true;
            player.usingHability = true;
            player.myAnim.Play("Dash");
            player.myAnim.SetBool("Dashing", true);
            player.StartCoroutine(TimeToDisable());
            player.lifeHUD.ActivateDashCD();
        }
    }

    void Charge()
    {
        if(_target is Berserk && ((Berserk)_target).chargeAttack)
        {
            berserkPlayer.chargeAttack = false;
            ((Berserk)_target).chargeAttack = false;
            var dirPlayer = Mathf.Sign(player.transform.localScale.z);
            var dirTarget = Mathf.Sign(_target.transform.localScale.z);
            _target.ReceiveImpact(Vector3.right * dirPlayer * 50, player);
            player.ReceiveImpact(Vector3.right * dirTarget * 50, _target);
            ResetValues();
        }
        else
        {
            _target.hittedChargeBerserk = true;
            _target.transform.position = player.transform.position + (Vector3.right * Mathf.Sign(player.transform.localScale.z));
            var enemies = Physics.OverlapSphere(player.transform.position, 1f, 1 << 9).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player && x != _target).Where(x => !x.isDead);
            foreach (var _enemy in enemies)
                _enemy.ReceiveImpact((Vector3.right * Mathf.Sign(player.transform.localScale.z) * 30), player);
        }
    }

    void DamageTarget()
    {
        _target.transform.parent = null;
        _target.hittedChargeBerserk = false;
        _target.SetStun(0.2f);
        int dmg = Mathf.RoundToInt((distance / 1.3f) / 5) * 5;
        if (dmg > 35)
            dmg = 35;
        _target.SetDamage(dmg);
        _target.SetLastOneWhoHittedMe(player);
    }

    void ResetValues()
    {
        if (_target)
        {
            _target.transform.parent = null;
            _target.hittedChargeBerserk = false;
        }
        player.GetComponent<BerserkerParticlesManager>().StopCharge();
        player.myAnim.SetBool("Dashing", false);
        _target = null;
        timerActive = 0;
        distance = 0;
        timerCoolDown = coolDown;
        player.canMove = true;
        player.usingHability = false;
        berserkPlayer.chargeAttack = false;
    }

    public override void Release()
    {

    }
}
