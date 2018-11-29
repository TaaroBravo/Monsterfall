using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersInfoManager : MonoBehaviour {

    public static PlayersInfoManager Instance { get; private set; }

    public List<PlayerInfo> playersInfo = new List<PlayerInfo>();
    public List<PlayerInfo> playersInfoOrder = new List<PlayerInfo>();
    public int playersCount;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetUpInfo(List<PlayerAvatar> players)
    {
        playersCount = players.Count;
        if(playersInfo.Count != playersCount)
        {
            for (int i = 0; i < playersCount; i++)
            {
                PlayerInfo info = new PlayerInfo();
                info.player_number = players[i].player_number;
                info.controller = (int)players[i].GetComponent<PlayerInputMenu>().controller;
                info.ID = players[i].GetComponent<PlayerInputMenu>().id;
                info.characterChosen = players[i].characterChosen;
                playersInfo.Add(info);
            }
        }
        Loading.Instance.ChangeScene();
    }
}
