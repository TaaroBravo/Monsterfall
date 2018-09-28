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

    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.position += transform.up * speed * Time.deltaTime;
            _target = Physics.OverlapSphere(transform.position, 2f, 1 << 9).Where(x => x.GetComponent<PlayerController>() != null).Select(x => x.GetComponent<PlayerController>()).Where(x => x != _shooter).Where(x => !x.isDead).FirstOrDefault();
            if (_target)
                OnHitEnemy(_target, this);
            if (GameManager.Instance.OutOfLimits(transform.position))
                OnFailedHit(this);
        }
    }

    public void SetShooter(PlayerController rogue)
    {
        _shooter = rogue;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != _shooter.gameObject)
            OnFailedHit(this);
    }
}
