using System.Collections;
using System.Collections.Generic;
using FrameworkGoat.ObjectPool;
using UnityEngine;

public class ImmobilizerTrapHability : IHability
{

    Vector3 startPos;
    Vector3 dir;

    Immobilizer _bulletPrefab;

    public ImmobilizerTrapHability(PlayerController p, Immobilizer _immobilizer, float _timerCoolDown = 0)
    {
        player = p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _bulletPrefab = _immobilizer;
        ObjectPoolManager.Instance.AddObjectPool<Immobilizer>(InstantiateBullet, Initializate, Finalizate, 5, false);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            //Hacer un spawnpoint
            startPos = player.transform.position + (Vector3.up * 3);
            float x = player.GetComponent<PlayerInput>().MainHorizontal();
            float y = player.GetComponent<PlayerInput>().MainVertical();
            if (x + y == 0)
                x = Mathf.Sign(player.transform.localScale.z);

            Shoot(new Vector3(x, y, 0));
            player.usingHability = true;
            timerCoolDown = coolDown;
            player.lifeHUD.ActivateSkillCD();
        }
    }

    void Shoot(Vector3 dirPos)
    {
        //Hacer animacion de disparo.
        Vector3 _direction = ((startPos + dirPos) - startPos).normalized;
        Immobilizer immobilizer = ObjectPoolManager.Instance.GetObject<Immobilizer>();
        immobilizer.transform.position = startPos;
        immobilizer.SetShooter(player, _direction);
        immobilizer.OnFailedHit += x => ResetValues(x);
        immobilizer.OnHitEnemy += (x, y) =>
        {
            OnHitEnemy(x);
            ResetValues(y);
        };
    }

    void OnHitEnemy(PlayerController enemy)
    {
        enemy.myAnim.Play("Stunned");
        enemy.SetStun(1f);
        enemy.ResetVelocity();
        enemy.SetDamage(5);
        enemy.SetLastOneWhoHittedMe(player);
    }

    void ResetValues(Immobilizer immobilizer)
    {
        immobilizer.OnHitEnemy -= (x, y) =>
        {
            OnHitEnemy(x);
            ResetValues(y);
        };
        immobilizer.OnFailedHit -= x => ResetValues(x);
        ObjectPoolManager.Instance.ReturnObject<Immobilizer>(immobilizer);
        player.usingHability = false;
    }

    Immobilizer InstantiateBullet()
    {
        return GameObject.Instantiate(_bulletPrefab);
    }

    void Initializate(Immobilizer c)
    {
        c.gameObject.SetActive(true);
    }

    void Finalizate(Immobilizer c)
    {
        c.gameObject.SetActive(false);
    }

    public override void Release()
    {

    }
}
