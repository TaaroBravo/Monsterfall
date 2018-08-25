using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Hook : MonoBehaviour
{

    public float speed;
    public float targetTravelSpeed;

    public float maxDistance;
    private float _currentDistance;
    private float _currentTime;

    private Vector3 _startPosition;
    private Vector3 _playerPos;
    private Vector3 _direction;

    public bool fired;
    public bool hooked;
    public bool returnFail;

    private PlayerController _myPlayer;
    private PlayerController _target;

    public event Action<PlayerController> OnHookTarget = delegate { };
    public event Action<PlayerController> OnReachedTarget = delegate { };
    public event Action OnFailedFire = delegate { };

    void Start()
    {
        _myPlayer = FindMyPlayer(transform.parent);
        gameObject.SetActive(false);
    }

    void Update()
    {

        if (fired && !hooked)
        {
            transform.parent = null;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _direction, speed * Time.deltaTime);
            //_currentDistance = (transform.position - _playerPos).magnitude;
            _currentTime += Time.deltaTime;
            _currentDistance = speed * _currentTime;
            _target = Physics.OverlapSphere(transform.position, 2f, 1<<9).Select(x => x.GetComponent<PlayerController>()).Where(x => x != _myPlayer).FirstOrDefault();

            if (_currentDistance >= maxDistance)
                FailedFire();
        }

        if (returnFail)
            ReturnHook();

        if (_target)
            HookTarget(_target);

        if (hooked)
        {
            transform.parent = _target.transform;
            _target.transform.position = Vector3.MoveTowards(_target.transform.position, _playerPos, targetTravelSpeed * Time.deltaTime);
            if ((_playerPos - _target.transform.position).magnitude <= 1)
                ReachedTarget(_target);
        }
    }

    public void Fire(Vector3 dir)
    {
        fired = true;
        _startPosition = transform.localPosition;
        _playerPos = transform.position;
        _direction = ((_playerPos + dir) - _playerPos).normalized;
        transform.up = -_direction;
        _currentTime = 0;
    }

    public void HookTarget(PlayerController target)
    {
        hooked = true;
        target.canMove = false;
        _currentTime = 0;
        //target.transform.position = transform.position; Comentado: El target no cambia de posicion pero el gancho hace bulletrail raro. No comentado: Bullettrail bien pero demas no 
        target.GetComponent<CharacterController>().enabled = false;
        target.myAnim.Play("GetHit");
        OnHookTarget(target);
    }

    public void ReturnHook()
    {
        fired = false;
        hooked = false;
        transform.position = Vector3.MoveTowards(transform.position, _playerPos, speed * Time.deltaTime);
        if ((_playerPos - transform.position).magnitude <= 1)
        {
            transform.parent = _myPlayer.transform;
            transform.localPosition = _startPosition;
            returnFail = false;
            OnFailedFire();
            return;
        }
    }

    public void FailedFire()
    {
        _currentTime = 0;
        returnFail = true;
        fired = false;
        hooked = false;
    }

    public void ReachedTarget(PlayerController target)
    {
        OnReachedTarget(target);
        _target = null;
        hooked = false;
        fired = false;
        transform.parent = _myPlayer.transform;
        transform.localPosition = _startPosition;
        target.myAnim.Play("Stunned");
        target.GetComponent<CharacterController>().enabled = true;
    }

    PlayerController FindMyPlayer(Transform trans)
    {
        if (trans.GetComponent<PlayerController>())
            return trans.GetComponent<PlayerController>();
        return FindMyPlayer(trans.parent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DoorWarp"))
            other.gameObject.GetComponent<WarpController>().WarpHook(transform);
    }
}
