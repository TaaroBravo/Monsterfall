using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MissileTeleport : MonoBehaviour {

    PlayerController player;
    List<PlayerController> _targets = new List<PlayerController>();
    public event Action<MissileTeleport> OnDestroyMissile = delegate { };
    public event Action<PlayerController> OnHitPlayer = delegate { };

    Vector3 _dir;
    float speed;

    void Start()
    {
        speed = 4;
    }

    void Update()
    {
        transform.position = _dir * speed * Time.deltaTime;

        if (Physics.OverlapSphere(transform.position, 3f, 1 << 19).Any())
            DestroyedMissile();

        var _target = Physics.OverlapSphere(transform.position, 2f, 1 << 9).Where(x => x.GetComponent<PlayerController>() != null).Select(x => x.GetComponent<PlayerController>()).Where(x => x != player).Where(x => !x.isDead).FirstOrDefault();
        if (_target && !_targets.Contains(_target))
        {
            _targets.Add(_target);
            OnHitPlayer(_target);
        }
    }

    public void SetDir(PlayerController p, Vector3 dir)
    {
        player = p;
        _dir = dir;
    }

    void ResetValues()
    {
        speed = 4;
        _targets.Clear();
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
