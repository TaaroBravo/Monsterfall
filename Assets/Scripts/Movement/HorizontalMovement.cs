using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : IMove
{
    float movement;
    float maxSpeedTimer;
    float slowSpeedCharge;
    float currentSpeedTimer;

    float scale;

    public HorizontalMovement(PlayerController pl)
    {
        player = pl;
        currentSpeedTimer = 1;
        maxSpeedTimer = player.maxSpeedChargeTimer;
        slowSpeedCharge = player.slowSpeedCharge;
        scale = Mathf.Abs(player.transform.localScale.x);
    }

    public override void Update()
    {
        base.Update();

        if (player.controller.isGrounded)
        {
            player.verticalVelocity = -player.gravity * Time.deltaTime;
            if (!player.isDead)
            {
                //Hacerlo con un bool en la animacion
                player.myAnim.SetBool("Jumping", false);
                player.myAnim.SetBool("Grounded", true);
            }
        }
        else
        {
            if (player.verticalVelocity >= 0)
                player.verticalVelocity -= player.gravity * Time.deltaTime;
            else
                player.verticalVelocity -= player.gravity * (Time.deltaTime * 2);
            if (player.verticalVelocity < -40)
                player.verticalVelocity = -40;
            if (!player.isDead)
                player.myAnim.SetBool("Grounded", false);
        }

        movement = player.GetComponent<PlayerInput>().MainHorizontal();
        if (movement != 0)
        {
            if (!player.myAnim.GetBool("Jumping") && !player.isDead)
                player.myAnim.SetBool("Running", true);
            currentSpeedTimer += Time.deltaTime / slowSpeedCharge;

            if (currentSpeedTimer >= maxSpeedTimer)
                currentSpeedTimer = maxSpeedTimer;
        }
        else
        {
            currentSpeedTimer = 1;
            if (!player.isDead)
                player.myAnim.SetBool("Running", false);
        }
        if (player.stunnedByHit)
            player.verticalVelocity = 0;
    }

    public override void Move()
    {
        if (player.canMove)
        {
            if (movement > 0)
                player.transform.localScale = new Vector3(-scale, scale, scale);
            else if (movement < 0)
                player.transform.localScale = new Vector3(-scale, scale, -scale);

            if (!player.controller.isGrounded && movement == 0 && player.moveVector.x != 0)
                player.moveVector.x += -Mathf.Sign(player.moveVector.x) * 1.5f;
            else
                player.moveVector.x = movement * currentSpeedTimer * player.moveSpeed + player.impactVelocity.x;
        }
        else if (!player.usingHability)
        {
            float currentX = player.moveVector.x;
            float newX = currentX - (Mathf.Sign(currentX) * Time.deltaTime * 2);
            if (Mathf.Round(newX) == 0)
                player.moveVector.x = 0;
            else
                player.moveVector.x = newX;
        }

        if (GameManager.Instance.finishedGame)
            player.moveVector.x = 0;
        if (!player.stunnedByHit)
            player.moveVector.y = player.verticalVelocity;
        else
            player.moveVector.y = 0;
        player.moveVector.z = 0;
    }
}
