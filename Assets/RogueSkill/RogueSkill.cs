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



    Color[] colors = new Color[]
    {
        Color.blue,
        Color.red,
        Color.green,
        Color.yellow
    };

    Color myColor;

    Action callBackDisable = delegate { };

    private void Start()
    {

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

    //void DebugKeys()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q) && test <= 3)
    //    {
    //        PassState(test);
    //        test++;
    //    }
    //    if (Input.GetKeyDown(KeyCode.W)) ResetFeedback();
    //}

    public void PassState(int state, int ID, Action callBack)
    {
        Debug.Log(colors[ID]);
        Debug.Log(myColor);
        if (colors[ID] != myColor)
        {
            myColor = colors[ID];
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
        //test = 0;
    }
}
