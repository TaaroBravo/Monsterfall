﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GeneralFeedback : MonoBehaviour
{
    public ParticleSystem slashParticle;
    public ParticleSystem slashDown;
    public ParticleSystem slashUp;
    public GameObject iceblock;
    List<Color> PColors = new List<Color>();
    public ParticleSystem.MainModule MainElfMark;
    public ParticleSystem ElfMark;
    public ParticleSystem CristalBuff;
    public ParticleSystem CristalBuff2;
    public ParticleSystem.ShapeModule CristalBuffShape;
    public ParticleSystem.ShapeModule CristalBuffShape2;
    public ParticleSystem Lightning;
    public ParticleSystem Crosshair;
    ParticleSystem.MainModule mainCrosshair;
    public Material LightningMat;
    public Material mymaterial;
    float _dissolvetimer;
    bool _start_dissolve;
    float XAxis;
    public bool iminverted;
    public bool reposition;

    private void Start()
    {
        if (iceblock)
            iceblock.SetActive(false);
        CristalBuff = transform.ChildrenWithComponent<RayShield>().First().GetComponent<ParticleSystem>();
        //CristalBuffShape = CristalBuff.shape;
        //CristalBuffShape2 = CristalBuff2.shape;
        MainElfMark = ElfMark.main;
        mainCrosshair = Crosshair.main;
        Color outcolor;
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) PColors.Add(outcolor); // blue - 0
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) PColors.Add(outcolor); // red - 1
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) PColors.Add(outcolor); // green - 2
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) PColors.Add(outcolor); // yellow - 3
    }
    private void Update()
    {
        if (_start_dissolve) _dissolvetimer += Time.deltaTime;
        //XAxis = GetComponent<PlayerInput>().MainHorizontal();
        //CristalBuff.gameObject.transform.eulerAngles = new Vector3(270, 180, 0);
        CristalBuff.transform.position = transform.position;
        if (!iminverted) ElfMark.gameObject.transform.eulerAngles = new Vector3(270, 180, 0);
        else ElfMark.gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
    }
    public void StartElfMark(int ID)
    {
        MainElfMark.startColor = PColors[ID];
        ElfMark.Play();
        mainCrosshair.startColor = PColors[ID];
        mainCrosshair.simulationSpeed = 0.5f;
        MainElfMark.startLifetime = 3f;
        //mainCrosshair
        if (!Crosshair.isPlaying)
            Crosshair.Play();
    }
    public void FinishElfMark() { ElfMark.Stop(); }
    public void StartCristalBuff()
    {
        CristalBuff.Play();
        foreach (var item in CristalBuff.transform.ChildrenWithComponent<ParticleSystem>())
            item.Play();
    }
    public void FinishCristalBuff()
    {
        CristalBuff.Stop();
        foreach (var item in CristalBuff.transform.ChildrenWithComponent<ParticleSystem>())
            item.Stop();
    }
    public void StartLightning(int ID)
    {
        Lightning.Play();
        LightningMat.SetColor("_Tinte", PColors[ID]);
    }
    public void FinishLightning() { Lightning.Stop(); }
    public void StartCrosshair(int ID)
    {
        //Crosshair.gameObject.SetActive(true);
        mainCrosshair.startColor = PColors[ID];
        mainCrosshair.simulationSpeed = 2f;
        if (!Crosshair.isPlaying)
            Crosshair.Play();
    }
    public void FinishCrosshair()
    {
        //Crosshair.gameObject.SetActive(false);
        if (!Crosshair.isStopped)
            Crosshair.Stop();
    }
    public void StartFrozen()
    {
        iceblock.GetComponent<IceBlock>().CenterShakePosition = iceblock.transform.position;
        iceblock.SetActive(true);
    }
    public void FinishFrozen()
    {
        iceblock.SetActive(false);
    }
    public void StartIceblockShake()
    {
        iceblock.GetComponent<IceBlock>().Shaking = true;
    }
    public void FinishIceblockShake()
    {
        iceblock.GetComponent<IceBlock>().Shaking = false;
    }

    public void PlaySlash()
    {
        var slash = GameObject.Instantiate(slashParticle);
        slash.gameObject.SetActive(true);
        slash.gameObject.AddComponent<SelfDestruct>();
        slash.GetComponent<SelfDestruct>().selfdestruct_in = 1;
        Vector3 pos = slashParticle.transform.localScale;
        pos.x *= Mathf.Sign(transform.localScale.z);
        slash.gameObject.transform.localScale = pos;
        slash.transform.position = slashParticle.transform.position;
        slash.transform.rotation = slashParticle.transform.rotation;
        slash.Play();
    }

    public void PlaySlashDown()
    {
        var slash = GameObject.Instantiate(slashDown);
        slash.gameObject.SetActive(true);
        slash.gameObject.AddComponent<SelfDestruct>();
        slash.GetComponent<SelfDestruct>().selfdestruct_in = 1;
        Vector3 pos = slashDown.transform.localScale;
        pos.x *= Mathf.Sign(transform.localScale.z);
        slash.gameObject.transform.localScale = pos;
        slash.transform.rotation = slashDown.transform.rotation;
        slash.transform.position = slashDown.transform.position;
        slash.Play();
    }

    public void PlaySlashUp()
    {
        var slash = GameObject.Instantiate(slashUp);
        slash.gameObject.SetActive(true);
        slash.gameObject.AddComponent<SelfDestruct>();
        slash.GetComponent<SelfDestruct>().selfdestruct_in = 1;
        slash.transform.rotation = slashUp.transform.rotation;
        slash.transform.position = slashUp.transform.position;
        Vector3 pos = slashUp.transform.localScale;
        pos.x *= Mathf.Sign(transform.localScale.z);
        slash.gameObject.transform.localScale = pos;
        slash.Play();
    }
    public void Start_Death_Dissolve()
    {
        _start_dissolve = true;
        mymaterial.SetFloat("_Dissolve_Intensity", _dissolvetimer);
    }
}
