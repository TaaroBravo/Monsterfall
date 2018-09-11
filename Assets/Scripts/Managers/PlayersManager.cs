﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayersManager : MonoBehaviour
{

    public List<PlayerController> myPlayers = new List<PlayerController>();

    private void Awake()
    {
        GameManager.Instance.OnSpawnCharacters += x => SetPlayers(x);
    }

    void Update()
    {
        var alivePlayers = myPlayers.Where(x => x != null && x.GetComponent<PlayerController>());
        if (alivePlayers.Count() <= 1)
        {
            alivePlayers.First().myAnim.Play("Victory");
            GameManager.Instance.FinishGame();
        }
    }

    void SetPlayers(List<PlayerController> heroes)
    {
        myPlayers = heroes;
    }
}
