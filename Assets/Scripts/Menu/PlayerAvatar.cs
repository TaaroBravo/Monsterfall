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

    public Image character;
    public Color myColor;
    public Color baseColor;

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
        if (onTarget && !ready)
        {
            ready = true;
            character.color = myColor;
            OnSelectedCharacter(this);
        }
    }

    public void RejectButton()
    {
        if(ready)
        {
            ready = false;
            OnRejectedCharacter(this);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            string[] character = collision.gameObject.name.Split('-');
            characterChosen = int.Parse(character[1]);
            onTarget = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Target")
            onTarget = false;
    }
}
