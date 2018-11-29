using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTint : MonoBehaviour {

    public Material lightningmaterial;
    List<Color> PColors = new List<Color>();

    void Start () {
        Color outcolor;
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) PColors.Add(outcolor); // blue - 0
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) PColors.Add(outcolor); // red - 1
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) PColors.Add(outcolor); // green - 2
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) PColors.Add(outcolor); // yellow - 3
    }
    private void Update()
    {
        DebugKeys();
    }
    public void ChangeTint(int ID)
    {
        lightningmaterial.SetColor("_Tinte", PColors[ID]);
    }
    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ChangeTint(Random.Range(0, PColors.Count));
    }
}
