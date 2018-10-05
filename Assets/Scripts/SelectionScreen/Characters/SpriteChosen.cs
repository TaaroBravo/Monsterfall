using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChosen : MonoBehaviour
{
    public PlayerAvatar player;
    public Image myAvatar;

    public Sprite[] posibleCharacters;

    void Update()
    {
        if (player.ready)
        {
            myAvatar.enabled = true;
            myAvatar.sprite = posibleCharacters[player.characterChosen];
        }
        else
        {
            myAvatar.enabled = false;
        }
    }
}
