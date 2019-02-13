using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class IceSpikesHability : IHability
{
    Yeti _yeti;
    IceSpikes _spikesPrefab;
    bool usingHability;

    public IceSpikesHability(PlayerController p, IceSpikes spikesPrefab, float _timerCoolDown = 0)
    {
        player = p;
        _yeti = (Yeti)p;
        _spikesPrefab = spikesPrefab;
        timerCoolDown = 2;
        coolDown = _timerCoolDown;
    }

    public override void Hability()
    {
        if (timerCoolDown < 0 && !usingHability)
        {
            //usingHability = true;
            //player.usingHability = true;
            Shoot();
            FeedbackPlay();
            timerCoolDown = coolDown;
            player.lifeHUD.ActivateSkillCD();
        }
    }

    void Shoot()
    {
        int amount = UnityEngine.Random.Range(8, 10);
        var radius = 2;

        for (int i = 0; i < amount; i++)
        {
            IceSpikes spike = GameObject.Instantiate(_spikesPrefab);

            float angle = i * Mathf.PI * 2 / 8;
            Vector3 newPos = new Vector3(_yeti.transform.position.x - Mathf.Cos(angle) * radius, _yeti.transform.position.y -Mathf.Sin(angle) * radius, _yeti.transform.position.z);

            spike.transform.position = newPos;
            Vector3 dir = (newPos - _yeti.transform.position).normalized;
            spike.SetDir(player, dir);
            //spike.ChangeColor(_yeti.GetComponent<PlayerInput>().player_number);
        }
    }

    public override void Release()
    {

    }

    void FeedbackPlay()
    {
        //player.GetComponent<FeedbackElf>().shootFire.Play();
        //player.myAnim.Play("ShootHability");
    }

}
