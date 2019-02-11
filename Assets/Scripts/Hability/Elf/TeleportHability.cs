using System.Collections;
using System.Collections.Generic;
using FrameworkGoat.ObjectPool;
using UnityEngine;

public class TeleportHability : IHability
{

    Elf _elfPlayer;

    MissileTeleport _missilePrefab;
    MissileTeleport _currentMissile;

    float currentTimer;
    float maxTimer;
    bool activeTimer;
    bool missileMoving;

    bool usingHability;

    public TeleportHability(PlayerController p, MissileTeleport missilePrefab, float _timerCoolDown = 0)
    {
        player = p;
        _elfPlayer = (Elf)p;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _missilePrefab = missilePrefab;
        maxTimer = 1.7f;
        //ObjectPoolManager.Instance.AddObjectPool<MissileTeleport>(InstantiateBullet, Initializate, Finalizate, 6, false);
    }

    public override void Update()
    {
        base.Update();
        //if (activeTimer)
        //{
        //    maxTimer += Time.deltaTime;
        //    if (maxTimer > 3)
        //        maxTimer = 3;
        //}
        /*else */
        //if (missileMoving)
        //{
        //    if (currentTimer < maxTimer)
        //        currentTimer += Time.deltaTime;
        //    else
        //    {
        //        if (_currentMissile)
        //            ReturnBullet(_currentMissile);
        //        missileMoving = false;
        //        ResetValues();
        //    }
        //}
    }

    public override void Hability()
    {
        activeTimer = true;
        if (timerCoolDown < 0 && !_currentMissile)
        {
            player.usingHability = true; //Quizas remplazar esto por una de dash
            Vector3 dir = CalculateDirection();
            dir = FeedbackPlay(dir);
            Shoot(player.GetComponent<FeedbackElf>().spawnPointDash.position, dir);
            timerCoolDown = coolDown;
            activeTimer = false;
            missileMoving = true;
            _elfPlayer.canMove = false;
            _elfPlayer.moveVector.x = 0;
            player.StartCoroutine(TimeToTeleport());
            player.lifeHUD.ActivateDashCD();
        }
    }

    void Shoot(Vector3 spawnPoint, Vector3 dir)
    {
        //MissileTeleport missile = ObjectPoolManager.Instance.GetObject<MissileTeleport>();
        MissileTeleport missile = GameObject.Instantiate(_missilePrefab);
        missile.transform.position = spawnPoint;
        missile.SetDir(player, dir);
        missile.ChangeColor(_elfPlayer.GetComponent<PlayerInput>().player_number);
        missile.OnDestroyMissile += x => ReturnBullet(x);
        missile.OnHitPlayer += x => HitPlayer(x);
        _currentMissile = missile;
    }

    void HitPlayer(PlayerController p)
    {
        p.GetComponent<GeneralFeedback>().StartLightning(_elfPlayer.GetComponent<PlayerInput>().player_number);
        if (_elfPlayer.targets.Contains(p))
        {
            foreach (var target in _elfPlayer.targets)
            {
                if (target == p)
                {
                    p.SetLastOneWhoHittedMe(_elfPlayer);
                    p.SetDamage(15);
                }
            }
        }
        else
        {
            p.SetLastOneWhoHittedMe(_elfPlayer);
            p.SetDamage(10);
        }
    }

    void Teleport()
    {
        if (_currentMissile)
        {
            player.StartCoroutine(FeedbackTeleport());
            //player.transform.position = _currentMissile.transform.position - new Vector3(0, player.GetComponent<Collider>().bounds.extents.y, 0);
        }
    }

