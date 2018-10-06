using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BerserkerParticlesManager : MonoBehaviour {

    public ParticleSystem p1;
    public ParticleSystem p2;
    public ParticleSystem p3;
    public ParticleSystem.EmissionModule p3Em;
    public DashFeedbackControl DashGordo;
    //public GameObject berserkerSkill;
    //public Transform berserkerFoot;

    //public GameObject berserkerDash;
    //public Transform berserkerBack;

    //GameObject currentDashPS;
    private void Start()
    {
        p3Em = p3.emission;
    }
    private void Update()
    {
        DebugKeys();
    }
    public void DisplayBerserkerSkill() { p1.Play(); } 
    public void DisplayBerserkerCharge() { p2.Play(); p3Em.rateOverDistance = 5; DashGordo.activate = true; } 
    public void StopCharge() { p3Em.rateOverDistance = 0; }
    public void DebugKeys()
    {
        //if (Input.GetKeyDown(KeyCode.Q)) DisplayBerserkerSkill();
        //if (Input.GetKeyDown(KeyCode.W)) DisplayBerserkerCharge();
    }
}
