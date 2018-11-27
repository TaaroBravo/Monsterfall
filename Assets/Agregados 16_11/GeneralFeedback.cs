using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFeedback : MonoBehaviour {

    List<Color> PColors = new List<Color>();
    public ParticleSystem.MainModule MainElfMark;
    public ParticleSystem ElfMark;
    public ParticleSystem CristalBuff;
    public ParticleSystem CristalBuff2;
    public ParticleSystem.ShapeModule CristalBuffShape;
    public ParticleSystem.ShapeModule CristalBuffShape2;
    float XAxis;
    public bool iminverted;
    public bool reposition;

    private void Start()
    {
        CristalBuffShape = CristalBuff.shape;
        CristalBuffShape2 = CristalBuff2.shape;
        MainElfMark = ElfMark.main;
        Color outcolor;
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) PColors.Add(outcolor); // blue - 0
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) PColors.Add(outcolor); // red - 1
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) PColors.Add(outcolor); // green - 2
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) PColors.Add(outcolor); // yellow - 3
    }
    private void Update()
    {
        XAxis = GetComponent<PlayerInput>().MainHorizontal();
        if (!iminverted)
        {
            if (XAxis > 0)
            {
                CristalBuffShape.rotation = new Vector3(0, 180, 0);
                CristalBuffShape2.rotation = new Vector3(0, 180, 0);
            }
            else if (XAxis < 0)
            {
                CristalBuffShape.rotation = new Vector3(0, 0, 0);
                CristalBuffShape2.rotation = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (XAxis > 0)
            {
                CristalBuffShape.rotation = new Vector3(0, 0, 0);
                CristalBuffShape2.rotation = new Vector3(0, 0, 0);
            }
            else if (XAxis < 0)
            {
                CristalBuffShape.rotation = new Vector3(0, 180, 0);
                CristalBuffShape2.rotation = new Vector3(0, 180, 0);
            }
        }
    }
    public void StartElfMark(int ID)
    {
        MainElfMark.startColor = PColors[ID];
        ElfMark.Play();
    }
    public void FinishElfMark() { ElfMark.Stop(); }
    public void StartCristalBuff() { CristalBuff.Play(); }
    public void FinishCristalBuff() { CristalBuff.Stop(); }
}