    IEnumerator TimeToTeleport()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.6f);
            ReturnBullet(_currentMissile);
            break;
        }
    }


    IEnumerator FeedbackTeleport()
    {
        while (true)
        {
            MissileTeleport m = _currentMissile;
            //player.GetComponent<FeedbackElf>().StartTeleportFeedback();
            var trail = m.GetComponent<ElfTeleportMissileFeedback>().Part3.transform;
            trail.SetParent(null);
            yield return new WaitForSeconds(0.01f);
            //player.GetComponent<FeedbackElf>().FinishTeleportFeedback();
            player.transform.position = m.transform.position - new Vector3(0, player.GetComponent<Collider>().bounds.extents.y, 0);
            m.DestroyMissile();
            yield return new WaitForSeconds(1f);
            player.GetComponent<FeedbackElf>().DestroyTrail(trail);
            break;
        }
    }

    public override void Release()
    {
        //if (timerCoolDown < 0 && !_currentMissile)
        //{
        //    player.usingHability = true; //Quizas remplazar esto por una de dash
        //    FeedbackPlay();
        //    Shoot(player.GetComponent<FeedbackElf>().spawnPointDash.position, CalculateDirection());
        //    timerCoolDown = coolDown;
        //    activeTimer = false;
        //    missileMoving = true;
        //    player.lifeHUD.ActivateDashCD();
        //}
    }

    void ResetValues()
    {
        _currentMissile = null;
        player.usingHability = false;
        maxTimer = 1;
        currentTimer = 0;
    }

    Vector3 FeedbackPlay(Vector3 dir)
    {
        player.GetComponent<FeedbackElf>().shootDashFire.Play();
        float dirX = 0;
        float dirY = 0;
        if (Mathf.Abs(dir.y) * Mathf.Sign(dir.y) == 1)
        {
            if (Mathf.Abs(dir.x) == 1)
            {
                dirX = Mathf.Sign(dir.x);
                dirY = 1;
                player.myAnim.Play("DashUpForward");
            }
            else
            {
                dirX = 0;
                dirY = 1;
                player.myAnim.Play("DashUp");
            }
        }
        else if (Mathf.Abs(dir.y) * Mathf.Sign(dir.y) == -1)
        {
            if (Mathf.Abs(dir.x) == 1)
            {
                dirX = Mathf.Sign(dir.x);
                dirY = -1;
                player.myAnim.Play("DashDownForward");
            }
            else
            {
                dirX = 0;
                dirY = -1;
                player.myAnim.Play("DashDown");
            }
        }
        else
        {
            dirY = 0;
            if (Mathf.Abs(dir.x) == 1)
                dirX = Mathf.Sign(dir.x);
            else
                dirX = 0;
            player.myAnim.Play("DashForward");
        }
        //if (dirX == 0)
        //    dirX = Mathf.Abs(player.transform.localScale.z) * Mathf.Sign(player.transform.localScale.z);
        return new Vector3(dirX, dirY);
    }

    #region Pool
    void ReturnBullet(MissileTeleport m)
    {
        _elfPlayer.canMove = true;
        Teleport();
        //m.OnDestroyMissile -= x => ReturnBullet(x);
        //m.OnHitPlayer -= x => HitPlayer(x);
        ResetValues();
        //m.DestroyMissile();
        //ObjectPoolManager.Instance.ReturnObject<MissileTeleport>(m);
    }

    MissileTeleport InstantiateBullet()
    {
        return GameObject.Instantiate(_missilePrefab);
    }

    void Initializate(MissileTeleport c)
    {
        c.gameObject.SetActive(true);
    }

    void Finalizate(MissileTeleport c)
    {
        c.gameObject.SetActive(false);
    }
    #endregion

    Vector3 CalculateDirection()
    {
        float x = player.GetComponent<PlayerInput>().MainHorizontal();
        float y = player.GetComponent<PlayerInput>().MainVertical();
        //-1;1
        //1;-1
        //int x = (int)(Mathf.Sign(x) * Mathf.Abs(x));
        //int y = (int)(Mathf.Sign(y) * Mathf.Abs(y));
        if (x + y == 0)
            x = (int)Mathf.Sign(player.transform.localScale.z);
        return new Vector3(x, y);

    }


}
