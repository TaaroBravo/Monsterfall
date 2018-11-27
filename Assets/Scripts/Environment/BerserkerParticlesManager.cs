using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BerserkerParticlesManager : MonoBehaviour
{
    /// feedback de golpe normal
    public ParticleSystem golpenormal;
    public ParticleSystem golpenormal1;
    public ParticleSystem golpenormal2;
    ParticleSystem.MainModule GP_main1;
    ParticleSystem.ShapeModule GP_shape1;
    ParticleSystem.MainModule GP_main2;
    ParticleSystem.ShapeModule GP_shape2;
    /// </>
    public ParticleSystem p1;
    public ParticleSystem p2;
    public ParticleSystem p3;
    ParticleSystem.EmissionModule p2Em;
    ParticleSystem.EmissionModule p3Em;
    //public DashFeedbackControl DashGordo;
    public float XAxis;
    ParticleSystem.MainModule p2main;
    ParticleSystem.ShapeModule p2shape;
    public bool iminverted;
    float dashCDtimer; // solucion temporal
    bool candash; // solucion temporal

    private void Start()
    {
        p2main = p2.main;
        p2shape = p2.shape;
        p2Em = p2.emission;
        p3Em = p3.emission;
        GP_main1 = golpenormal1.main;
        GP_shape1 = golpenormal1.shape;
        GP_main2 = golpenormal2.main;
        GP_shape2 = golpenormal2.shape;
    }
    private void Update()
    {
        if (!candash) dashCDtimer += Time.deltaTime; // solucion temporal
        if (dashCDtimer >= 3f)
        {
            candash = true; // solucion temporal
            dashCDtimer = 0;
        }
        if (dashCDtimer >= 1.2f) p2Em.rateOverDistance = 0f; // solucion temporal
        /// feedback de golpe normal
        if (!iminverted)
        {
            if (XAxis > 0) // dado vuelta
            {
                Debug.Log("caso 1");
                //GP_main1.startRotationY = 0f;
                //GP_main2.startRotationY = 0f;
                GP_shape1.rotation = new Vector3(0, -90, 0);
                GP_shape2.rotation = new Vector3(0, 90, 0);
            }
            else if (XAxis < 0) // frente
            {
                //GP_main1.startRotationY = 0f;
                //GP_main2.startRotationY = 0f;
                GP_shape1.rotation = new Vector3(0, -90, 0);
                GP_shape2.rotation = new Vector3(0, 90, 0);
            }
        }
        else
        {
            if (XAxis > 0) // frente
            {
                Debug.Log("caso 3");
                //GP_main1.startRotationY = 0f;
                //GP_main2.startRotationY = 0f;
                GP_shape1.rotation = new Vector3(0, 90, 0);
                GP_shape2.rotation = new Vector3(0, -90, 0);
            }
            else if (XAxis < 0) // dado vuelta
            {
                Debug.Log("caso 4");
                //GP_main1.startRotationY = 0f;
                //GP_main2.startRotationY = 0f;
                GP_shape1.rotation = new Vector3(0, 90, 0);
                GP_shape2.rotation = new Vector3(0, -90, 0);
            }
        }
        /// </>
        XAxis = GetComponent<PlayerInput>().MainHorizontal();
        if (!iminverted) // esto quizas lo tenga k cambiar
        {
            if (XAxis > 0)
            {
                p2main.startRotationY = 3.2f;
                p2shape.rotation = new Vector3(0, 180, 0);
            }
            else if (XAxis < 0)
            {
                p2main.startRotationY = 0f;
                p2shape.rotation = new Vector3(0, 0, 0);
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
    public void PlayAttackParticle() { golpenormal.Play(); }
    public void DisplayBerserkerSkill() { p1.Play(); }
    public void DisplayBerserkerCharge() // solucion temporal
    {
        p2.Play();
        //if (candash)
        //{
        //    Debug.Log("imactivating"); // esto se activa de 2 a 3 veces
        //    p2.Play();
        //    p2Em.rateOverDistance = 0.025f;
        //    //p3Em.rateOverDistance = 5;
        //    DashGordo.activate = true;
        //    DashGordo.timer = 0;
        //    DashGordo.sarasa.SetFloat("_Speed", 0);
        //    candash = false;
        //}
    }
    public void StopCharge() { p2.Stop(); }
    public void DebugKeys()
    {
        //if (Input.GetKeyDown(KeyCode.Q)) DisplayBerserkerSkill();
        //if (Input.GetKeyDown(KeyCode.W)) DisplayBerserkerCharge();
    }
}
