﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IceSpikes : MonoBehaviour {

    PlayerController player;
    PlayerController _target;

    public Ice icePrefab;

	void Update ()
    {
        transform.position += transform.forward * 50 * Time.deltaTime;
        if (Physics.OverlapSphere(transform.position, 0.25f, 1 << 19).Any())
            DestroySpike();
        _target = Physics.OverlapSphere(transform.position, 2.25f, 1 << 9).Where(x => x.GetComponent<PlayerController>() != null).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player).Where(x => !x.isDead).FirstOrDefault();
        if (_target)
            HitEnemy();

    }

    public void SetDir(PlayerController p, Vector3 dir, Ice myIce)
    {
        player = p;
        transform.forward = dir;
        Physics.IgnoreCollision(GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        icePrefab = myIce;
    }


    void DestroySpike()
    {
        //Feedback 
        AudioManager.Instance.CreateSound("HitWithIce");
        Destroy(gameObject);
    }

    void HitEnemy()
    {
        _target.myAnim.Play("GetHit");
        _target.SetStun(3.2f);
        _target.ResetVelocity();
        _target.SetDamage(5);
        _target.SetLastOneWhoHittedMe(player);
        var ice = GameObject.Instantiate(icePrefab);
        var iceDry = GameObject.Instantiate(((Yeti)player).explotePS);
        AudioManager.Instance.CreateSound("IceCreation");
        ice.transform.position = _target.transform.position;
        iceDry.transform.position = _target.transform.position;
        ((Yeti)player).frozenCharacter.Add(_target);
        ice.SetPlayer(player, _target);
        DestroySpike();
    }

}
