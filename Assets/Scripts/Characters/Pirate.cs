using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pirate : PlayerController {

    public float hookCoolDown;

    public override void Start ()
    {
        base.Start();
        SetHabilities();
	}
	
	public override void Update ()
    {
        base.Update();
	}

    void PirateHability()
    {
        hability["HookHability"].Hability();
    }

    void SetHabilities()
    {
        myHability = PirateHability;
        hability.Add(typeof(HookHability).ToString(), new HookHability(this, transform.ChildrenWithComponent<Hook>().First(), hookCoolDown));
    }
}
