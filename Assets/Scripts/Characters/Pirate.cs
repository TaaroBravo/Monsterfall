using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pirate : PlayerController
{
    public Transform[] hookPointsPositions;
    public Transform hookChosenPosition;
    public float hookCooldown;
    public float movementHookCooldown;

    public override void Start()
    {
        base.Start();
        hookPointsPositions = GameObject.FindObjectsOfType<HookPoints>().Select(x => x.GetComponent<Transform>()).ToArray();
        SetHabilities();
    }

    public override void Update()
    {
        base.Update();
        hookChosenPosition = ClosesToDirection(Physics.OverlapSphere(transform.position, 10f).Where(x => x.GetComponent<HookPoints>()).Select(x => x.GetComponent<HookPoints>()), new Vector3(GetComponent<PlayerInput>().MainHorizontal(), GetComponent<PlayerInput>().MainVertical(), 0));
        if (hookChosenPosition)
            hookChosenPosition.gameObject.SetActive(true);
        foreach (var hookPoint in hookPointsPositions)
        {
            if(hookPoint != hookChosenPosition)
                hookPoint.gameObject.SetActive(false);
        }
    }

    void PirateHability()
    {
        hability["HookHability"].Hability();
    }

    void MovementHability()
    {
        if(hookChosenPosition)
            hability["MovementHook"].Hability();
    }

    Transform ClosesToDirection(IEnumerable<HookPoints> hookPoints, Vector3 input)
    {
        Vector3 dirPoint = ((transform.position + input) - transform.position).normalized * 10;
        float distance = 1000;
        if (hookPoints.Count() == 0)
            return null;
        foreach (var point in hookPoints)
        {
            var tempDist = (point.transform.position - dirPoint).magnitude;
            if (tempDist < distance)
                distance = tempDist;
        }
        return hookPoints.Where(x => (x.transform.position - dirPoint).magnitude == distance).Select(x => x.GetComponent<Transform>()).First();
    }

    void SetHabilities()
    {
        hability.Add(typeof(HookHability).ToString(), new HookHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), transform.ChildrenWithComponent<Hook>().First(), hookCooldown));
        hability.Add(typeof(MovementHook).ToString(), new MovementHook(this, transform.ChildrenWithComponent<Hook>().First(), hookChosenPosition, movementHookCooldown));
        myHability = PirateHability;
        movementHability = MovementHability;
    }
}
