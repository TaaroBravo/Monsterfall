using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PortalesFeedback : MonoBehaviour
{

    public List<GameObject> PortalDerParticles = new List<GameObject>();
    public List<GameObject> PortalIzqParticles = new List<GameObject>();
    public List<GameObject> PortalTopParticles = new List<GameObject>();
    public List<GameObject> PortalBotParticles = new List<GameObject>();
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
    List<Vector3> PortalDerStartPositions = new List<Vector3>();
    List<Vector3> PortalIzqStartPositions = new List<Vector3>();
    List<Vector3> PortalDerFinalPositions = new List<Vector3>();
    List<Vector3> PortalIzqFinalPositions = new List<Vector3>();
    List<Vector3> PortalTopStartPositions = new List<Vector3>();
    List<Vector3> PortalBotStartPositions = new List<Vector3>();
    List<Vector3> PortalTopFinalPositions = new List<Vector3>();
    List<Vector3> PortalBotFinalPositions = new List<Vector3>();
    // Mira esas 25 listas papaaaaaaa, el cancer en codigo...

    public WarpController leftWarp;
    public WarpController rightWarp;

    public WarpController upWarp;
    public WarpController downWarp;

    void Start()
    {
        AssignValues(PortalDerStartPositions, PortalDerFinalPositions, PortalDerParticles);
        AssignValues(PortalIzqStartPositions, PortalIzqFinalPositions, PortalIzqParticles);
        AssignValues(PortalTopStartPositions, PortalTopFinalPositions, PortalTopParticles);
        AssignValues(PortalBotStartPositions, PortalBotFinalPositions, PortalBotParticles);
        #region BackUp de codigo por si explota AssignValues
        //for (int i = 0; i < PortalDerParticles.Count; i++)
        //{
        //    PortalDerStartPositions.Add(PortalDerParticles[i].transform.position);
        //    PortalDerFinalPositions.Add(new Vector3(
        //        PortalDerParticles[i].transform.position.x - DistanceToGo,
        //        PortalDerParticles[i].transform.position.y,
        //        PortalDerParticles[i].transform.position.z));
        //}
        #endregion
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
        //CheckForPortalActivation(DerPortalGo, DerPortalReturn, DerPortalTimer, PortalDerStartPositions, PortalDerFinalPositions, PortalDerParticles);
        //CheckForPortalActivation(IzqPortalGo, IzqPortalReturn, IzqPortalTimer, PortalIzqStartPositions, PortalIzqFinalPositions, PortalIzqParticles);
        //CheckForPortalActivation(TopPortalGo, TopPortalReturn, TopPortalTimer, PortalTopStartPositions, PortalTopFinalPositions, PortalTopParticles);
        //CheckForPortalActivation(BotPortalGo, BotPortalReturn, BotPortalTimer, PortalBotStartPositions, PortalBotFinalPositions, PortalBotParticles);
        #region BackUp de codigo por si explota CheckForPortalActivation
        if (DerPortalGo)
        {
            for (int i = 0; i < PortalDerParticles.Count; i++)
            {
                PortalDerParticles[i].transform.position = Vector3.Lerp(PortalDerStartPositions[i], PortalDerFinalPositions[i], DerPortalTimer / TimeToGo);
                var main = PortalDerParticles[i].GetComponent<ParticleSystem>().main;
                main.startColor = new Color(255, 234, 255);
            }

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
            {
                PortalDerParticles[i].transform.position = Vector3.Lerp(PortalDerFinalPositions[i], PortalDerStartPositions[i], DerPortalTimer / TimeToReturn);
                var main = PortalDerParticles[i].GetComponent<ParticleSystem>().main;
                Color lerpcolor = new Color(255 - DerPortalTimer * 510, 234, 255);
                main.startColor = lerpcolor;
            }
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
        if (TopPortalGo)
        {
            for (int i = 0; i < PortalTopParticles.Count; i++)
                PortalTopParticles[i].transform.position = Vector3.Lerp(PortalTopStartPositions[i], PortalTopFinalPositions[i], TopPortalTimer / TimeToGo);
            if (TopPortalTimer >= TimeToGo)
            {
                TopPortalTimer = 0;
                TopPortalReturn = true;
                TopPortalGo = false;
            }
        }
        else if (TopPortalReturn)
        {
            for (int i = 0; i < PortalTopParticles.Count; i++)
                PortalTopParticles[i].transform.position = Vector3.Lerp(PortalTopFinalPositions[i], PortalTopStartPositions[i], TopPortalTimer / TimeToReturn);
            if (TopPortalTimer >= TimeToReturn)
            {
                TopPortalTimer = 0;
                TopPortalReturn = false;
            }
        }
        if (BotPortalGo)
        {
            for (int i = 0; i < PortalBotParticles.Count; i++)
                PortalBotParticles[i].transform.position = Vector3.Lerp(PortalBotStartPositions[i], PortalBotFinalPositions[i], BotPortalTimer / TimeToGo);
            if (BotPortalTimer >= TimeToGo)
            {
                BotPortalTimer = 0;
                BotPortalReturn = true;
                BotPortalGo = false;
            }
        }
        else if (BotPortalReturn)
        {
            for (int i = 0; i < PortalBotParticles.Count; i++)
                PortalBotParticles[i].transform.position = Vector3.Lerp(PortalBotFinalPositions[i], PortalBotStartPositions[i], BotPortalTimer / TimeToReturn);
            if (BotPortalTimer >= TimeToReturn)
            {
                BotPortalTimer = 0;
                BotPortalReturn = false;
            }
        }
        #endregion
    }

    //void DebugKeys()
    //{
    //    if (Input.GetKeyDown(KeyCode.O)) ActivateRightPortal();
    //    if (Input.GetKeyDown(KeyCode.P)) ActivateLeftPortal();
    //    if (Input.GetKeyDown(KeyCode.L)) ActivateTopPortal();
    //    if (Input.GetKeyDown(KeyCode.K)) ActivateBotPortal();
    //}
    void AssignValues(List<Vector3> StartPos, List<Vector3> EndPos, List<GameObject> Particles)
    {
        for (int i = 0; i < Particles.Count; i++)
        {
            StartPos.Add(Particles[i].transform.position);
            EndPos.Add(new Vector3(
                Particles == PortalIzqParticles? Particles[i].transform.position.x + DistanceToGo :
                Particles == PortalDerParticles? Particles[i].transform.position.x - DistanceToGo :
                Particles[i].transform.position.x
                ,
                Particles == PortalTopParticles ? Particles[i].transform.position.y - 2.3f :
                Particles == PortalBotParticles ? Particles[i].transform.position.y + 6f :
                Particles[i].transform.position.y
                ,
                Particles[i].transform.position.z));
        }
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
