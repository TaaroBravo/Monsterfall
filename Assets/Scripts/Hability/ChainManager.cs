using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameworkGoat.ObjectPool;

public class ChainManager : MonoBehaviour
{
    public Hook hook;
    private Transform spawnPoint;
    public Chain chainPrefab;
    List<Chain> allChains = new List<Chain>();

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
        };

        hook.OnReturnedEnd += () => returned = true;
    }

    private void Update()
    {
        if (onActive)
        {
            var distance = (hook.transform.position - spawnPoint.position).magnitude;
            for (int i = 0; i < distance / distanceToSpawn; i++)
            {
                if (allChains.Count <= i + 1)
                    allChains.Add(ObjectPoolManager.Instance.GetObject<Chain>());

                allChains[i].transform.up = spawnPoint.position - hook.transform.position;
                allChains[i].transform.position = hook.transform.position + (spawnPoint.position - hook.transform.position).normalized * distanceToSpawn * i;
            }

            for (int i = allChains.Count - 1; i >= (int)(distance / distanceToSpawn); i--)
            {
                ObjectPoolManager.Instance.ReturnObject<Chain>(allChains[i]);
                allChains.RemoveAt(i);
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

        allChains.Clear();
        onActive = false;
        returned = false;
    }

    Chain InstantiateChain()
    {
        return Instantiate(chainPrefab);
    }
}
