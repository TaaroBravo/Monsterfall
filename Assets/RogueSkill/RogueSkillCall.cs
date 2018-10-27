using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueSkillCall : MonoBehaviour {

    public RogueSkill skill;

	void Start ()
    {
        skill = transform.Find("RogueSkillFeedback").GetComponent<RogueSkill>();
	}

    public void PassState(int state)
    {
        skill.PassState(state);
    }

    public void ResetFeedback()
    {
        skill.ResetFeedback();
    }
}
