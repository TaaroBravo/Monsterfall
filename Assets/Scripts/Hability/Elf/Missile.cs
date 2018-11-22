using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Missile : MonoBehaviour {

    PlayerController player;
    PlayerController _target;
    public event Action<Missile> OnDestroyMissile = delegate { };
    public event Action<PlayerController> OnHitPlayer = delegate { };

    Vector3 _dir;
    float speed;

    Transform _objetive;

	void Start ()
    {
        speed = 1;
        StartCoroutine(TimeToDestroy());
	}
	
	void Update ()
    {
        transform.position = _dir * speed * Time.deltaTime;
        speed += Time.deltaTime * 3;
        if ((_objetive.position - transform.position).magnitude < 2f)
            Explote();
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
        speed = 1;
        _objetive = null;
    }

    void Explote()
    {
        foreach (var enemy in Physics.OverlapSphere(player.transform.position, 7f, 1 << 9).Where(x => x.GetComponent<PlayerController>()).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player).Where(x => !x.isDead))
        {
            enemy.ReceiveImpact((Vector3.right * Mathf.Sign((enemy.transform.position - transform.position).x) * 30), player);
            enemy.SetLastOneWhoHittedMe(player);
            OnHitPlayer(enemy);
        }
        DestroyedMissile();
    }

    IEnumerator TimeToDestroy()
    {
        while(true)
        {
            yield return new WaitForSeconds(6f);
            DestroyedMissile();
            break;
        }
    }

    public void DestroyedMissile()
    {
        ResetValues();
        ExploteFeedback();
        OnDestroyMissile(this);
    }
    
    void ExploteFeedback()
    {
        //Todo el feedback
    }
}
