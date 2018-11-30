using System.Collections;
using System.Collections.Generic;
using FrameworkGoat.ObjectPool;
using UnityEngine;
using System.Linq;
using System;

public class MissileHability : IHability
{
    Elf _elfPlayer;

    Transform[] _randomPositions;
    Missile _missilePrefab;

    //float currentTimer;
    //float maxTimer;
    bool activeTimer;
    bool usingHability;

    Missile _shootMissile;
    bool inScreen;

    public MissileHability(PlayerController p, Missile missilePrefab, float _timerCoolDown = 0)
    {
        player = p;
        _elfPlayer = (Elf)p;
        timerCoolDown = 2;
        coolDown = _timerCoolDown;
        _randomPositions = _elfPlayer.randomPositions;
        _missilePrefab = missilePrefab;
        //maxTimer = 3f;
        //ObjectPoolManager.Instance.AddObjectPool<Missile>(InstantiateBullet, Initializate, Finalizate, 50, false);
    }

    public override void Update()
    {
        base.Update();
        //if (currentTimer < maxTimer && usingHability)
        //    currentTimer += Time.deltaTime;
        if (activeTimer && !inScreen)
        {
            for (int i = 0; i < 6; i++)
            {
                #region Shoots
                if (_elfPlayer.targets.Count == 1)
                {
                    if (i < 2)
                    {
                        Transform objetive = _elfPlayer.targets[0].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x + UnityEngine.Random.Range(0, 3f), 70 + UnityEngine.Random.Range(0, 2f), objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[0]);
                    }
                    else
                    {
                        Transform objetive = _randomPositions[CalculateCoef()]; //Random cerca del objetivo con rulete
                        Vector3 spawnPoint = new Vector3(objetive.position.x + UnityEngine.Random.Range(0, 3f), 70 + UnityEngine.Random.Range(0, 2f), objetive.position.z);
                        Shoot(spawnPoint, objetive);
                    }
                }
                else if (_elfPlayer.targets.Count == 2)
                {
                    if (i < 2)
                    {
                        Transform objetive = _elfPlayer.targets[0].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x + UnityEngine.Random.Range(0, 3f), 70 + UnityEngine.Random.Range(0, 2f), objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[0]);
                    }
                    else if (i < 4)
                    {
                        Transform objetive = _elfPlayer.targets[1].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x + UnityEngine.Random.Range(0, 3f), 70 + UnityEngine.Random.Range(0, 2f), objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[1]);
                    }
                    else
                    {
                        Transform objetive = _randomPositions[CalculateCoef()];  //Random cerca del objetivo con rulete
                        Vector3 spawnPoint = new Vector3(objetive.position.x + UnityEngine.Random.Range(0, 3f), 70 + UnityEngine.Random.Range(0, 2f), objetive.position.z);
                        Shoot(spawnPoint, objetive);
                    }
                }
                else if (_elfPlayer.targets.Count == 3)
                {
                    if (i < 2)
                    {
                        Transform objetive = _elfPlayer.targets[0].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x + UnityEngine.Random.Range(0, 3f), 70 + UnityEngine.Random.Range(0, 2f), objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[0]);
                    }
                    else if (i < 4)
                    {
                        Transform objetive = _elfPlayer.targets[1].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x + UnityEngine.Random.Range(0, 3f), 70 + UnityEngine.Random.Range(0, 2f), objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[1]);
                    }
                    else
                    {
                        Transform objetive = _elfPlayer.targets[2].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x + UnityEngine.Random.Range(0, 3f), 70 + UnityEngine.Random.Range(0, 2f), objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[2]);
                    }
                }
                else
                {
                    Transform objetive = _randomPositions[CalculateCoef()];
                    Vector3 spawnPoint = new Vector3(objetive.position.x + UnityEngine.Random.Range(0, 3f), 70 + UnityEngine.Random.Range(0, 2f), objetive.position.z);
                    Shoot(spawnPoint, objetive);
                }
                #endregion
            }
            activeTimer = false;
            ResetValues();
        }
    }

    public override void Hability()
    {
        if (timerCoolDown < 0 && !usingHability)
        {
            usingHability = true;
            inScreen = true;
            player.usingHability = true;
            FeedbackPlay();
            player.StartCoroutine(FeedbackCantMove());
            Shoot(player.GetComponent<FeedbackElf>().spawnPointHability.position, Vector3.up);
            timerCoolDown = coolDown;
            activeTimer = true;
            player.lifeHUD.ActivateSkillCD();
        }
    }

    IEnumerator FeedbackCantMove()
    {
        while (true)
        {
            player.canMove = false;
            if (player.controller.isGrounded)
                yield return new WaitForSeconds(1f);
            else
                yield return new WaitUntil(() => player.controller.isGrounded);
            player.canMove = true;
            break;
        }
    }

    void FirstOutOfScreen()
    {
        inScreen = false;
    }

    void Shoot(Vector3 spawnPoint, Vector3 dir)
    {
        //Missile missile = ObjectPoolManager.Instance.GetObject<Missile>();
        Missile missile = GameObject.Instantiate(_missilePrefab);
        missile.transform.position = spawnPoint;
        missile.SetDir(player, dir);
        missile.ChangeColor(_elfPlayer.GetComponent<PlayerInput>().player_number);
        missile.OutOfScreen += () => FirstOutOfScreen();
        missile.OnDestroyMissile += x => ReturnBullet(x);
    }

    void Shoot(Vector3 spawnPoint, Transform objetive, PlayerController target = null)
    {
        //Missile missile = ObjectPoolManager.Instance.GetObject<Missile>();
        Missile missile = GameObject.Instantiate(_missilePrefab);
        missile.transform.position = spawnPoint;
        missile.SetObjetive(player, objetive, target);
        missile.ChangeColor(_elfPlayer.GetComponent<PlayerInput>().player_number);
        missile.OnDestroyMissile += x => ReturnBullet(x);
        missile.OnHitPlayer += x => HitPlayer(x);
    }

    void HitPlayer(PlayerController p)
    {
        if (p.stunnedByHit)
            return;
        if (_elfPlayer.targets.Contains(p))
        {
            p.SetLastOneWhoHittedMe(_elfPlayer);
            p.SetDamage(15);
            _elfPlayer.DisableEffect(p);
        }
        else
        {
            p.SetLastOneWhoHittedMe(_elfPlayer);
            p.SetDamage(8);
        }
    }

    public override void Release()
    {

    }

    void ResetValues()
    {
        usingHability = false;
        player.usingHability = false;
        //currentTimer = 0;
    }

    void FeedbackPlay()
    {
        player.GetComponent<FeedbackElf>().shootFire.Play();
        player.myAnim.Play("ShootHability");
    }

    #region Pool
    void ReturnBullet(Missile m)
    {
        //m.OnDestroyMissile -= x => ReturnBullet(x);
        //m.OnHitPlayer -= x => HitPlayer(x);
        //ObjectPoolManager.Instance.ReturnObject<Missile>(m);
        m.DestroyMissile();
    }

    Missile InstantiateBullet()
    {
        return GameObject.Instantiate(_missilePrefab);
    }

    void Initializate(Missile c)
    {
        c.gameObject.SetActive(true);
    }

    void Finalizate(Missile c)
    {
        c.gameObject.SetActive(false);
    }
    #endregion

    #region Roulette Selection

    int CalculateCoef()
    {
        List<float> totalCoef = new List<float>();
        foreach (var place in _randomPositions)
        {
            float distance = (GameManager.Instance.ClosesPlayer(place.position) - place.position).magnitude;
            //if (distance < 10)
            //    totalCoef.Add((100 * 3) - distance);
            //else
            totalCoef.Add(50 - distance);
        }
        return RouletteWheelSelection(totalCoef);
    }

    int RouletteWheelSelection(List<float> values)
    {
        float sum = 0;

        sum = values.Sum();

        List<float> coefList = new List<float>();

        foreach (var coef in values)
        {
            coefList.Add(coef / sum);
        }

        int random = UnityEngine.Random.Range(0, 10);
        float selectedNumber = random / 10f;

        float sumCoef = 0;
        for (int i = 0; i < values.Count; i++)
        {
            sumCoef += coefList[i];

            if (sumCoef > selectedNumber)
                return i;
        }

        return -1;


    }
    #endregion
}
