using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PortalesFeedback : MonoBehaviour
{

    public GameObject portalTop;
    public GameObject portalBot;
    public GameObject portalIzq;
    public GameObject portalDer;
    public float DistanceToGo;
    public float TimeToGo;
    public float TimeToReturn;

    bool DerPortalGo;
    bool DerPortalReturn;
    bool IzqPortalGo;
    bool IzqPortalReturn;
    bool TopPortalGo;
    bool TopPortalReturn;
    bool BotPortalGo;
    bool BotPortalReturn;
    float DerPortalTimer;
    float IzqPortalTimer;
    float TopPortalTimer;
    float BotPortalTimer;
    public Vector3 portalTopSP;
    public Vector3 portalBotSP;
    public Vector3 portalDerSP;
    public Vector3 portalIzqSP;
    public Vector3 portalTopEP;
    public Vector3 portalBotEP;
    public Vector3 portalDerEP;
    public Vector3 portalIzqEP;

    public WarpController leftWarp;
    public WarpController rightWarp;

    public WarpController upWarp;
    public WarpController downWarp;

    void Start()
    {
        portalDerSP = portalDer.transform.position;
        portalIzqSP = portalIzq.transform.position;
        portalTopSP = portalTop.transform.position;
        portalBotSP = portalBot.transform.position;
        portalDerEP = new Vector3(portalDer.transform.position.x - DistanceToGo, portalDer.transform.position.y, portalDer.transform.position.z);
        portalIzqEP = new Vector3(portalIzq.transform.position.x + DistanceToGo, portalIzq.transform.position.y, portalIzq.transform.position.z);
        portalTopEP = new Vector3(portalTop.transform.position.x, portalTop.transform.position.y - 7f, portalTop.transform.position.z);
        portalBotEP = new Vector3(portalBot.transform.position.x, portalBot.transform.position.y + 6.5f, portalBot.transform.position.z);
        leftWarp.OnTeleportPlayer += () => ActivateRightPortal();
        rightWarp.OnTeleportPlayer += () => ActivateLeftPortal();
        upWarp.OnTeleportPlayer += () => ActivateBotPortal();
        downWarp.OnTeleportPlayer += () => ActivateTopPortal();
    }

    void Update()
    {
        DerPortalTimer += Time.deltaTime;
        IzqPortalTimer += Time.deltaTime;
        TopPortalTimer += Time.deltaTime;
        BotPortalTimer += Time.deltaTime;
        #region BackUp de codigo por si explota CheckForPortalActivation
        if (DerPortalGo)
        {
            portalDer.transform.position = Vector3.Lerp(portalDerSP, portalDerEP, DerPortalTimer / TimeToGo);
            if (DerPortalTimer >= TimeToGo)
            {
                DerPortalTimer = 0;
                DerPortalReturn = true;
                DerPortalGo = false;
            }
        }
        else if (DerPortalReturn)
        {
            portalDer.transform.position = Vector3.Lerp(portalDerEP, portalDerSP, DerPortalTimer / TimeToReturn);
            if (DerPortalTimer >= TimeToReturn)
            {
                DerPortalTimer = 0;
                DerPortalReturn = false;
            }
        }
        if (IzqPortalGo)
        {
            portalIzq.transform.position = Vector3.Lerp(portalIzqSP, portalIzqEP, IzqPortalTimer / TimeToGo);
            if (IzqPortalTimer >= TimeToGo)
            {
                IzqPortalTimer = 0;
                IzqPortalReturn = true;
                IzqPortalGo = false;
            }
        }
        else if (IzqPortalReturn)
        {
            portalIzq.transform.position = Vector3.Lerp(portalIzqEP, portalIzqSP, IzqPortalTimer / TimeToReturn);
            if (IzqPortalTimer >= TimeToReturn)
            {
                IzqPortalTimer = 0;
                IzqPortalReturn = false;
            }
        }
        if (TopPortalGo)
        {
            portalTop.transform.position = Vector3.Lerp(portalTopSP, portalTopEP, TopPortalTimer / (TimeToGo));
            if (TopPortalTimer >= TimeToGo)
            {
                TopPortalTimer = 0;
                TopPortalReturn = true;
                TopPortalGo = false;
            }
        }
        else if (TopPortalReturn)
        {
            portalTop.transform.position = Vector3.Lerp(portalTopEP, portalTopSP, TopPortalTimer / (TimeToReturn + 0.17f));
            if (TopPortalTimer >= TimeToReturn + 0.17f)
            {
                TopPortalTimer = 0;
                TopPortalReturn = false;
            }
        }
        if (BotPortalGo)
        {
            portalBot.transform.position = Vector3.Lerp(portalBotSP, portalBotEP, BotPortalTimer / (TimeToGo));
            if (BotPortalTimer >= TimeToGo)
            {
                BotPortalTimer = 0;
                BotPortalReturn = true;
                BotPortalGo = false;
            }
        }
        else if (BotPortalReturn)
        {
            portalBot.transform.position = Vector3.Lerp(portalBotEP, portalBotSP, BotPortalTimer / (TimeToReturn + 0.07f));
            if (BotPortalTimer >= TimeToReturn + 0.07f)
            {
                BotPortalTimer = 0;
                BotPortalReturn = false;
            }
        }
        #endregion
        DebugKeys();
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.O)) ActivateRightPortal();
        if (Input.GetKeyDown(KeyCode.P)) ActivateLeftPortal();
        if (Input.GetKeyDown(KeyCode.L)) ActivateTopPortal();
        if (Input.GetKeyDown(KeyCode.K)) ActivateBotPortal();
    }
    void AssignValues(Vector3 StartingPos, Vector3 EndingPos, GameObject ParticleGO)
    {
        StartingPos = ParticleGO.transform.position;
        EndingPos = new Vector3(
            ParticleGO == portalIzq ? ParticleGO.transform.position.x + DistanceToGo :
            ParticleGO == portalDer ? ParticleGO.transform.position.x - DistanceToGo :
            ParticleGO.transform.position.x
            ,
            ParticleGO == portalTop ? ParticleGO.transform.position.y - 3f :
            ParticleGO == portalBot ? ParticleGO.transform.position.y + 6f :
            ParticleGO.transform.position.y
            ,
            ParticleGO.transform.position.z);
    }
    void CheckForPortalActivation(bool go, bool ret, float timer, List<Vector3> StartPos, List<Vector3> EndPos, List<GameObject> Particles)
    {
        if (go)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].transform.position = Vector3.Lerp(StartPos[i], EndPos[i], timer / TimeToGo);
                var main = Particles[i].GetComponent<ParticleSystem>().main;
                main.startColor = new Color(timer * 50, 0, 0);
            }

            if (timer >= TimeToGo)
            {
                timer = 0;
                ret = true;
                go = false;
            }
        }
        else if (ret)
        {
            for (int i = 0; i < Particles.Count; i++)
                Particles[i].transform.position = Vector3.Lerp(EndPos[i], StartPos[i], timer / TimeToReturn);
            if (timer >= TimeToReturn)
            {
                timer = 0;
                ret = false;
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
    public void ActivateTopPortal()
    {
        if (!TopPortalGo && !TopPortalReturn)
        {
            TopPortalTimer = 0;
            TopPortalGo = true;
        }
    }
    public void ActivateBotPortal()
    {
        if (!BotPortalGo && !BotPortalReturn)
        {
            BotPortalTimer = 0;
            BotPortalGo = true;
        }
    }
}
