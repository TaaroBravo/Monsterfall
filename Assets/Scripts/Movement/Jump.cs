using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : IMove
{
    private ParticleSystemRenderer psr;
    string key;

    public Jump(PlayerController pl)
    {
        player = pl;
        if (player is Pirate || player is Elf)
            key = "JumpB";
        else if (player is Rogue)
            key = "JumpC";
        else if (player is Yeti)
            key = "JumpD";
        else if (player is Knight || player is Berserk)
            key = "JumpA";
    }

    public override void Move()
    {
        //RaycastHit hit;
        //if (Physics.Raycast(player.transform.position, -Vector3.up, out hit, player.GetComponent<Collider>().bounds.extents.y + 0.5f))
        //{
        //    if (hit.collider)
        //    {
        //        //player.PS_Jump.transform.parent.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        //    }
        //}
       
        AudioManager.Instance.CreateSound(key);
        player.verticalVelocity = player.jumpForce;
        player.moveVector.y = player.verticalVelocity;
        player.myAnim.SetBool("Jumping", true);
        var jumpParticle = GameObject.Instantiate(player.PS_Jump);
        psr = jumpParticle.GetComponent<ParticleSystemRenderer>();
        //psr.pivot = new Vector3(0, 0.65f, 0);
        psr.pivot = new Vector3(0, 0.4f, 0);
        psr.gameObject.AddComponent<DestroyableObject>();
        psr.GetComponent<DestroyableObject>().timeToDestroy = 2;
        jumpParticle.transform.position = player.transform.position;
        jumpParticle.transform.localScale = Vector3.one * 3;
        jumpParticle.Play();
        //player.PS_Jump.Play();
    }
}
