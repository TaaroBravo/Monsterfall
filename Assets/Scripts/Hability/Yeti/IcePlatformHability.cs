using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IcePlatformHability : IHability
{
    Yeti _yeti;
    IcePlatform _platformPrefab;

    public IcePlatformHability(PlayerController p, IcePlatform platformPrefab, float _timerCoolDown = 0)
    {
        player = p;
        _yeti = (Yeti)p;
        timerCoolDown = 2;
        coolDown = _timerCoolDown;
        _platformPrefab = platformPrefab;
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            FeedbackPlay();
            SpawnPlatform();
            timerCoolDown = coolDown;
            player.lifeHUD.ActivateDashCD();
        }
    }

    public override void Release()
    {

    }

    void SpawnPlatform()
    {
        var platformIce = GameObject.Instantiate(_platformPrefab);
        platformIce.transform.position = _yeti.transform.position + (new Vector3(3, 0, 0) * Mathf.Sign(_yeti.transform.localScale.z));
        platformIce.SetPlayer(player);
    }

    void FeedbackPlay()
    {
        //Poner Feedback de explosion de hielo
    }
}
