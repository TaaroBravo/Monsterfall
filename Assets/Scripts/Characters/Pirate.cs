using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        var points = Physics.OverlapSphere(transform.position, 10f).Where(x => x.GetComponent<HookPoints>()).Select(x => x.GetComponent<HookPoints>());
        hookChosenPosition = ClosesToDirection(points /*,new Vector3(GetComponent<PlayerInput>().MainHorizontal(), GetComponent<PlayerInput>().MainVertical(), 0)*/);
        if (hookChosenPosition)
            hookChosenPosition.gameObject.SetActive(true);

        foreach (var hookPoint in hookPointsPositions)
        {
            if((hookPoint.transform.position - transform.position).magnitude <= 10)
                hookPoint.gameObject.SetActive(true);
            else
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
        {
            hability["MovementHook"].Hability();
        }
    }

    Transform ClosesToDirection(IEnumerable<HookPoints> hookPoints/*, Vector3 input*/)
    {
        //Vector3 dirPoint = ((transform.position + input) - transform.position).normalized * 10;
        float distance = 10000;
        if (hookPoints.Count() == 0)
            return null;
        foreach (var point in hookPoints)
        {
            var tempDist = (point.transform.position - transform.position).magnitude;
            if (tempDist < distance)
                distance = tempDist;
        }
        return hookPoints.Where(x => (x.transform.position - transform.position).magnitude == distance).Select(x => x.GetComponent<Transform>()).First();
    }

    void SetHabilities()
    {
        hability.Add(typeof(HookHability).ToString(), new HookHability(this, transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), transform.ChildrenWithComponent<Hook>().First(), hookCooldown));
        hability.Add(typeof(MovementHook).ToString(), new MovementHook(this, transform.ChildrenWithComponent<Hook>().First(), hookChosenPosition, movementHookCooldown));
        myHability = PirateHability;
        movementHability = MovementHability;
    }
}
