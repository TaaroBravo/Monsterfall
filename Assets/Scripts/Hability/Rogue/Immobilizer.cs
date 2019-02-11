using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Immobilizer : MonoBehaviour
{

    public event Action<PlayerController, Immobilizer> OnHitEnemy = delegate { };
    public event Action<Immobilizer> OnFailedHit = delegate { };

    PlayerController _shooter;
    PlayerController _target;

    public float speed;

    Vector3 direction;

    bool canEnter;

    private void Awake()
    {
        OnFailedHit += x => StopDestroy();
        OnHitEnemy += (x, y) => StopDestroy();
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.right = direction;
            transform.position += transform.right * speed * Time.deltaTime;
            if (Physics.OverlapSphere(transform.position, 0.5f, 1 << 19).Any())
                OnFailedHit(this);
            _target = Physics.OverlapSphere(transform.position, 2f, 1 << 9).Where(x => x.GetComponent<PlayerController>() != null).Select(x => x.GetComponent<PlayerController>()).Where(x => x != _shooter).Where(x => !x.isDead).FirstOrDefault();
            if (_target && canEnter)
            {
                canEnter = false;
                OnHitEnemy(_target, this);
            }
            if (GameManager.Instance.OutOfLimits(transform.position))
                OnFailedHit(this);
        }
    }

    public void SetShooter(PlayerController rogue, Vector3 dir)
    {
        _shooter = rogue;
        direction = dir;
        canEnter = true;
        StartCoroutine(OffObject());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
            OnFailedHit(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DoorWarp"))
        {
            WarpController door = other.gameObject.GetComponent<WarpController>();
            door.WarpHook(transform);
            StartCoroutine(OffParticles());
        }
    }

    void StopDestroy()
    {
        StopCoroutine(OffObject());
    }

    public void DestroyImmobilizer()
    {
        Destroy(gameObject);
    }

    IEnumerator OffObject()
    {
        yield return new WaitForSeconds(3.5f);
        if(gameObject.activeSelf)
            OnFailedHit(this);
    }

    IEnumerator OffParticles()
    {
        while (true)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            yield return new WaitForSeconds(0.1f);
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            break;
        }
    }
}
