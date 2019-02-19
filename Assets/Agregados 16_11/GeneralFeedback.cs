using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFeedback : MonoBehaviour
{
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
    float XAxis;
    public bool iminverted;
    public bool reposition;

    private void Start()
    {
        if (iceblock)
            iceblock.SetActive(false);
        CristalBuffShape = CristalBuff.shape;
        CristalBuffShape2 = CristalBuff2.shape;
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
        //XAxis = GetComponent<PlayerInput>().MainHorizontal();
        CristalBuff.gameObject.transform.eulerAngles = new Vector3(270, 180, 0);
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
    public void StartCristalBuff() { CristalBuff.Play(); }
    public void FinishCristalBuff() { CristalBuff.Stop(); }
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
}
