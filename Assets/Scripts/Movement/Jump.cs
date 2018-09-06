using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : IMove {

    public Jump(PlayerController pl)
    {
        player = pl;
    }

    public override void Move()
    {
        player.PS_Jump.Play();
        player.verticalVelocity = player.jumpForce;
        player.moveVector.y = player.verticalVelocity;
        player.myAnim.SetBool("Jumping", true);
    }
}
