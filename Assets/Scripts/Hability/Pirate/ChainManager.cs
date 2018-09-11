using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkGoat.ObjectPool;
using System;

public class ChainManager : MonoBehaviour
{
    public Hook hook;
    private Transform spawnPoint;
    public Chain chainPrefab;
    List<Chain> allChains = new List<Chain>();
    List<Tuple<int, Vector3, Vector3>> positions = new List<Tuple<int, Vector3, Vector3>>();

    public float distanceToSpawn;

    bool onActive;
    bool returned;

    void Start()
    {
        ObjectPoolManager.Instance.AddObjectPool<Chain>(InstantiateChain, Initializate, Finalizate, 100, true);
        hook.OnFireHook += () =>
        {
            onActive = true;
            spawnPoint = hook.spawnPoint;
            positions.Add(Tuple.Create(0, hook.spawnPoint.position, hook.spawnPoint.position));
        };

        hook.OnReturnedEnd += () => returned = true;
        hook.OnTeleport += (x, y) => Teleported(x, y);
    }

    private void Update()
    {
        if (onActive)
        {
            var distance = 0f;

            if (hook.teleportedBack)
                distance = (hook.transform.position - spawnPoint.position).magnitude;
            else
                distance = (hook.transform.position - positions[1].Item3).magnitude + (positions[1].Item2 - spawnPoint.position).magnitude;

            var forLenght = (distance / distanceToSpawn) - 1;

            for (int i = 0; i < forLenght; i++)
            {
                if (allChains.Count <= i + 1)
                    allChains.Add(ObjectPoolManager.Instance.GetObject<Chain>());

                var targetPos = GetPosition(i);
                if (targetPos == 0)
                {
                    if (positions.Count > 1)
                    {
                        allChains[i].transform.up = spawnPoint.position - positions[1].Item2;
                        allChains[i].transform.position = spawnPoint.position - (spawnPoint.position - positions[1].Item2).normalized * distanceToSpawn * (i + 1);
                    }
                    else
                    {
                        allChains[i].transform.up = spawnPoint.position - hook.transform.position;
                        allChains[i].transform.position = hook.transform.position + (spawnPoint.position - hook.transform.position).normalized * distanceToSpawn * i;
                    }
                }
                else
                {
                    allChains[i].transform.up = positions[targetPos].Item3 - hook.transform.position;
                    allChains[i].transform.position = hook.transform.position + (positions[targetPos].Item3 - hook.transform.position).normalized
                        * distanceToSpawn * (i - positions[targetPos].Item1 - 2);
                }
            }

            for (int i = allChains.Count - 1; i > forLenght; i--)
            {
                ObjectPoolManager.Instance.ReturnObject<Chain>(allChains[i]);
                allChains.RemoveAt(i);

                if (i < positions[positions.Count - 1].Item1)
                    positions.RemoveAt(positions.Count - 1);
            }

            if (returned)
                EndChain();
        }
    }

    void Initializate(Chain c)
    {
        c.gameObject.SetActive(true);
    }

    void Finalizate(Chain c)
    {
        c.gameObject.SetActive(false);
    }

    void EndChain()
    {
        for (int i = allChains.Count - 1; i >= 0; i--)
            ObjectPoolManager.Instance.ReturnObject<Chain>(allChains[i]);

        positions.Clear();
        allChains.Clear();
        onActive = false;
        returned = false;
    }

    int GetPosition(int index)
    {
        var result = 0;
        if (positions.Count > 1 && index * distanceToSpawn > Vector3.Distance(spawnPoint.position, positions[1].Item2))
            result = 1;

        return result;
    }

    void Teleported(Vector3 spawn, Vector3 end)
    {
        positions.Add(Tuple.Create(allChains.Count - 1, spawn, end));
    }

    Chain InstantiateChain()
    {
        return Instantiate(chainPrefab);
    }
}
