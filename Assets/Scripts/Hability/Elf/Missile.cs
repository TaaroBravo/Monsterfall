using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Missile : MonoBehaviour
{

    PlayerController player;
    PlayerController _target;
    public event Action<Missile> OnDestroyMissile = delegate { };
    public event Action<PlayerController> OnHitPlayer = delegate { };

    public ParticleSystem explotion;

    Vector3 _dir;
    float speed;

    Transform _objetive;

    void Start()
    {
        speed = 4;
        StartCoroutine(TimeToDestroy());
    }

    void Update()
    {
        transform.position += _dir * speed * Time.deltaTime;
        speed += Time.deltaTime * 9;
        if (_objetive)
        {
            _dir = (_objetive.position - transform.position).normalized;
            if ((_objetive.position - transform.position).magnitude < 5f || Physics.OverlapSphere(transform.position, 5, 1 << 9).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => x.transform == _objetive).Any())
                Explote();
        }
    }

    public void SetDir(PlayerController p, Vector3 dir)
    {
        player = p;
        _dir = dir;
    }

    public void SetObjetive(PlayerController p, Transform objetive, PlayerController target = null)
    {
        player = p;
        _objetive = objetive;
        _target = target;
    }

    void ResetValues()
    {
        speed = 4;
        _objetive = null;
    }

    void Explote()
    {
        var layerMask1 = 1 << 8;
        var layerMask2 = 1 << 9;
        var layerMask = layerMask1 | layerMask2;
        foreach (var enemy in Physics.OverlapSphere(transform.position, 5, layerMask).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player).Where(x => !x.isDead))
        {
            if (!enemy.stunnedByHit)
            {
                OnHitPlayer(enemy);
                enemy.ReceiveImpact((Vector3.right * Mathf.Sign((enemy.transform.position - transform.position).x) * 30), player);
                enemy.SetLastOneWhoHittedMe(player);
            }
        }
        DestroyedMissile();
    }

    IEnumerator TimeToDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            DestroyedMissile();
            break;
        }
    }

    public void DestroyedMissile()
    {
        StartCoroutine(EndMissile());
    }

    IEnumerator EndMissile()
    {
        while (true)
        {
            ExploteFeedback();
            yield return new WaitForSeconds(2f);
            ResetValues();
            OnDestroyMissile(this);
            break;
        }
    }

    void ExploteFeedback()
    {
        if (!explotion.isPlaying)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            explotion.Play();
        }
    }
}
