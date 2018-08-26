using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PortalesFeedback : MonoBehaviour
{

    public List<GameObject> PortalDerParticles = new List<GameObject>();
    public List<GameObject> PortalIzqParticles = new List<GameObject>();
    public float DistanceToGo;
    public float TimeToGo;
    public float TimeToReturn;

    bool DerPortalGo;
    bool DerPortalReturn;
    bool IzqPortalGo;
    bool IzqPortalReturn;
    float DerPortalTimer;
    float IzqPortalTimer;
    List<Vector3> PortalDerStartPositions = new List<Vector3>();
    List<Vector3> PortalIzqStartPositions = new List<Vector3>();
    List<Vector3> PortalDerFinalPositions = new List<Vector3>();
    List<Vector3> PortalIzqFinalPositions = new List<Vector3>();

    public WarpController leftWarp;
    public WarpController rightWarp;

    void Start()
    {
        for (int i = 0; i < PortalDerParticles.Count; i++)
        {
            PortalDerStartPositions.Add(PortalDerParticles[i].transform.position);
            PortalDerFinalPositions.Add(new Vector3(
                PortalDerParticles[i].transform.position.x - DistanceToGo,
                PortalDerParticles[i].transform.position.y,
                PortalDerParticles[i].transform.position.z));
        }

        for (int i = 0; i < PortalIzqParticles.Count; i++)
        {
            PortalIzqStartPositions.Add(PortalIzqParticles[i].transform.position);
            PortalIzqFinalPositions.Add(new Vector3(
                PortalIzqParticles[i].transform.position.x + DistanceToGo,
                PortalIzqParticles[i].transform.position.y,
                PortalIzqParticles[i].transform.position.z));
        }

        leftWarp.OnTeleportPlayer += () => ActivateRightPortal();
        rightWarp.OnTeleportPlayer += () => ActivateLeftPortal();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) ActivateRightPortal();
        if (Input.GetKeyDown(KeyCode.P)) ActivateLeftPortal();
        DerPortalTimer += Time.deltaTime;
        IzqPortalTimer += Time.deltaTime;
        if (DerPortalGo)
        {
            for (int i = 0; i < PortalDerParticles.Count; i++)
                PortalDerParticles[i].transform.position = Vector3.Lerp(PortalDerStartPositions[i], PortalDerFinalPositions[i], DerPortalTimer / TimeToGo);
            if (DerPortalTimer >= TimeToGo)
            {
                DerPortalTimer = 0;
                DerPortalReturn = true;
                DerPortalGo = false;
            }
        }
        else if (DerPortalReturn)
        {
            for (int i = 0; i < PortalDerParticles.Count; i++)
                PortalDerParticles[i].transform.position = Vector3.Lerp(PortalDerFinalPositions[i], PortalDerStartPositions[i], DerPortalTimer / TimeToReturn);
            if (DerPortalTimer >= TimeToReturn)
            {
                DerPortalTimer = 0;
                DerPortalReturn = false;
            }
        }
        if (IzqPortalGo)
        {
            for (int i = 0; i < PortalIzqParticles.Count; i++)
                PortalIzqParticles[i].transform.position = Vector3.Lerp(PortalIzqStartPositions[i], PortalIzqFinalPositions[i], IzqPortalTimer / TimeToGo);
            if (IzqPortalTimer >= TimeToGo)
            {
                IzqPortalTimer = 0;
                IzqPortalReturn = true;
                IzqPortalGo = false;
            }
        }
        else if (IzqPortalReturn)
        {
            for (int i = 0; i < PortalIzqParticles.Count; i++)
                PortalIzqParticles[i].transform.position = Vector3.Lerp(PortalIzqFinalPositions[i], PortalIzqStartPositions[i], IzqPortalTimer / TimeToReturn);
            if (IzqPortalTimer >= TimeToReturn)
            {
                IzqPortalTimer = 0;
                IzqPortalReturn = false;
            }
        }
    }

    public void ActivateRightPortal()
    {
        if (!DerPortalGo && !DerPortalReturn)
        {
            DerPortalTimer = 0;
            DerPortalGo = true;
        }
    }
    public void ActivateLeftPortal()
    {
        if (!IzqPortalGo && !IzqPortalReturn)
        {
            IzqPortalTimer = 0;
            IzqPortalGo = true;
        }
    }
}
