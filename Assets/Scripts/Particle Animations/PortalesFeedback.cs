using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Experimental.VFX;

public class PortalesFeedback : MonoBehaviour
{
    #region Top Portal
    public VisualEffect Particulas_Portal_Top; // VFX ( visual effect )
    public Light Luz_Portal_Top; // Point Light
    public Material Material_Portal_Top;
    bool Activated_Top;
    bool GoingUp_Top;
    bool GoingDown_Top;
    float timer_top;
    #endregion

    #region Top Portal
    public VisualEffect Particulas_Portal_Bot; // VFX ( visual effect )
    public Light Luz_Portal_Bot; // Point Light
    public Material Material_Portal_Bot;
    bool Activated_Bot;
    bool GoingUp_Bot;
    bool GoingDown_Bot;
    float timer_Bot;
    #endregion

    #region Top Portal
    public VisualEffect Particulas_Portal_Izq; // VFX ( visual effect )
    public Light Luz_Portal_Izq; // Point Light
    public Material Material_Portal_Izq;
    bool Activated_Izq;
    bool GoingUp_Izq;
    bool GoingDown_Izq;
    float timer_Izq;
    #endregion

    #region Top Portal
    public VisualEffect Particulas_Portal_Der; // VFX ( visual effect )
    public Light Luz_Portal_Der; // Point Light
    public Material Material_Portal_Der;
    bool Activated_Der;
    bool GoingUp_Der;
    bool GoingDown_Der;
    float timer_Der;
    #endregion

    public WarpController leftWarp;
    public WarpController rightWarp;

    public WarpController upWarp;
    public WarpController downWarp;

    void Start()
    {
        leftWarp.OnTeleportPlayer += () => ActivateRightPortal();
        rightWarp.OnTeleportPlayer += () => ActivateLeftPortal();
        upWarp.OnTeleportPlayer += () => ActivateBotPortal();
        downWarp.OnTeleportPlayer += () => ActivateTopPortal();
        Particulas_Portal_Top.SendEvent("Parar");
        Particulas_Portal_Bot.SendEvent("Parar");
        Particulas_Portal_Izq.SendEvent("Parar");
        Particulas_Portal_Der.SendEvent("Parar");
    }

    void Update()
    {
        #region Activacion Portal Top
        if (Activated_Top)
        {
            timer_top += Time.deltaTime;
            if (GoingUp_Top)
            {
                Material_Portal_Top.SetFloat("Vector1_EAB94DAD", -1f); // importante
                Luz_Portal_Top.range = timer_top * 200; // importante

                if (timer_top > 0.1)
                {
                    GoingUp_Top = false;
                    GoingDown_Top = true;
                    timer_top = 0;
                }
            }
            else if (GoingDown_Top)
            {
                Luz_Portal_Top.range = 23 - timer_top * 23; // importante
                Material_Portal_Top.SetFloat("Vector1_EAB94DAD", timer_top * 6); // importante
                if (timer_top > 1)
                {
                    GoingDown_Top = false;
                    timer_top = 0;
                    Luz_Portal_Top.range = 0;
                    Material_Portal_Top.SetFloat("Vector1_EAB94DAD", 5.08f);
                    Activated_Top = false;
                }
            }
        }
        #endregion
        #region Activacion Portal Bot
        if (Activated_Bot)
        {
            timer_Bot += Time.deltaTime;
            if (GoingUp_Bot)
            {
                Material_Portal_Bot.SetFloat("Vector1_EAB94DAD", -1f); // importante
                Luz_Portal_Bot.range = timer_Bot * 200; // importante

                if (timer_Bot > 0.1)
                {
                    GoingUp_Bot = false;
                    GoingDown_Bot = true;
                    timer_Bot = 0;
                }
            }
            else if (GoingDown_Bot)
            {
                Luz_Portal_Bot.range = 23 - timer_Bot * 23; // importante
                Material_Portal_Bot.SetFloat("Vector1_EAB94DAD", timer_Bot * 6); // importante
                if (timer_Bot > 1)
                {
                    GoingDown_Bot = false;
                    timer_Bot = 0;
                    Luz_Portal_Bot.range = 0;
                    Material_Portal_Bot.SetFloat("Vector1_EAB94DAD", 5.08f);
                    Activated_Bot = false;
                }
            }
        }
        #endregion
        #region Activacion Portal Izq
        if (Activated_Izq)
        {
            timer_Izq += Time.deltaTime;
            if (GoingUp_Izq)
            {
                Material_Portal_Izq.SetFloat("Vector1_EAB94DAD", -1f); // importante
                Luz_Portal_Izq.range = timer_Izq * 200; // importante

                if (timer_Izq > 0.1)
                {
                    GoingUp_Izq = false;
                    GoingDown_Izq = true;
                    timer_Izq = 0;
                }
            }
            else if (GoingDown_Izq)
            {
                Luz_Portal_Izq.range = 23 - timer_Izq * 23; // importante
                Material_Portal_Izq.SetFloat("Vector1_EAB94DAD", timer_Izq * 6); // importante
                if (timer_Izq > 1)
                {
                    GoingDown_Izq = false;
                    timer_Izq = 0;
                    Luz_Portal_Izq.range = 0;
                    Material_Portal_Izq.SetFloat("Vector1_EAB94DAD", 5.08f);
                    Activated_Izq = false;
                }
            }
        }
        #endregion
        #region Activacion Portal Der
        if (Activated_Der)
        {
            timer_Der += Time.deltaTime;
            if (GoingUp_Der)
            {
                Material_Portal_Der.SetFloat("Vector1_EAB94DAD", -1f); // importante
                Luz_Portal_Der.range = timer_Der * 200; // importante

                if (timer_Der > 0.1)
                {
                    GoingUp_Der = false;
                    GoingDown_Der = true;
                    timer_Der = 0;
                }
            }
            else if (GoingDown_Der)
            {
                Luz_Portal_Der.range = 23 - timer_Der * 23; // importante
                Material_Portal_Der.SetFloat("Vector1_EAB94DAD", timer_Der * 6); // importante
                if (timer_Der > 1)
                {
                    GoingDown_Der = false;
                    timer_Der = 0;
                    Luz_Portal_Der.range = 0;
                    Material_Portal_Der.SetFloat("Vector1_EAB94DAD", 5.08f);
                    Activated_Der = false;
                }
            }
        }
        #endregion
    }

    public void ActivateRightPortal()
    {
        Activated_Der = true;
        GoingUp_Der = true;
        Particulas_Portal_Der.SendEvent("Reproducir");
    }
    public void ActivateLeftPortal()
    {
        Activated_Izq = true;
        GoingUp_Izq = true;
        Particulas_Portal_Izq.SendEvent("Reproducir");
    }
    public void ActivateTopPortal()
    {
        Activated_Top = true;
        GoingUp_Top = true;
        Particulas_Portal_Top.SendEvent("Reproducir");
    }
    public void ActivateBotPortal()
    {
        Activated_Bot = true;
        GoingUp_Bot = true;
        Particulas_Portal_Bot.SendEvent("Reproducir");
    }
}
