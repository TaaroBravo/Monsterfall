using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KillsCount : MonoBehaviour {

    //TODO: Referencia a mi player

    public bool[] kills;
    private int currentKills;

    void Awake()
    {
        kills = new bool[10];
    }

    private void Update()
    {
        if (kills.Any(x => x == true))
            ShowScore();
    }
    
    public void SetOldScore(int old)
    {
        currentKills = old;
    }

    public void SetKill()
    {
        currentKills++;
        for (int i = 0; i < currentKills; i++)
            kills[i] = true;
    }

    void ShowScore()
    {
        //Feedback de cuando aparece las nuevas kills.
    }
}
