using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPHud : MonoBehaviour {

    bool dead;
    Color playerOutline;
    public int player_number;
    public int character_chosen;
    public float maxHP;
    public float currentHP;
    private float percentHP;
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
        if (!dead)
            CharPortrait.sprite = AlivePics[character_chosen];
        else
        {
            HpNumber.text = 0.ToString();
            CharPortrait.sprite = DeadPics[character_chosen];
            HpNumber.color = new Color(0, 0, 0, 255);
        }
    }

    public void AssignChar(int ID)
    {
        player_number = ID;
    }

    public void SetCharacter(int character)
    {
        character_chosen = character;
        SetLife();
    }

    void SetLife()
    {
        currentHP = maxHP;
        percentHP = currentHP / maxHP;
    }
    private void UpdateMyLifeHUD(float damage)
    {
        var ratio = damage / maxHP;
        percentHP = currentHP / maxHP;
        percentHP *= 100;
        if (percentHP <= 0)
        {
            dead = true;
            percentHP = 0;
        }
        else if (percentHP < 30f)
        {
            HpNumber.GetComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
            HpNumber.color = new Color(1, 0, 0, 1);
        }
        else
        {
            HpNumber.GetComponent<Outline>().effectColor = playerOutline;
            HpNumber.color = new Color(1, 1, 1, 1);
        }
        HpNumber.text = percentHP.ToString("F0");
    }

    public void TakeDamage(float damage)
    {
        currentHP -= Mathf.RoundToInt(damage);
        UpdateMyLifeHUD(damage);
    }

}
