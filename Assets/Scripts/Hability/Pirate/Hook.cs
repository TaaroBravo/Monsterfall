using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Hook : MonoBehaviour
{
    public event Action OnFireHook = delegate { };
    public event Action OnReturnedEnd = delegate { };
    public event Action<PlayerController> OnHookTarget = delegate { };
    public event Action<PlayerController> OnReachedTarget = delegate { };
    public event Action OnFailedFire = delegate { };
    public event Action<Vector3, Vector3> OnTeleport = delegate { };

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

    bool canEnterTeleport;
    public bool teleportedBack;

    public bool hookGrabbed;

    public Transform spawnPoint;
    public Transform endPoint;

    private PlayerController _myPlayer;
    private PlayerController _target;
    private PlayerController _playerGrabbedHook;
    private Transform _warpedPos;
    private Transform _whereIWarped;

    private ParticleSystem _psParry;

    PlayerContrains contrains;
    Vector3 playerSpawnPos;
    Vector3 playerEndPos;
    bool playerTeleported;

    private void Awake()
    {
        contrains = transform.parent.GetComponent<PlayerContrains>();
    }
    void Start()
    {
        _myPlayer = FindMyPlayer(transform.parent);
        gameObject.SetActive(false);
        _psParry = transform.ChildrenWithComponent<ParticleSystem>().Where(x => x != null).First();
        teleportedBack = true;
        contrains.OnTeleportPlayer += (x, y) =>
        {
            PlayerTeleported(x, y);
        };
    }

    void Update()
    {
        if (!_myPlayer)
            Destroy(gameObject);


        if (hookGrabbed)
        {
            CounterHook();
            return;
        }

        if (fired && !hooked)
        {
            transform.parent = null;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _direction, speed * Time.deltaTime);
            _currentTime += Time.deltaTime;
            _currentDistance = speed * _currentTime;
            _target = Physics.OverlapSphere(transform.position, 2f, 1 << 9).Select(x => x.GetComponent<PlayerController>()).Where(x => x != _myPlayer).Where(x => !x.isDead).FirstOrDefault();

            if (_currentDistance >= maxDistance)
                FailedFire();
        }

        if (returnFail)
            ReturnHook();

        if (_target)
            HookTarget(_target);

        if (hooked)
        {
            if (!_target)
                ReturnHook();
            transform.parent = _target.transform;
            if (_warpedPos)
            {
                _target.controller.enabled = true;
                _target.transform.position = Vector3.MoveTowards(_target.transform.position, _warpedPos.position, targetTravelSpeed * Time.deltaTime);
                Vector3 targetPos = _target.transform.position;
                targetPos.z = 0;
                _target.transform.position = targetPos;
            }
            else if (playerTeleported)
            {
                _target.transform.position = Vector3.MoveTowards(_target.transform.position, playerSpawnPos, targetTravelSpeed * Time.deltaTime);
                if ((playerSpawnPos - _target.transform.position).magnitude <= 1)
                {
                    playerTeleported = false;
                    _target.transform.position = playerEndPos;
                }
            }
            else
            {
                _target.controller.enabled = false;
                _target.transform.position = Vector3.MoveTowards(_target.transform.position, _playerPos, targetTravelSpeed * Time.deltaTime);
                Vector3 targetPos = _target.transform.position;
                targetPos.z = 0;
                _target.transform.position = targetPos;
                if ((_playerPos - _target.transform.position).magnitude <= 1)
                    ReachedTarget(_target);
            }

        }
    }

    void PlayerTeleported(Vector3 spawn, Vector3 end)
    {
        if(fired && !playerTeleported)
        {
            playerTeleported = true;
            playerSpawnPos = spawn;
            playerEndPos = end;
        }
    }

    #region Fire Hook
    public void Fire(Vector3 dir)
    {
        canEnterTeleport = true;
        fired = true;
        transform.localPosition = spawnPoint.transform.localPosition;
        _startPosition = endPoint.localPosition;
        _playerPos = spawnPoint.transform.position;
        _playerPos.z = 0;
        //_playerPos = endPoint.position;
        _direction = ((_playerPos + dir) - _playerPos).normalized;
        transform.up = -_direction;
        _currentTime = 0;
        OnFireHook();
    }

    public void HookTarget(PlayerController target)
    {
        if (target.isDead)
        {
            ReturnHook();
            _target = null;
            return;
        }
        hooked = true;
        if (target.canMove)
            target.myAnim.Play("GetHit");
        target.canMove = false;
        _currentTime = 0;
        target.transform.position = transform.position;
        target.controller.enabled = false;
        OnHookTarget(target);
    }

    public void ReturnHook()
    {
        fired = false;
        hooked = false;
        hookGrabbed = false;
        transform.parent = null;
        _myPlayer.controller.enabled = true;
        if (_warpedPos)
            transform.position = Vector3.MoveTowards(transform.position, _warpedPos.position, speed * Time.deltaTime);
        else if (playerTeleported)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerSpawnPos, speed * Time.deltaTime);
            if ((playerSpawnPos - transform.position).magnitude <= 1)
            {
                playerTeleported = false;
                transform.position = playerEndPos;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnPoint.position, speed * Time.deltaTime);
            if ((spawnPoint.position - transform.position).magnitude <= 1)
            {
                transform.parent = _myPlayer.transform;
                transform.localPosition = _startPosition;
                returnFail = false;
                _warpedPos = null;
                OnReturnedEnd();
                OnFailedFire();
                return;
            }
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
        OnReturnedEnd();
        OnReachedTarget(target);
        _target = null;
        _warpedPos = null;
        hooked = false;
        fired = false;
        playerTeleported = false;
        transform.parent = _myPlayer.transform;
        transform.localPosition = _startPosition;
        target.transform.position = endPoint.position;
        target.myAnim.Play("Stunned");
        target.controller.enabled = true;
    }
    #endregion

    #region Counter
    public void SetHookGrabbed(PlayerController p)
    {
        hookGrabbed = true;
        _playerGrabbedHook = p;
        p.GetComponent<PlayerContrains>().OnTeleportPlayer += (x, y) => DisableWarpedZone();
        _psParry.Play();
    }

    void CounterHook()
    {
        if (!_playerGrabbedHook)
        {
            _myPlayer.controller.enabled = true;
            ReturnHook();
            return;
        }
        transform.parent = _playerGrabbedHook.transform;
        if (_warpedPos)
        {
            _myPlayer.controller.enabled = true;
            _myPlayer.transform.position = Vector3.MoveTowards(_myPlayer.transform.position, _whereIWarped.position, targetTravelSpeed * Time.deltaTime);
        }
        else if(playerTeleported)
        {
            _myPlayer.transform.position = Vector3.MoveTowards(_myPlayer.transform.position, playerSpawnPos, targetTravelSpeed * Time.deltaTime);
            if ((playerSpawnPos - _myPlayer.transform.position).magnitude <= 1)
            {
                playerTeleported = false;
                _myPlayer.transform.position = playerEndPos;
            }
        }
        else
        {
            //Cuidado porque se puede seguir moviendo el player que lo agarro y llevandoselo eternamente
            _myPlayer.controller.enabled = false;
            _myPlayer.transform.position = Vector3.MoveTowards(_myPlayer.transform.position, _playerGrabbedHook.transform.position, targetTravelSpeed * Time.deltaTime);
            if ((_playerGrabbedHook.transform.position - _myPlayer.transform.position).magnitude <= 1)
            {
                hookGrabbed = false;
                ReachedTarget(_myPlayer);
            }
        }
    }

    void DisableWarpedZone()
    {
        _whereIWarped = null;
    }
    #endregion

    PlayerController FindMyPlayer(Transform trans)
    {
        if (trans.GetComponent<PlayerController>())
            return trans.GetComponent<PlayerController>();
        return FindMyPlayer(trans.parent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DoorWarp") && canEnterTeleport)
        {
            WarpController door = other.gameObject.GetComponent<WarpController>();
            if (!_warpedPos)
            {
                door.WarpHook(transform);
                _warpedPos = door.parentWarp.zoneToReturnHook;
                _warpedPos.position = new Vector3(_warpedPos.position.x, _warpedPos.position.y, 0);
                _whereIWarped = door.zoneToRespawn;
                if (!hooked && !returnFail)
                {
                    teleportedBack = false;
                    OnTeleport(door.zoneToTeleportHook.position, door.parentWarp.zoneToTeleportHook.position);
                }
            }
            else
            {
                if (hooked || returnFail)
                {
                    if (_target)
                        door.WarpHook(_target.transform);
                    door.WarpHook(transform);
                    canEnterTeleport = false;
                    teleportedBack = true;
                }
                _warpedPos = null;
            }

        }
    }
}
