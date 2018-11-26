using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFeedback : MonoBehaviour {

    List<Color> PColors = new List<Color>();
    public ParticleSystem.MainModule MainElfMark;
    public ParticleSystem ElfMark;
    public ParticleSystem CristalBuff;

    private void Start()
    {
        MainElfMark = ElfMark.main;
        Color outcolor;
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) PColors.Add(outcolor); // blue - 0
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) PColors.Add(outcolor); // red - 1
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) PColors.Add(outcolor); // green - 2
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) PColors.Add(outcolor); // yellow - 3
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
