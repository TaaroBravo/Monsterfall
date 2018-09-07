using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersInfoManager : MonoBehaviour {

    public List<PlayerInfo> playersInfo = new List<PlayerInfo>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetUpInfo(List<PlayerAvatar> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            PlayerInfo info = new PlayerInfo();
            info.controller = (int)players[i].GetComponent<PlayerInputMenu>().controller;
            info.ID = players[i].GetComponent<PlayerInputMenu>().id;
            info.characterChosen = players[i].characterChosen;
            playersInfo.Add(info);
        }
        StartGame();
    }
	
	void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
