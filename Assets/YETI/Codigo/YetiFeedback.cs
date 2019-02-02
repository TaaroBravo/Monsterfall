using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiFeedback : MonoBehaviour {

    public GameObject dash_Platform;
    public ParticleSystem Skill;

    #region Intento para que se quede quieta la plataforma cuando usa el dash
    Vector3 Platform_initial_position; //
    Vector3 Platform_position; //
    bool PlatformAppear;//

    void Awake()
    {
        Platform_initial_position = dash_Platform.transform.localPosition; //
    }
    void Update()
    {
        if (PlatformAppear) dash_Platform.transform.position = Platform_position; //
    }
    #endregion

    public void StartDash()
    {
        dash_Platform.SetActive(true);

        Platform_position = dash_Platform.transform.position; // intento
        PlatformAppear = true; // intento
    }
    public void EndDash()
    {
        dash_Platform.SetActive(false);

        PlatformAppear = false; // intento
        dash_Platform.transform.position = Platform_initial_position; // intento
    }
    public void StartSkill()
    {
        Skill.Play();
    }

}
