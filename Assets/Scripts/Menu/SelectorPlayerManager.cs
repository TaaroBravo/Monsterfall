using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SelectorPlayerManager : MonoBehaviour {

    private static SelectorPlayerManager _instance;
    public static SelectorPlayerManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public List<PlayerAvatar> players = new List<PlayerAvatar>();
    List<PlayerAvatar> _playersReady = new List<PlayerAvatar>();

	void Start ()
    {
        foreach (var player in players)
        {
            player.OnSelectedCharacter += x => OnChosenCharacter(x);
            player.OnRejectedCharacter += x => OnRejectedCharacter(x);
        }
	}
	
	void Update ()
    {
        if (_playersReady.Count() == 4)
            SetUpInfo();
	}

    void OnChosenCharacter(PlayerAvatar player)
    {
        _playersReady.Add(player);
    }

    void OnRejectedCharacter(PlayerAvatar player)
    {
        _playersReady.Remove(player);
    }

    void SetUpInfo()
    {
        GameObject.FindObjectOfType<PlayersInfoManager>().SetUpInfo(_playersReady);
    }
}
