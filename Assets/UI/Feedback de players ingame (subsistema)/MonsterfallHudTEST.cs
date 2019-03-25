using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class MonsterfallHudTEST : MonoBehaviour {


    GameObject[] players;
    public GameObject hudPrefab;
    public Vector3 offset;
    Image[] hudImages;

    void Start ()
    {
        players = GameObject.FindGameObjectsWithTag("Player");


        List<Image> hudFeedback = new List<Image>();
        foreach (var item in players)
        {
            var hud = Instantiate(hudPrefab, this.transform, true);
            hud.transform.position = Camera.main.WorldToScreenPoint(item.transform.position) + offset;
        }
        hudImages = hudFeedback.ToArray();
	}

    private void Update()
    {
    }

}
