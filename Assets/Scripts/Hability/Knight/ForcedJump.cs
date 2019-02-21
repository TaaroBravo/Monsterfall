using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ForcedJump : IHability
{
    float _power;

    public ForcedJump(PlayerController p, float power, float _timerCoolDown = 0)
    {
        player = p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _power = power;
    }

    public override void Update()
    {
        base.Update();
        if (player.hittedChargeBerserk)
        {
            ForceReset();
            player.hittedChargeBerserk = false;
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0 && !player.usingHability)
        {
            player.usingHability = true;
            player.canInteract = false;
            JumpPlayer();
            player.StartCoroutine(WaitToLand());
            player.StartCoroutine(ResetValuesCoroutine());
            ((Knight)player).CallJumpHabilityCoroutine();
            AudioManager.Instance.CreateSound("FireJump");
            player.lifeHUD.ActivateDashCD();
        }
    }

    IEnumerator ResetValuesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            player.canInteract = true;
            player.usingHability = false;
            break;
        }
    }

    IEnumerator WaitToLand()
    {
        while (true)
        {
            ((Knight)player).forcedJumping = true;
            yield return new WaitForSeconds(0.3f);
            if (player.IsTouchingWalls())
                player.canInteract = true;
            yield return new WaitUntil(() => player.controller.isGrounded /*|| player.IsTouchingWalls()*/);
            player.GetComponent<KnightFeedbackController>().fireEstela.Stop();
            //player.GetComponent<KnightFeedbackController>().fireEstela.gameObject.SetActive(false);
            if (player.IsTouchingWalls())
                player.canInteract = true;
            ElevatePlayers();
            ((Knight)player).forcedJumping = false;
            break;
        }
    }

    void ElevatePlayers()
    {
        var targets = GameManager.Instance.myPlayers.Where(x => x != player)
                                        .Where(x => x.controller.isGrounded)
                                        .Where(x => (x.transform.position - player.transform.position).magnitude < 5f);
        if (targets.Any())
        {
            foreach (var target in targets)
            {
                target.verticalVelocity = target.jumpForce;
                target.moveVector.y = target.verticalVelocity;
                target.controller.Move(target.moveVector * Time.deltaTime);
                target.SetStun(0.5f);
            }
        }
        player.GetComponent<KnightFeedbackController>().PlayLanding();
        ResetValues();
    }

    void JumpPlayer()
    {
        player.GetComponent<KnightFeedbackController>().fireEstela.gameObject.SetActive(true);
        player.GetComponent<KnightFeedbackController>().fireEstela.Play();
        player.moveVector.x = _power * Mathf.Sign(player.transform.localScale.z) * 2;
        player.verticalVelocity = player.jumpForce / 2;
        player.moveVector.y = player.verticalVelocity;
        player.controller.Move(player.moveVector * Time.deltaTime);
        player.myAnim.SetBool("Jumping", true);
    }

    void ForceFall()
    {
        player.verticalVelocity = -60;
        player.moveVector.y = player.verticalVelocity;
        player.controller.Move(player.moveVector * Time.deltaTime);
        player.myAnim.Play("Forced Fall");
    }

    void ResetValues()
    {
        timerCoolDown = coolDown;
        player.canInteract = true;
        player.usingHability = false;
        ((Knight)player).forcedJumping = false;
    }

    void ForceReset()
    {
        timerCoolDown = coolDown;
        player.canInteract = true;
        player.usingHability = false;
        ((Knight)player).forcedJumping = false;
        player.StopCoroutine(WaitToLand());
        player.StopCoroutine(ResetValuesCoroutine());
    }

    public override void Release()
    {

    }
}
