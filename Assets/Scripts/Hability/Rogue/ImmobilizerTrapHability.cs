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
        ObjectPoolManager.Instance.AddObjectPool<Immobilizer>(InstantiateBullet, Initializate, Finalizate, 3, false);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            startPos = player.transform.position;
            JumpPlayer();
            player.StartCoroutine(ShootImmobilizer(0.3f));
            player.usingHability = true;
            timerCoolDown = coolDown;
        }
    }

    IEnumerator ShootImmobilizer(float x)
    {
        while(true)
        {
            yield return new WaitForSeconds(x);
            Shoot(player.transform.position);
            break;
        }
    }

    void JumpPlayer()
    {
        player.verticalVelocity = player.jumpForce;
        player.moveVector.y = player.verticalVelocity;
        player.myAnim.SetBool("Jumping", true);
    }

    void Shoot(Vector3 finalPos)
    {
        //Hacer animacion de disparo.
        dir = (startPos - player.transform.position).normalized;

        Immobilizer immobilizer = ObjectPoolManager.Instance.GetObject<Immobilizer>();
        immobilizer.transform.position = player.transform.position;
        immobilizer.transform.up = dir;
        immobilizer.SetShooter(player);
        immobilizer.OnFailedHit += x => ResetValues(x);
        immobilizer.OnHitEnemy += (x, y) =>
        {
            OnHitEnemy(x);
            ResetValues(y);
        };
    }

    void OnHitEnemy(PlayerController enemy)
    {
        enemy.SetStun(0.2f);
        enemy.SetDamage(10);
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
}
