using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

    public static HUDManager Instance { get; private set; }

    public List<GameObject> particles = new List<GameObject>();
    public List<GameObject> HUD_lifes = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void SetUpHUD(List<PlayerInfo> players)
    {
        for (int i = 0; i < HUD_lifes.Count; i++)
        {
            particles[i].SetActive(false);
            HUD_lifes[i].SetActive(false);
        }
        for (int i = 0; i < players.Count; i++)
        {
            particles[players[i].player_number].SetActive(true);
            HUD_lifes[players[i].player_number].SetActive(true);
        }
    }
}
