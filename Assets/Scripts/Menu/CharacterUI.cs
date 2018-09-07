using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public PlayerAvatar player;
    public Color baseColor;
    public Image image;

    public bool chosen;

    void Update()
    {
        if (player)
        {
            image.enabled = true;
            image.color = player.myColor;
        }
        else
        {
            image.enabled = false;
        }
    }
}
