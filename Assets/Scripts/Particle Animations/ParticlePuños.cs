using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParticlePuños : MonoBehaviour
{

    PlayerController yo;
    public ParticleSystem puñoderecho;
    public ParticleSystem puñoizquierdo;
    [HideInInspector]
    public ParticleSystem.EmissionModule puñoder;
    [HideInInspector]
    public ParticleSystem.EmissionModule puñoizq;
    bool PrenderPuñoDerecho;
    bool PrenderPuñoIzquierdo;
    float TimerDeActivacion;

    // Use this for initialization
    void Start()
    {
        yo = GetComponent<PlayerController>();
        puñoder = puñoderecho.emission;
        puñoizq = puñoizquierdo.emission;
        ResetEmission();
    }
    void Update()
    {
        if (PrenderPuñoDerecho && puñoderecho) ParticulasPuñoDerecho();
        if (PrenderPuñoIzquierdo && puñoizquierdo) ParticulasPuñoIzquierdo();
    }
    public void PuñoAActivar(string golpe)
    {
        if (yo.myAnim.GetBool("Grounded"))
        {
            if (golpe == "arriba") PrenderPuñoDerecho = true;
            if (golpe == "abajo") PrenderPuñoIzquierdo = true;
            if (golpe == "recto") PrenderPuñoIzquierdo = true;
        }
        else
        {
            if (golpe == "arriba") PrenderPuñoDerecho = true;
            if (golpe == "abajo") PrenderPuñoDerecho = true;
            if (golpe == "recto") PrenderPuñoDerecho = true;
        }
    }
    void ParticulasPuñoDerecho()
    {
        TimerDeActivacion += Time.deltaTime;
        puñoder.rateOverDistance = 15;
        if (TimerDeActivacion > 0.2f)
        {
            ResetEmission();
            PrenderPuñoDerecho = false;
        }
    }
    void ParticulasPuñoIzquierdo()
    {
        TimerDeActivacion += Time.deltaTime;
        puñoizq.rateOverDistance = 15;
        if (TimerDeActivacion > 0.2f)
        {
            ResetEmission();
            PrenderPuñoIzquierdo = false;
        }
    }
    void ResetEmission()
    {
        TimerDeActivacion = 0;
        if (puñoderecho)
        {
            puñoder.rateOverDistance = 0;
            puñoizq.rateOverDistance = 0;
        }
    }
}
