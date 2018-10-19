using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Pirate : PlayerController
{
    public float hookCooldown;

    public ChainPart chainPrefab;

    public override void Start()
    {
        base.Start();
        SetHabilities();
        //StartCoroutine(TimerRecalculate());
    }

    public override void Update()
    {
        base.Update();
    }

    void PirateHability()
    {
        hability["HookHability"].Hability();
    }

    void MovementHability()
    {
        
    }


    void SetHabilities()
    {
        hability.Add(typeof(HookHability).ToString(), new HookHability(this, chainPrefab, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), transform.ChildrenWithComponent<Hook>().First(), 1.5f));
        myHability = PirateHability;
        movementHability = MovementHability;
    }

    #region Maybe
    //Transform ClosesToDirection(IEnumerable<Transform> hookPoints)
    //{
    //    if (hookPoints.Count() == 0)
    //        return null;
    //    float x = GetComponent<PlayerInput>().MainHorizontal();
    //    float y = GetComponent<PlayerInput>().MainVertical();
    //    if (x + y == 0)
    //        x = Mathf.Sign(transform.localScale.z);

    //    Vector3 direction = ((transform.position + new Vector3(x, y, 0)) - transform.position).normalized;
    //    Vector3 startingPoint = transform.position;

    //    List<Tuple<Transform, float>> listTuple = new List<Tuple<Transform, float>>();
    //    float minDistance = 10000;

    //    foreach (var point in hookPoints)
    //    {
    //        Ray ray = new Ray(startingPoint, direction);
    //        float distance = Vector3.Cross(ray.direction, point.position - ray.origin).magnitude;
    //        if (distance < minDistance)
    //            minDistance = distance;
    //        listTuple.Add(Tuple.Create(point, distance));
    //    }
    //    return listTuple.Where(h => h.Item2 == minDistance).Select(h => h.Item1).First();
    //}
    #endregion
}
