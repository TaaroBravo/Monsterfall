using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPHud : MonoBehaviour {

    bool dead;
    Color playerOutline;
    public int characterID;
    public float totalHP;
    public float currentHP;
    public Text HpNumber;
    public Image CharPortrait;
    public List<Sprite> AlivePics = new List<Sprite>();
    public List<Sprite> DeadPics = new List<Sprite>();
    void Start()
    {
        playerOutline = HpNumber.GetComponent<Outline>().effectColor; 
    }
    void Update()
    {
        if (currentHP > totalHP) currentHP = totalHP;
        else if (((totalHP / 100) * currentHP) < 30)
        {
            HpNumber.GetComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
            HpNumber.color = new Color(1, 0, 0, 1);
        }
        else
        {
            HpNumber.GetComponent<Outline>().effectColor = playerOutline;
            HpNumber.color = new Color(1, 1, 1, 1);
        }
        dead = currentHP > 0 ? false : true;
        if (!dead)
        {
            HpNumber.text = ((totalHP / 100) * currentHP).ToString("F0");
            CharPortrait.sprite = AlivePics[characterID];
        }
        else
        {
            HpNumber.text = 0.ToString();
            CharPortrait.sprite = DeadPics[characterID];
        }
    }
    public void AssignChar(int ID)
    {
        characterID = ID;
    }

}
