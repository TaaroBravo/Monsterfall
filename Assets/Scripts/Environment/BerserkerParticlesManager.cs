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
    public float XAxis;
    public ParticleSystem.MainModule p2main;
    public ParticleSystem.ShapeModule p2shape;
    public bool iminverted;

    private void Start()
    {
        p2main = p2.main;
        p2shape = p2.shape;
        p3Em = p3.emission;
    }
    private void Update()
    {
        XAxis = GetComponent<PlayerInput>().MainHorizontal();
        if (!iminverted)
        {
            if (XAxis > 0)
            {
                p2main.startRotationY = 3.2f;
                p2shape.rotation = new Vector3(0, 0, 0);
            }
            else if (XAxis < 0)
            {
                p2main.startRotationY = 0f;
                p2shape.rotation = new Vector3(0, 180, 0);
            }
        }
        else
        {
            if (XAxis > 0)
            {
                p2main.startRotationY = 0f;
                p2shape.rotation = new Vector3(0, 180, 0);
            }
            else if (XAxis < 0)
            {
                p2main.startRotationY = 3.2f;
                p2shape.rotation = new Vector3(0, 0, 0);
            }
        }
        //DebugKeys();
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
