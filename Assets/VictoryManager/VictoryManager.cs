using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour {

    public Image BlackScreen;
    public Image Circle;
    public Image Banner;
    public Image VictoryText;
    public List<Image> Heroes = new List<Image>();
    public Image BorderGlow1;
    public Image BorderGlow2;
    List<Color> PlayerColors = new List<Color>();
    List<Vector3> FinalPositions = new List<Vector3>();
    List<Vector3> InitialPositions = new List<Vector3>();
    List<Vector3> FinalScale = new List<Vector3>();
    List<Vector3> InitialScale = new List<Vector3>();
    float animationtimer;
    bool startanimation;
    int activehero;

    void Start()
    {
        FinalPositions.Add(BorderGlow1.GetComponent<RectTransform>().position + new Vector3(270, 0, 0));
        FinalPositions.Add(BorderGlow2.GetComponent<RectTransform>().position + new Vector3(-270, 0, 0));
        InitialPositions.Add(BorderGlow1.GetComponent<RectTransform>().position);
        InitialPositions.Add(BorderGlow2.GetComponent<RectTransform>().position);
        for (int i = 0; i < Heroes.Count; i++)
        {
            FinalScale.Add(Heroes[i].GetComponent<RectTransform>().localScale);
            InitialScale.Add(Heroes[i].GetComponent<RectTransform>().localScale * 10);
        }
        var color = new Color(0, 0, 0, 0);
        if (ColorUtility.TryParseHtmlString("#00cbff", out color)) PlayerColors.Add(color); // azul
        if (ColorUtility.TryParseHtmlString("#ec4312", out color)) PlayerColors.Add(color); // rojo
        if (ColorUtility.TryParseHtmlString("#96ff00", out color)) PlayerColors.Add(color); // verde
        if (ColorUtility.TryParseHtmlString("#fff000", out color)) PlayerColors.Add(color); // amarillo
    }
    private void Update()
    {
        DebugKeys();
        if (startanimation)
        {
            animationtimer += Time.deltaTime;
            BorderGlow1.GetComponent<RectTransform>().position = Vector3.Lerp(InitialPositions[0],FinalPositions[0],animationtimer / 2);
            BorderGlow2.GetComponent<RectTransform>().position = Vector3.Lerp(InitialPositions[1], FinalPositions[1], animationtimer / 2);
            if (BlackScreen.color.a < 0.4f)
            BlackScreen.color = new Color(BlackScreen.color.r, BlackScreen.color.g, BlackScreen.color.b, animationtimer / 3f);
            Circle.color = new Color(Circle.color.r, Circle.color.g, Circle.color.b, animationtimer / 1f);
            Banner.color = new Color(Banner.color.r, Banner.color.g, Banner.color.b, animationtimer / 1f);
            VictoryText.color = new Color(1, 1, 1, animationtimer / 1f);
            Heroes[activehero].GetComponent<RectTransform>().localScale = Vector3.Lerp(InitialScale[activehero],FinalScale[activehero],animationtimer / 1f);
            Heroes[activehero].color = new Color(1, 1, 1, animationtimer / 1f);
        }
    }
    public void AssignValues(int id, int hero)
    {
        Circle.color = PlayerColors[id];
        Banner.color = PlayerColors[id];
        BorderGlow1.color = PlayerColors[id];
        BorderGlow2.color = PlayerColors[id];
        Heroes[hero].gameObject.SetActive(true);
        activehero = hero;
        startanimation = true;
    }
    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.Q)) AssignValues(Random.Range(0, 4), Random.Range(0, 4));
    }
}
