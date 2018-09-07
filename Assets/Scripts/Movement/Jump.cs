using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : IMove
{

    public Jump(PlayerController pl)
    {
        player = pl;
    }

    public override void Move()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, -Vector3.up, out hit, player.GetComponent<Collider>().bounds.extents.y + 0.5f))
        {
            if (hit.collider)
            {
                player.PS_Jump.transform.position = hit.point;
                player.PS_Jump.Play();
            }
        }
        player.verticalVelocity = player.jumpForce;
        player.moveVector.y = player.verticalVelocity;
        player.myAnim.SetBool("Jumping", true);
    }
}
