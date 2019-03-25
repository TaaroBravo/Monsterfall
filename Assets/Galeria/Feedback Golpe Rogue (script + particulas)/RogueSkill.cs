using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RogueSkill : MonoBehaviour
{

    public SpriteRenderer RogueSkillSprite;
    public List<Sprite> SkillStates = new List<Sprite>();
    public ParticleSystem RogueSkillParticle;
    int test;
    List<Color> PColors = new List<Color>();

    //Color[] colors = new Color[]
    //{
    //    Color.blue,
    //    Color.red,
    //    Color.green,
    //    Color.yellow
    //};

    Color myColor;

    Action callBackDisable = delegate { };

    private void Start()
    {
        Color outcolor;
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) PColors.Add(outcolor); // blue - 0
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) PColors.Add(outcolor); // red - 1
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) PColors.Add(outcolor); // green - 2
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) PColors.Add(outcolor); // yellow - 3
    }

    void Update()
    {
        SetTransform();
    }

    void SetTransform()
    {
        Vector3 playerPos = new Vector3(transform.parent.position.x, transform.parent.position.y + transform.parent.GetComponent<Collider>().bounds.extents.y, transform.parent.position.z);
        Vector3 dir = (Camera.main.transform.position - playerPos).normalized;
        transform.position = playerPos + (dir * 6);
        transform.forward = -dir;
    }

    public void PassState(int state, int ID, Action callBack)
    {
        if (PColors[ID] != myColor)
        {
            myColor = PColors[ID];
            callBackDisable();
        }
        callBackDisable = callBack;
        RogueSkillSprite.color = myColor;
        RogueSkillSprite.sprite = SkillStates[state];
        RogueSkillParticle.Play();
    }

    public void ResetFeedback()
    {
        RogueSkillSprite.color = new Color(1, 1, 1, 0);
    }
}
