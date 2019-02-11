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
    public event Action OutOfScreen = delegate { };

    public ParticleSystem explotion;

    Vector3 _dir;
    float speed;

    Transform _objetive;
    bool exploted;
    public bool inScreen;

    public projectileTrail projectile;
    int _ID;

    void Start()
    {
        speed = 10;
        StartCoroutine(TimeToDestroy());
        StartCoroutine(OutOfLimitsTimer());
    }

    void Update()
    {
        if (!GameManager.Instance.OutOfLimits(transform.position))
            inScreen = true;
        else
            OutOfScreen();
        transform.position += _dir * speed * Time.deltaTime;
        speed += Time.deltaTime * 20;
        if (_objetive && !exploted)
        {
            Vector3 newPos = (_objetive.position - transform.position).normalized;
            _dir = Vector3.Lerp(_dir, newPos, Time.deltaTime);
            //_dir = (_objetive.position - transform.position).normalized;
            if ((_objetive.position - transform.position).magnitude < 3f || Physics.OverlapSphere(transform.position, 3, 1 << 9).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => x.transform == _objetive).Any())
            {
                exploted = true;
                if (_target)
                    projectile.GetComponent<LineRenderer>().positionCount = 0;
                Explote();
            }
        }
        if (_target)
            projectile.SetPositions(transform.position, _target.transform.position + new Vector3(0, _target.GetComponent<Collider>().bounds.extents.y));

    }

    IEnumerator OutOfLimitsTimer()
    {
        while (true)
        {
            yield return new WaitUntil(() => inScreen);
            yield return new WaitForSeconds(1f);
            if (!exploted && GameManager.Instance.OutOfLimits(transform.position))
            {
                exploted = true;
                Explote();
            }
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
        speed = 10;
        _objetive = null;
        _target = null;
        exploted = false;
        inScreen = false;
    }

    void Explote()
    {
        var layerMask1 = 1 << 8;
        var layerMask2 = 1 << 9;
        var layerMask = layerMask1 | layerMask2;
        List<PlayerController> playersHitted = new List<PlayerController>();
        foreach (var enemy in Physics.OverlapSphere(transform.position, 5, layerMask).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => !playersHitted.Contains(x)).Where(x => x != player).Where(x => !x.isDead))
        {
            if (!enemy.stunnedByHit && !playersHitted.Contains(enemy))
            {
                playersHitted.Add(enemy);
                OnHitPlayer(enemy);
                enemy.ReceiveImpact((Vector3.right * Mathf.Sign((enemy.transform.position - transform.position).x) * 30), player);
                enemy.myAnim.Play("GetHit");
                enemy.SetLastOneWhoHittedMe(player);
                DestroyedMissile();
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
        exploted = true;
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
            if (_target)
                _target.GetComponent<GeneralFeedback>().FinishCrosshair();
            explotion.Play();
        }
    }

    public void DestroyMissile()
    {
        //GetComponent<FeedbackMissile>().destroysound.Play();
        Destroy(gameObject);
    }

    public void ChangeColor(int ID)
    {
        GetComponent<FeedbackMissile>().ChangeColor(ID);
        _ID = ID;
        if (_target)
        {
            projectile.SetColor(ID);
            _target.GetComponent<GeneralFeedback>().StartCrosshair(ID);
        }
    }
}
