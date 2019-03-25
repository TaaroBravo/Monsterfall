using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadController : MonoBehaviour {

    int colorid;
    public ParticleSystem Deathparticle;
    public SpriteRenderer Glow1small;
    public SpriteRenderer Glow2small;
    public SpriteRenderer Glow1;
    public SpriteRenderer Glow2;
    public SpriteRenderer FullOverhead;
    public List<SpriteRenderer> Parts = new List<SpriteRenderer>();
    List<Vector3> endpositions = new List<Vector3>();
    List<Color> playercolors = new List<Color>();
    bool initialize;
    bool deathanim;
    float timer;
    float delaytime;

	void Start ()
    {
        FullOverhead.sharedMaterial.color = new Color(0, 0, 0, 0);
        Glow2.sharedMaterial.color = new Color(0, 0, 0, 0);
        for (int i = 0; i < Parts.Count; i++) // 1- muevo las partes rotas al centro
        {
            endpositions.Add(Parts[i].transform.position);
            Parts[i].transform.position = FullOverhead.transform.position;
        }
        Color myColor; // 2- preparo los colores de los jugadores con colores hexa
        ColorUtility.TryParseHtmlString("#0095FFFF", out myColor);
        playercolors.Add(myColor); // azul
        ColorUtility.TryParseHtmlString("#FF0000FF", out myColor);
        playercolors.Add(myColor); // rojo
        ColorUtility.TryParseHtmlString("#28FF00FF", out myColor);
        playercolors.Add(myColor); // verde
        ColorUtility.TryParseHtmlString("#FBFF00FF", out myColor);
        playercolors.Add(myColor); // amarillo
    }
    void Update()
    {
        if (initialize)
        {
            timer += Time.deltaTime;
            //Glow2.gameObject.SetActive(true);
            if (timer <= 1)
            {
                if (timer >= 0.1f)
                {
                    FullOverhead.gameObject.SetActive(true);
                    Glow2.gameObject.SetActive(true);
                }
                Color sarasa = new Color(FullOverhead.sharedMaterial.color.r, FullOverhead.sharedMaterial.color.g, FullOverhead.sharedMaterial.color.b, timer);
                Glow2.sharedMaterial.color = sarasa;
                Glow2.gameObject.SetActive(false);
                Glow2.gameObject.SetActive(true);
                FullOverhead.sharedMaterial.color = sarasa;
            }
            else
            {
                Glow1small.gameObject.SetActive(true);
                Glow2small.gameObject.SetActive(true);
                Glow1.gameObject.SetActive(true);
                timer = 0;
                initialize = false;
            }
        }
        if (deathanim) // 3- si estoy muerto reproduzco esta animacion
        {
            timer += Time.deltaTime;
            if (timer >= delaytime)
                for (int i = 0; i < Parts.Count; i++)
                {
                    //Parts[i].transform.position = Vector3.Lerp(FullOverhead.transform.position, endpositions[i], timer - delaytime / 1);
                    Parts[i].sharedMaterial.color = new Color(Parts[i].sharedMaterial.color.r, Parts[i].sharedMaterial.color.g, Parts[i].sharedMaterial.color.b, 1 - timer - delaytime / 0.5f);
                }
            if (timer > 1) for (int i = 0; i < Parts.Count; i++) Parts[i].gameObject.SetActive(false);
        }
    }
    public void State1()
    {
        FullOverhead.GetComponent<Animator>().SetBool("Under50", true);
        switch (colorid)
        {
            case 0:
                Glow1small.GetComponent<Animator>().SetBool("ActivateR", true);
                Glow2small.GetComponent<Animator>().SetBool("ActivateR", true);
                break;
            case 1:
                Glow1small.GetComponent<Animator>().SetBool("ActivateB", true);
                Glow2small.GetComponent<Animator>().SetBool("ActivateB", true);
                break;
            case 2:
                Glow1small.GetComponent<Animator>().SetBool("ActivateG", true);
                Glow2small.GetComponent<Animator>().SetBool("ActivateG", true);
                break;
            case 3:
                Glow1small.GetComponent<Animator>().SetBool("ActivateY", true);
                Glow2small.GetComponent<Animator>().SetBool("ActivateY", true);
                break;
        }
        
    }
    public void State2()
    {
        FullOverhead.GetComponent<Animator>().SetBool("Under20", true);
        switch (colorid)
        {
            case 0:
                Glow1.GetComponent<Animator>().SetBool("ActivateR", true);
                Glow2.GetComponent<Animator>().SetBool("ActivateR", true);
                break;
            case 1:
                Glow1.GetComponent<Animator>().SetBool("ActivateB", true);
                Glow2.GetComponent<Animator>().SetBool("ActivateB", true);
                break;
            case 2:
                Glow1.GetComponent<Animator>().SetBool("ActivateG", true);
                Glow2.GetComponent<Animator>().SetBool("ActivateG", true);
                break;
            case 3:
                Glow1.GetComponent<Animator>().SetBool("ActivateY", true);
                Glow2.GetComponent<Animator>().SetBool("ActivateY", true);
                break;
        }
        Activation(true, true, false, false, true);
    }
    public void State3()
    {
        Deathparticle.Play();
        Activation(false, false, false, false, false);
        deathanim = true;
        for (int i = 0; i < Parts.Count; i++)
        {
            Parts[i].gameObject.SetActive(true);
        }
    }
    public void ChangeState(int state)
    {
        switch (state)
        {
            case 1:
                State1();
                break;
            case 2:
                State2();
                break;
            case 3:
                State3();
                break;
        }
    }
    public void Initialize(int id)
    {
        colorid = id;
        //Activation(false, false, false, false, false);
        timer = 0;
        initialize = true;
        FullOverhead.sharedMaterial.color = playercolors[id];
        Glow1.sharedMaterial.color = playercolors[id];
        Glow2.sharedMaterial.color = playercolors[id];
        Glow1small.sharedMaterial.color = playercolors[id];
        Glow2small.sharedMaterial.color = playercolors[id];
        FullOverhead.sharedMaterial.SetColor("_EmissionColor", playercolors[id]);
        Glow1.sharedMaterial.SetColor("_EmissionColor", playercolors[id]);
        Glow2.sharedMaterial.SetColor("_EmissionColor", playercolors[id]);
        Glow1small.sharedMaterial.SetColor("_EmissionColor", playercolors[id]);
        Glow2small.sharedMaterial.SetColor("_EmissionColor", playercolors[id]);
        for (int i = 0; i < Parts.Count; i++) Parts[i].sharedMaterial.color = playercolors[id];
    }
    void Activation(bool glow1 = true, bool glow2 = true, bool glow1small = true, bool glow2small = true, bool fullover = true)
    {
        Glow1.gameObject.SetActive(glow1);
        Glow2.gameObject.SetActive(glow2);
        Glow1small.gameObject.SetActive(glow1small);
        Glow2small.gameObject.SetActive(glow2small);
        FullOverhead.gameObject.SetActive(fullover);
    }
}
