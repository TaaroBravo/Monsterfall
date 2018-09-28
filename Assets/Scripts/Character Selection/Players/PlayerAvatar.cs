﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using IA2;

public class PlayerAvatar : MonoBehaviour
{

    public int indexInSection;

    public int player_number;
    public int characterChosen;

    public event Action<PlayerAvatar> OnSelectedCharacter = delegate { };
    public event Action<PlayerAvatar> OnRejectedCharacter = delegate { };

    public bool inGame;
    public bool ready;
    public bool canMove;

    public Section character;

    public Image myImage;

    private void Start()
    {
        myImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (inGame && !ready)
            transform.position = SectionManager.Instance.ChangePosition(indexInSection);
    }

    public void Move(int input)
    {
        indexInSection = SectionManager.Instance.MoveForSections(indexInSection, input);
        SelectCharacter();
    }

    public void ActionButton()
    {
        if (inGame && !ready)
        {
            ready = true;
            OnSelectedCharacter(this);
        }
    }

    public void RejectButton()
    {
        if (inGame && ready)
        {
            ready = false;
            OnRejectedCharacter(this);
        }
    }

    void SelectCharacter()
    {
        characterChosen = SectionManager.Instance.SelectCharacter(indexInSection);
    }

    public void SelectRandomCharacter()
    {
        characterChosen = SectionManager.Instance.RandomCharacter();
    }
}
