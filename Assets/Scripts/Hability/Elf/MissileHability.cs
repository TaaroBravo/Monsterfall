using System.Collections;
using System.Collections.Generic;
using FrameworkGoat.ObjectPool;
using UnityEngine;
using System.Linq;
using System;

public class MissileHability : IHability
{
    Elf _elfPlayer;
    float _power;

    Transform[] _randomPositions;
    Missile _missilePrefab;

    float currentTimer;
    float maxTimer;
    bool activeTimer;

    public MissileHability(PlayerController p, Missile missilePrefab, float power, float _timerCoolDown = 0)
    {
        player = p;
        _elfPlayer = (Elf)p;
        _power = power;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _randomPositions = _elfPlayer.randomPositions;
        _missilePrefab = missilePrefab;
        maxTimer = 3f;
        ObjectPoolManager.Instance.AddObjectPool<Missile>(InstantiateBullet, Initializate, Finalizate, 50, false);
    }

    public override void Update()
    {
        base.Update();
        if (currentTimer < maxTimer)
            currentTimer += Time.deltaTime;
        else if (activeTimer)
        {
            for (int i = 0; i < 6; i++)
            {
                #region Shoots
                if (_elfPlayer.targets.Count == 1)
                {
                    if (i < 2)
                    {
                        Transform objetive = _elfPlayer.targets[0].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x, objetive.position.y + 20f, objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[0]);
                    }
                    else
                    {
                        Transform objetive = _randomPositions[CalculateCoef()]; //Random cerca del objetivo con rulete
                        Vector3 spawnPoint = new Vector3(objetive.position.x, objetive.position.y + 20f, objetive.position.z);
                        Shoot(spawnPoint, objetive);
                    }
                }
                else if (_elfPlayer.targets.Count == 2)
                {
                    if (i < 2)
                    {
                        Transform objetive = _elfPlayer.targets[0].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x, objetive.position.y + 20f, objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[0]);
                    }
                    else if (i < 4)
                    {
                        Transform objetive = _elfPlayer.targets[1].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x, objetive.position.y + 20f, objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[1]);
                    }
                    else
                    {
                        Transform objetive = _randomPositions[CalculateCoef()];  //Random cerca del objetivo con rulete
                        Vector3 spawnPoint = new Vector3(objetive.position.x, objetive.position.y + 20f, objetive.position.z);
                        Shoot(spawnPoint, objetive);
                    }
                }
                else if (_elfPlayer.targets.Count == 3)
                {
                    if (i < 2)
                    {
                        Transform objetive = _elfPlayer.targets[0].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x, objetive.position.y + 20f, objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[0]);
                    }
                    else if (i < 4)
                    {
                        Transform objetive = _elfPlayer.targets[1].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x, objetive.position.y + 20f, objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[1]);
                    }
                    else
                    {
                        Transform objetive = _elfPlayer.targets[2].transform;
                        Vector3 spawnPoint = new Vector3(objetive.position.x, objetive.position.y + 20f, objetive.position.z);
                        Shoot(spawnPoint, objetive, _elfPlayer.targets[2]);
                    }
                }
                else
                {
                    Transform objetive = _randomPositions[CalculateCoef()];
                    Vector3 spawnPoint = new Vector3(objetive.position.x, objetive.position.y + 20f, objetive.position.z);
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
        if (timerCoolDown < 0)
        {
            player.usingHability = true;
            FeedbackPlay();
            Shoot(player.transform.position, Vector3.up);
            timerCoolDown = coolDown;
            activeTimer = true;
            player.lifeHUD.ActivateSkillCD();
        }
    }

    void Shoot(Vector3 spawnPoint, Vector3 dir)
    {
        Missile missile = ObjectPoolManager.Instance.GetObject<Missile>();
        missile.transform.position = spawnPoint;
        missile.SetDir(player, dir);
        missile.OnDestroyMissile += x => ReturnBullet(x);
    }

    void Shoot(Vector3 spawnPoint, Transform objetive, PlayerController target = null)
    {
        Missile missile = ObjectPoolManager.Instance.GetObject<Missile>();
        missile.transform.position = spawnPoint;
        missile.SetObjetive(player, objetive, target);
        missile.OnDestroyMissile += x => ReturnBullet(x);
        missile.OnHitPlayer += x => HitPlayer(x);
    }

    void HitPlayer(PlayerController p)
    {
        if (_elfPlayer.targets.Contains(p))
        {
            p.SetDamage(20);
            _elfPlayer.CleanTargets();
        }
        else
            p.SetDamage(5);
    }

    public override void Release()
    {

    }

    void ResetValues()
    {
        player.usingHability = false;
        currentTimer = 0;
    }

    void FeedbackPlay()
    {
        //Particulas de disparo .Play()
        //Animación de disparo
    }

    #region Pool
    void ReturnBullet(Missile m)
    {
        m.OnDestroyMissile -= x => ReturnBullet(x);
        m.OnHitPlayer -= x => HitPlayer(x);
        ObjectPoolManager.Instance.ReturnObject<Missile>(m);
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
            totalCoef.Add(100 - distance);
        }
        return RouletteWheelSelection(totalCoef);
    }

    int RouletteWheelSelection(List<float> values)
    {
        float sumatoria = 0;

        //calculo la sumatoria de todas los coeficientes iniciales:
        sumatoria = values.Sum();

        List<float> coefList = new List<float>();

        //calculo la lista de coeficiente calculados para el roullette:
        foreach (var item in values)
        {
            coefList.Add(item / sumatoria);
            Debug.Log("Percent: " + item / sumatoria);
        }

        //calculo valor random:
        System.Random rnd = new System.Random();
        int rndPercent = rnd.Next(100);
        float r = rndPercent / 100f;


        //corro el algoritmo de seleccion de ruleta
        float sumCoef = 0;
        for (int i = 0; i < values.Count; i++)
        {
            //sumo los deltas a la variable sumcoef para saber en que slot cayo el valor random:
            sumCoef += coefList[i];

            if (sumCoef > r)
                return i;
        }

        return -1;


    }
    #endregion
}
