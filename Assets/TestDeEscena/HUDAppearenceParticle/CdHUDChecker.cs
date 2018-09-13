using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CdHUDChecker : MonoBehaviour {

    public List<Sprite> SkillHUDs = new List<Sprite>();
    public GameObject CDText;
    public ParticleSystem PS_ReadySkill;
    public bool onCD;
    bool showCDHud;
    float hidetimer;
    float currentCD;
    public float maxCD;

    void Start()
    {
        AssignSkill(0); // DEBUG
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        CDText.GetComponent<TextMesh>().color = new Color(1, 1, 1, 0);
    }
    void Update()
    {
        DebugKeys();
        if (onCD) currentCD -= Time.deltaTime;
        if (currentCD < 0)
        {
            if (onCD) PS_ReadySkill.Play();
            currentCD = 0;
            onCD = false;
        }
        CDText.GetComponent<TextMesh>().text = currentCD.ToString("F1");
        if (showCDHud)
        {
            hidetimer += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - hidetimer / 0.5f);
            CDText.GetComponent<TextMesh>().color = new Color(1, 1, 1, 1 - hidetimer / 0.5f);
        }
        if (hidetimer >= 0.5f)
        {
            showCDHud = false;
            hidetimer = 0;
        }
    }
    void AssignSkill(int ID)
    {
        GetComponent<SpriteRenderer>().sprite = SkillHUDs[ID];
    }
    void UseSkill()
    {
        if (!showCDHud) showCDHud = true;
        if (!onCD)
        {
            onCD = true;
            currentCD = maxCD;
        }
    }
    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.M)) UseSkill();
    }
}
