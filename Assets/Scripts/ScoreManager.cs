using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance { get; private set; }

    public GameObject[] pointBars = new GameObject[0];
    public Image roundNumber;
    public Sprite[] roundCounters = new Sprite[5];
    public int round;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        roundNumber.sprite = roundCounters[round];
    }

    public void LoadBars(List<PlayerInfo> playerInfo, Action<List<PlayerInfo>> callBack, PlayerInfo alivePlayer)
    {
        playerInfo = playerInfo.OrderByDescending(x => x.newKills + x.previousKills).ToList();
        if(alivePlayer != playerInfo.First() && alivePlayer.newKills + alivePlayer.previousKills == playerInfo.First().newKills + playerInfo.First().previousKills)
        {
            PlayerInfo tempFirst = playerInfo[0];
            int index = playerInfo.FindIndex(0, playerInfo.Count(), x => x == alivePlayer);
            playerInfo[0] = playerInfo[index];
            playerInfo[index] = tempFirst;
        }
        var maxKills = playerInfo.Select(x => x.newKills).First();
        if (maxKills >= 2)
            CalculateMVP(playerInfo.Select(x => x.player_number).First(), playerInfo.Select(x => x.characterChosen).First(), maxKills);
        else
            CalculateMVP(playerInfo.OrderByDescending(x => x.newKills).Select(x => x.player_number).First(), playerInfo.OrderByDescending(x => x.newKills).Select(x => x.characterChosen).First(), 1);
        for (int i = 0; i < playerInfo.Count; i++)
        {
            pointBars[i].SetActive(true);
            var pb = pointBars[i].GetComponent<PointsBar>();
            pb.SetCharacterSpriteAndColor(playerInfo[i]);
            pb.sm.ActivateSkulls(playerInfo[i].previousKills);
            pb.sm.NormalizeSkulls();
            pb.sm.ActivateSkulls(playerInfo[i].newKills);
            playerInfo[i].previousKills += playerInfo[i].newKills;
            playerInfo[i].newKills = 0;

            playerInfo[i].round++;
        }
        callBack(playerInfo);
    }

    //TODO: Hacer que se calcule el daño que hizo al contrario y aparezca primero.
    void CalculateMVP(int player_number, int characterChosen, int motive)
    {
        MVPManager.Instance.ShowStand(player_number, characterChosen, motive - 1);
    }

    public void SetRound(int _round)
    {
        round = _round;
    }
}
