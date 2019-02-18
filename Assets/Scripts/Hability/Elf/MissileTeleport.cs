using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MissileTeleport : MonoBehaviour
{

    PlayerController player;
    List<PlayerController> _targets = new List<PlayerController>();
    public event Action<MissileTeleport> OnDestroyMissile = delegate { };
    public event Action<PlayerController> OnHitPlayer = delegate { };

    Vector3 _dir;
    float speed;

    Vector3 playerDir;

    void Start()
    {
        speed = 35;
    }

    void Update()
    {
        if (_dir == Vector3.zero || _dir.magnitude < 1)
            _dir = playerDir;
        transform.position += _dir * speed * Time.deltaTime;

        if (Physics.OverlapSphere(transform.position, 3f, 1 << 17).Any() || Physics.OverlapSphere(transform.position, 3f, 1 << 19).Where(x => x.tag == "Limit").Any())
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
        if (dir != Vector3.zero)
            _dir = dir;
        else
            _dir = new Vector3(Mathf.Sign(player.transform.localScale.z), 0, 0);
        playerDir = new Vector3(Mathf.Sign(player.transform.localScale.z), 0, 0);
    }

    void ResetValues()
    {
        speed = 35;
        _targets.Clear();
    }

    public void DestroyedMissile()
    {
        AudioManager.Instance.CreateSound("TeleportingElf");
        ResetValues();
        ExploteFeedback();
        OnDestroyMissile(this);
    }

    void ExploteFeedback()
    {
        //Todo el feedback
    }

    public void DestroyMissile()
    {
        Destroy(gameObject);
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

    IEnumerator OffParticles()
    {
        while (true)
        {
            transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
            yield return new WaitForSeconds(0.1f);
            transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            break;
        }
    }

    public void ChangeColor(int ID)
    {
        GetComponent<ElfTeleportMissileFeedback>().ChangeColor(ID);
    }
}
