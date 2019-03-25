using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RogueSkillCall : MonoBehaviour {

    public RogueSkill skill;

	void Start ()
    {
        skill = transform.Find("RogueSkillFeedback").GetComponent<RogueSkill>();
	}

    public void PassState(int state, int ID, Action callBack)
    {
        skill.PassState(state, ID, callBack);
    }

    public void ResetFeedback()
    {
        skill.ResetFeedback();
    }
}
