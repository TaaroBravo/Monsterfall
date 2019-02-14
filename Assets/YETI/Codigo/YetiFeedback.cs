using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiFeedback : MonoBehaviour {

    //public GameObject dash_Platform;
    public ParticleSystem Skill;

    //public void StartDash()
    //{
    //}
    //public void EndDash()
    //{
    //}
    public void StartSkill()
    {
        Skill.Play();
    }

}
