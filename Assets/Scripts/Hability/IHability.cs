using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class IHability {

    public PlayerController player;
    public CdHUDChecker cooldownHUD;
    public float timerCoolDown;
    public float coolDown;
    public bool isPressing;

    public abstract void Hability();
    public abstract void Release();

    public virtual void Update()
    {
        timerCoolDown -= Time.deltaTime;
    }
}
