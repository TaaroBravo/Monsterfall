using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfTeleportMissileFeedback : MonoBehaviour {

    List<Color> PColors = new List<Color>();
    public ParticleSystem.MainModule Part1Main;
    public ParticleSystem Part1;
    public ParticleSystem.MainModule Part2Main;
    public ParticleSystem Part2;
    public ParticleSystem.MainModule Part3Main;
    public ParticleSystem Part3;

    private void Awake()
    {
        Part1Main = Part1.main;
        Part2Main = Part2.main;
        Part3Main = Part3.main;
        Color outcolor;
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) PColors.Add(outcolor); // blue - 0
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) PColors.Add(outcolor); // red - 1
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) PColors.Add(outcolor); // green - 2
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) PColors.Add(outcolor); // yellow - 3
    }
    public void ChangeColor(int ID)
    {
        Part1Main.startColor = PColors[ID];
        Part2Main.startColor = PColors[ID];
        Part3Main.startColor = PColors[ID];
    }
}
