using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Pirate : PlayerController
{
    public List<Transform> hookPointsPositions;
    public float hookCooldown;
    public float movementHookCooldown;

    public override void Start()
    {
        base.Start();
        hookPointsPositions = GameObject.FindObjectsOfType<HookPoints>().Select(x => x.GetComponent<Transform>()).ToList();
        SetHabilities();
    }

    public override void Update()
    {
        base.Update();

        var allPointsVisible = GameObject.FindObjectsOfType<Pirate>().Aggregate(new FList<Transform>(), (x, y) =>
        {
            var points = Physics.OverlapSphere(y.transform.position, 10f).Where(h => h.GetComponent<HookPoints>()).Select(h => h.GetComponent<Transform>());
            if (points.Any())
                return x + points;
            return x;
        });

        hookChosenPosition = ClosesToDirection(allPointsVisible.Where(x => x.GetComponent<HookPoints>().isAvailable).Where(x => Vector3.Distance(x.position, transform.position) <= 10));

        if (hookChosenPosition)
            hookChosenPosition.GetComponent<HookPoints>().isAvailable = true;

        foreach (var hookPoint in hookPointsPositions)
        {
            if (allPointsVisible.Contains(hookPoint))
                hookPoint.GetComponent<HookPoints>().isAvailable = true;
            else
                hookPoint.GetComponent<HookPoints>().isAvailable = false;
        }
    }

    void PirateHability()
    {
        hability["HookHability"].Hability();
    }

    void MovementHability()
    {
        if (hookChosenPosition)
        {
            hability["MovementHook"].Hability();
        }
    }

    Transform ClosesToDirection(IEnumerable<Transform> hookPoints)
    {
        if (hookPoints.Count() == 0)
            return null;
        float x = GetComponent<PlayerInput>().MainHorizontal();
        float y = GetComponent<PlayerInput>().MainVertical();
        if (x + y == 0)
            x = Mathf.Sign(transform.localScale.z);

        Vector3 direction = ((transform.position + new Vector3(x, y, 0)) - transform.position).normalized;
        Vector3 startingPoint = transform.position;

        List<Tuple<Transform, float>> listTuple = new List<Tuple<Transform, float>>();
        float minDistance = 10000;

        foreach (var point in hookPoints)
        {
            Ray ray = new Ray(startingPoint, direction);
            float distance = Vector3.Cross(ray.direction, point.position - ray.origin).magnitude;
            if (distance < minDistance)
                minDistance = distance;
            listTuple.Add(Tuple.Create(point, distance));
        }
        return listTuple.Where(h => h.Item2 == minDistance).Select(h => h.Item1).First();
    }

    void SetHabilities()
    {
        hability.Add(typeof(HookHability).ToString(), new HookHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), transform.ChildrenWithComponent<Hook>().First(), hookCooldown));
        hability.Add(typeof(MovementHook).ToString(), new MovementHook(this, transform.ChildrenWithComponent<Hook>().First(), hookChosenPosition, movementHookCooldown));
        myHability = PirateHability;
        movementHability = MovementHability;
    }
}
