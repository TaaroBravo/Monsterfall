using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerAvatar : MonoBehaviour {

    public int characterChosen;

    public event Action<PlayerAvatar> OnSelectedCharacter = delegate { };
    public event Action<PlayerAvatar> OnRejectedCharacter = delegate { };

    PlayerInputMenu input;
    bool onTarget;

    public bool ready;
    public bool canMove;

    public CharacterUI character;
    public Color myColor;

    float speed;

    private void Start()
    {
        input = GetComponent<PlayerInputMenu>();
        speed = 200;
    }

    private void Update()
    {
        if(!ready)
            transform.position += new Vector3(input.MainHorizontal() * Time.deltaTime * speed, input.MainVertical() * Time.deltaTime * speed, 0);
    }

    public void ActionButton()
    {
        if (onTarget && !ready && !character.player)
        {
            ready = true;
            character.player = this;
            character.chosen = true;
            OnSelectedCharacter(this);
        }
    }

    public void RejectButton()
    {
        if(ready)
        {
            ready = false;
            character.player = null;
            character.chosen = false;
            OnRejectedCharacter(this);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            character = collision.GetComponent<CharacterUI>();
            string[] myChar = collision.gameObject.name.Split('-');
            characterChosen = int.Parse(myChar[1]);
            onTarget = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            character = null;
            onTarget = false;
        }
    }
}
