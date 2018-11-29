using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifebarController : MonoBehaviour
{

    public PlayerInput myplayerinput;
    float _SkillCD;
    float _DashCD;
    float _totalLife;
    float _currentLife;
    //public Image BackgroundParent;
    public Image SkillCDBar;
    public Image DashCDBar;
    public Image LifeBar;
    bool SkillOnCD;
    float SkillCDTimer;
    bool DashOnCD;
    float DashCDTimer;
    Color FinalDashColor;
    Color FinalSkillColor;
    Color FinalLifeColor;
    float XAxis;
    public bool iminverted;
    public float fixtimer;

    public void Set(float skillCD, float dashCD, float totalLife)
    {
        _SkillCD = skillCD;
        _DashCD = dashCD;
        _totalLife = totalLife;
        SkillCDBar.fillAmount = 1;
        DashCDBar.fillAmount = 1;
        LifeBar.fillAmount = 1;
        _currentLife = _totalLife;
        FinalSkillColor = SkillCDBar.color;
        FinalDashColor = DashCDBar.color;
        FinalLifeColor = LifeBar.color;
    }
    public void ReduceLife(float ammount) { _currentLife -= ammount; }
    public void ActivateSkillCD() { SkillOnCD = true; SkillCDBar.fillAmount = 0; }
    public void ActivateDashCD() { DashOnCD = true; DashCDBar.fillAmount = 0; }

    private void Start()
    {
    }
    void Update()
    {
        fixtimer += Time.deltaTime;
        XAxis = fixtimer > 0.2f ? myplayerinput.MainHorizontal() : 0.01f;
        //XAxis = myplayerinput.MainHorizontal();
        //if (myplayerinput.MainHorizontal() != 0) XAxis = myplayerinput.MainHorizontal();
        if (!iminverted)
        {
            if (XAxis >= 0) GetComponent<RectTransform>().rotation = new Quaternion(0, 90, 0, 0);
            else if (XAxis < 0) GetComponent<RectTransform>().rotation = new Quaternion(0, -90, 0, 0);
        }
        else
        {
            if (XAxis >= 0) GetComponent<RectTransform>().rotation = new Quaternion(0, -90, 0, 0);
            else if (XAxis < 0) GetComponent<RectTransform>().rotation = new Quaternion(0, 90, 0, 0);
        }
        //Debugkeys();
        if (_totalLife > 0)
        {
            LifeBar.fillAmount = _currentLife / _totalLife;
            LifeBar.color = new Color(FinalLifeColor.r, FinalLifeColor.g, FinalLifeColor.b, 1f - (_currentLife / _totalLife) / 4);
        }
        if (SkillOnCD)
        {
            SkillCDBar.color = new Color(FinalSkillColor.r, FinalSkillColor.g, FinalSkillColor.b, 0.6f);
            SkillCDTimer += Time.deltaTime;
            SkillCDBar.fillAmount = SkillCDTimer / _SkillCD;
            if (SkillCDTimer >= _SkillCD)
            {
                SkillOnCD = false;
                SkillCDTimer = 0;
                SkillCDBar.color = FinalSkillColor;
            }
        }
        if (DashOnCD)
        {
            DashCDBar.color = new Color(FinalDashColor.r, FinalDashColor.g, FinalDashColor.b, 0.6f);
            DashCDTimer += Time.deltaTime;
            DashCDBar.fillAmount = DashCDTimer / _DashCD;
            if (DashCDTimer >= _DashCD)
            {
                DashOnCD = false;
                DashCDTimer = 0;
                DashCDBar.color = FinalDashColor;
            }
        }
    }
    void Debugkeys()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var temp = Random.Range(0f, 5f);
            var temp2 = Random.Range(0f, 5f);
            Debug.Log(temp + "  " + temp2);
            Set(temp, temp2, 100);
        }
        if (Input.GetKeyDown(KeyCode.W)) ActivateSkillCD();
        if (Input.GetKeyDown(KeyCode.E)) ActivateDashCD();
        if (Input.GetKeyDown(KeyCode.R))
        {
            var temp = Random.Range(0, 33);
            Debug.Log(temp);
            ReduceLife(temp);
        }
    }
}
