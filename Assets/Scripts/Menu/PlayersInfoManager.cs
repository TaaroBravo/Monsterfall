using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersInfoManager : MonoBehaviour {

    public static PlayersInfoManager Instance { get; private set; }

    public List<PlayerInfo> playersInfo = new List<PlayerInfo>();
    public int playersCount;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetUpInfo(List<PlayerAvatar> players)
    {
        playersCount = players.Count;
        for (int i = 0; i < players.Count; i++)
        {
            PlayerInfo info = new PlayerInfo();
            info.player_number = players[i].player_number;
            info.controller = (int)players[i].GetComponent<PlayerInputMenu>().controller;
            info.ID = players[i].GetComponent<PlayerInputMenu>().id;
            info.characterChosen = players[i].characterChosen;
            playersInfo.Add(info);
        }
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);
            StopAllCoroutines();
            break;
        }
    }
}
