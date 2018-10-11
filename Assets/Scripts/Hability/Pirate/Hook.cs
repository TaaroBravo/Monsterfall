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

    public event Action<Vector3, Vector3> OnInitHook = delegate { };
    public event Action<Vector3, Vector3> OnEndHook = delegate { };
    public event Action OnTelepWithHookFired = delegate { };
    public event Action<Vector3, Vector3> OnTeleport = delegate { };

    public event Action OnReachedPoint = delegate { };

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
    private List<Tuple<Vector3, Vector3>> warpPositions = new List<Tuple<Vector3, Vector3>>();
    private Transform[] _warpedPos;
    private int indexWarped;
    private Transform _whereIWarped;

    private ParticleSystem _psParry;

    PlayerContrains contrains;
    Vector3 playerSpawnPos;
    Vector3 playerEndPos;
    bool playerTeleported;


    private Transform _hookPointTarget;
    public bool reachingPoint;
    private bool _hookReached;

    private void Awake()
    {
        contrains = transform.parent.GetComponent<PlayerContrains>();
        if(!contrains)
            contrains = transform.parent.parent.GetComponent<PlayerContrains>();
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
        if (reachingPoint)
        {
            //Jugador no se puede mover a menos que le peguen. Y si es así, se cancela el traveling.
            if (_hookReached)
            {
                _myPlayer.transform.position = Vector3.MoveTowards(_myPlayer.transform.position, _hookPointTarget.position, speed * Time.deltaTime);
                if ((_hookPointTarget.position - _myPlayer.transform.position).magnitude <= 1)
                {
                    PlayerReached();
                }
            }
            else
            {
                transform.parent = null;
                transform.position = Vector3.MoveTowards(transform.position, _hookPointTarget.position, speed * Time.deltaTime);
                if ((_hookPointTarget.position - transform.position).magnitude <= 1)
                {
                    transform.position = _hookPointTarget.position;
                    HookReached();
                }
            }

        }
        else
        {
            #region Hook For Enemies
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
                if (warpPositions.Count() > 0)
                {
                    _target.controller.enabled = true;
                    _target.transform.position = Vector3.MoveTowards(_target.transform.position, warpPositions[warpPositions.Count - 1].Item2, targetTravelSpeed * Time.deltaTime);
                    Vector3 targetPos = _target.transform.position;
                    targetPos.z = 0;
                    _target.transform.position = targetPos;
                    if ((_target.transform.position - warpPositions[warpPositions.Count - 1].Item2).magnitude <= 1f)
                    {
                        _target.transform.position = warpPositions[warpPositions.Count - 1].Item1;
                        warpPositions.Remove(warpPositions[warpPositions.Count - 1]);
                        OnEndHook(transform.position, transform.position);
                    }
                }
                else
                {
                    _target.controller.enabled = false;
                    _target.transform.position = Vector3.MoveTowards(_target.transform.position, _myPlayer.transform.position, targetTravelSpeed * Time.deltaTime);
                    Vector3 targetPos = _target.transform.position;
                    targetPos.z = 0;
                    _target.transform.position = targetPos;
                    if ((_myPlayer.transform.position - _target.transform.position).magnitude <= 1)
                        ReachedTarget(_target);
                }
            }
            #endregion
        }
    }

    void HookReached()
    {
        _hookReached = true;
        _direction = (_hookPointTarget.position - _playerPos).normalized;
    }

    void PlayerReached()
    {
        _hookReached = false;
        reachingPoint = false;
        transform.parent = _myPlayer.transform;
        _myPlayer.controller.enabled = true;
        ResidualVelocityOnReach();
        OnReachedPoint();
    }

    void ResidualVelocityOnReach()
    {
        _myPlayer.moveVector.x = Mathf.Sign(_direction.x) * 5f;
        _myPlayer.verticalVelocity = Mathf.Sign(_direction.y) * 15f;
        _myPlayer.moveVector.y = _myPlayer.verticalVelocity;
        _myPlayer.controller.Move(_myPlayer.moveVector * Time.deltaTime);
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
        _direction = ((_playerPos + dir) - _playerPos).normalized;
        transform.up = -_direction;
        _currentTime = 0;
        OnFireHook();
        OnInitHook(_myPlayer.transform.position, _myPlayer.transform.position);
    }

    public void Fire(Transform hookPoint)
    {
        reachingPoint = true;
        _hookPointTarget = hookPoint;
        transform.localPosition = spawnPoint.transform.localPosition;
        transform.parent = null;
        _playerPos = spawnPoint.transform.position;
        _direction = (_hookPointTarget.position - _playerPos).normalized;
        transform.up = -_direction;
        OnFireHook();
        OnInitHook(_myPlayer.transform.position, _myPlayer.transform.position);
    }

    public void HookTarget(PlayerController target)
    {
        if (target.isDead)
        {
            ReturnHook();
            _target = null;
            return;
        }
        target.DisableAll();
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
        if (warpPositions.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, warpPositions[warpPositions.Count - 1].Item2, speed * Time.deltaTime);
            if ((transform.transform.position - warpPositions[warpPositions.Count - 1].Item2).magnitude <= 1f)
            {
                transform.transform.position = warpPositions[warpPositions.Count - 1].Item1;
                warpPositions.Remove(warpPositions[warpPositions.Count - 1]);
                OnEndHook(transform.position, transform.position);
                print(warpPositions.Count());
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
                //EventManager.Instance.FireEvent(ChainManager.EVENT_DESTROY_LAST_SECTION, new ChainEvent(transform.position, transform.position));
                OnEndHook(transform.position, transform.position);
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
        //EventManager.Instance.FireEvent(ChainManager.EVENT_DESTROY_LAST_SECTION, new ChainEvent(transform.position, transform.position));
        OnEndHook(transform.position, transform.position);
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
        if (warpPositions.Any())
        {
            _myPlayer.controller.enabled = true;
            _myPlayer.transform.position = Vector3.MoveTowards(_myPlayer.transform.position, warpPositions[warpPositions.Count - 1].Item2, targetTravelSpeed * Time.deltaTime);
            if ((_myPlayer.transform.position - warpPositions[warpPositions.Count - 1].Item2).magnitude <= 0.5f)
            {
                _myPlayer.transform.position = warpPositions[warpPositions.Count - 1].Item1;
                warpPositions.Remove(warpPositions[warpPositions.Count - 1]);
                OnEndHook(transform.position, transform.position);
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

    void PlayerTeleported(Vector3 from, Vector3 to)
    {
        if(gameObject.activeSelf)
        {
            if(warpPositions.Count > 0 && (fired || returnFail || hooked))
            {
                OnTelepWithHookFired();
                warpPositions.Clear();
                OnInitHook(_myPlayer.transform.position, _myPlayer.transform.position);
            }
            else
            {
                warpPositions.Add(Tuple.Create(to, from));
                OnTeleport(to, from);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DoorWarp"))
        {
            WarpController door = other.gameObject.GetComponent<WarpController>();
            door.WarpHook(transform);
            warpPositions.Add(Tuple.Create(door.zoneToTeleportHook.position, door.parentWarp.zoneToTeleportHook.position));
            OnTeleport(door.zoneToTeleportHook.position, door.parentWarp.zoneToTeleportHook.position);
        }
    }
}
