using System.Collections;
using System.Collections.Generic;
using FrameworkGoat.ObjectPool;
using UnityEngine;

public class TeleportHability : IHability
{

    Elf _elfPlayer;

    MissileTeleport _missilePrefab;
    MissileTeleport _currentMissile;

    float currentTimer;
    float maxTimer;
    bool activeTimer;
    bool missileMoving;

    public TeleportHability(PlayerController p, MissileTeleport missilePrefab, float _timerCoolDown = 0)
    {
        player = p;
        _elfPlayer = (Elf)p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _missilePrefab = missilePrefab;
        ObjectPoolManager.Instance.AddObjectPool<MissileTeleport>(InstantiateBullet, Initializate, Finalizate, 50, false);
    }

    public override void Update()
    {
        base.Update();
        if (activeTimer)
            maxTimer += Time.deltaTime;
        else if (missileMoving)
        {
            if (currentTimer < maxTimer)
                currentTimer += Time.deltaTime;
            else
            {
                ReturnBullet(_currentMissile);
                missileMoving = false;
            }
        }
    }

    public override void Hability()
    {
        activeTimer = true;
    }

    void Shoot(Vector3 spawnPoint, Vector3 dir)
    {
        MissileTeleport missile = ObjectPoolManager.Instance.GetObject<MissileTeleport>();
        missile.transform.position = spawnPoint;
        missile.SetDir(player, dir);
        missile.OnDestroyMissile += x => ReturnBullet(x);
        missile.OnHitPlayer += x => HitPlayer(x);
        _currentMissile = missile;
    }

    void HitPlayer(PlayerController p)
    {
        if (_elfPlayer.targets.Contains(p))
        {
            foreach (var target in _elfPlayer.targets)
            {
                if (target == p)
                    p.SetDamage(10);
            }
        }
        else
            p.SetDamage(5);
    }

    void Teleport()
    {
        player.transform.position = _currentMissile.transform.position;
    }

    public override void Release()
    {
        if (timerCoolDown < 0)
        {
            player.usingHability = true; //Quizas remplazar esto por una de dash
            FeedbackPlay();
            Shoot(player.transform.position, CalculateDirection());
            timerCoolDown = coolDown;
            activeTimer = false;
            missileMoving = true;
            player.lifeHUD.ActivateDashCD();
        }
    }

    void ResetValues()
    {
        player.usingHability = false;
        currentTimer = 0;
    }

    void FeedbackPlay()
    {
        //Particulas de disparo .Play()
        //Animación de disparo
    }

    #region Pool
    void ReturnBullet(MissileTeleport m)
    {
        Teleport();
        m.OnDestroyMissile -= x => ReturnBullet(x);
        m.OnHitPlayer -= x => HitPlayer(x);
        ObjectPoolManager.Instance.ReturnObject<MissileTeleport>(m);
    }

    MissileTeleport InstantiateBullet()
    {
        return GameObject.Instantiate(_missilePrefab);
    }

    void Initializate(MissileTeleport c)
    {
        c.gameObject.SetActive(true);
    }

    void Finalizate(MissileTeleport c)
    {
        c.gameObject.SetActive(false);
    }
    #endregion

    Vector3 CalculateDirection()
    {
        float x = player.GetComponent<PlayerInput>().MainHorizontal();
        float y = player.GetComponent<PlayerInput>().MainVertical();
        if (x + y == 0)
            x = Mathf.Sign(player.transform.localScale.z);
        int newX = (int)Mathf.Sign(x) * (int)Mathf.Abs(x);
        int newY = (int)Mathf.Sign(y) * (int)Mathf.Abs(y);
        return new Vector3(newX, newY);
        
    }
}
