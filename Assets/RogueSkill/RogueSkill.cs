using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueSkill : MonoBehaviour
{

    public SpriteRenderer RogueSkillSprite;
    public List<Sprite> SkillStates = new List<Sprite>();
    public ParticleSystem RogueSkillParticle;
    int test;

    private void Start()
    {
        test = 0;
    }

    void Update()
    {
        //DebugKeys();
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
    public void PassState(int state)
    {
        RogueSkillSprite.color = new Color(1, 1, 1, 1);
        RogueSkillSprite.sprite = SkillStates[state];
        RogueSkillParticle.Play();
    }

    public void ResetFeedback()
    {
        RogueSkillSprite.color = new Color(1, 1, 1, 0);
        //test = 0;
    }
}
