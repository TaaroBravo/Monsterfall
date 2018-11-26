using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexColors : MonoBehaviour {

    public Material mattomodify;
    public List<Color> Colors = new List<Color>();

    private void Start()
    {
        Color outcolor;
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) Colors.Add(outcolor); // red
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) Colors.Add(outcolor); // blue
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) Colors.Add(outcolor); // green
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) Colors.Add(outcolor); // yellow
    }
    void Update()
    {
        DebugKeys();
    }
    public void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            mattomodify.SetColor("_Color", Colors[Random.Range(0, Colors.Count)]);
    }
}
