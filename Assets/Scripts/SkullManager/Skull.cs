using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skull : MonoBehaviour {

    public Image Brillo; // el brillo con su animator que lo hace girar
    public Image Skullsp; // arranca con el sprite de la calavera negra ( apagada )
    public List<Sprite> SkullStates; // 0 = normal, 1 = amarilla
    public bool yellowstate;
    public bool normalstate;
    
    public void Activate() // vuelve amarilla la calavera
    {
        Skullsp.sprite = SkullStates[1];
        Brillo.gameObject.SetActive(true);
        yellowstate = true;
    }
    public void Normalize() // vuelve blanca la calavera
    {
        Skullsp.sprite = SkullStates[0];
        Brillo.gameObject.SetActive(false);
        yellowstate = false;
        normalstate = true;
    }
}
