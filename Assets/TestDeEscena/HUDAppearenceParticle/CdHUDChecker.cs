using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CdHUDChecker : MonoBehaviour {

    public int character_chosen;
    public List<Sprite> SkillHUDs = new List<Sprite>();
    public GameObject CDText;
    public ParticleSystem PS_ReadySkill;
    public bool onCD;
    bool showCDHud;
    float hidetimer;
    float currentCD;

    Vector3 _localScale;

    void Start()
    {
        AssignSkill(character_chosen);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        CDText.GetComponent<TextMesh>().color = new Color(1, 1, 1, 0);
        _localScale = transform.localScale;
    }
    void Update()
    {
        //DebugKeys();
        SetTransform();
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

    void SetTransform()
    {
        Vector3 playerPos = transform.parent.position;
        Vector3 dir = (Camera.main.transform.position - playerPos).normalized;
        transform.position = playerPos + (dir * 6);
        transform.forward = -dir;
    }

    public void AssignSkill(int ID)
    {
        GetComponent<SpriteRenderer>().sprite = SkillHUDs[ID];
    }

    public void UseSkill(float maxCD)
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
        //if (Input.GetKeyDown(KeyCode.M)) UseSkill();
    }
}
