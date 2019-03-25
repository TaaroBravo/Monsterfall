using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShaderStateController : MonoBehaviour {

    public PlayerController myplayer;
    public Material myshader;
    //public OverheadController myoverhead;
    //public List<SpriteRenderer> overheaditems;
    public SpriteRenderer playermarker;
    public bool invertedflip;
    float showtimer;
    float deathshow;
    bool overheadappear;
    bool activatestate1;
    bool activatestate2;
    bool activatestate3;

    private void Start()
    {
        myshader.SetFloat("_DissolveIntensity", 1);
        myplayer.GetComponent<PlayerController>();
    }

    void Update () {
        //if (invertedflip) for (int i = 0; i < overheaditems.Count; i++) overheaditems[i].flipX = playermarker.flipX;
        //else for (int i = 0; i < overheaditems.Count; i++) overheaditems[i].flipX = !playermarker.flipX;
        if (showtimer <= 3f)
        {
            showtimer += Time.deltaTime;
        }
        else if (!overheadappear)
        {
            overheadappear = true;
            //myoverhead.Initialize(0);
        }
        if (myplayer.myLife <= 0)
        {
            deathshow += Time.deltaTime;
            myshader.SetFloat("_DissolveIntensity", 1 - deathshow / 1.1f);
            if (!activatestate3)
            {
                activatestate3 = true;
                //myoverhead.State3();
            }
        }
        else if (myplayer.myLife <= 25 && !activatestate2)
        {
            activatestate2 = true;
            //myoverhead.State2();
        }
        else if (myplayer.myLife <= 50 && !activatestate1)
        {
            activatestate1 = true;
            //myoverhead.State1();
        }
    }
}
