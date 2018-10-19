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

    public void LoadBars(List<PlayerInfo> playerInfo, Action callBack)
    {
        playerInfo = playerInfo.OrderByDescending(x => x.newKills + x.previousKills).ToList();

        for (int i = 0; i < playerInfo.Count; i++)
        {
            var maxKills = playerInfo[i].newKills;
            if (maxKills >= 2)
                CalculateMVP(playerInfo[i].player_number, playerInfo[i].characterChosen, maxKills);
            else
                CalculateMVP(playerInfo[i].player_number, playerInfo[i].characterChosen, 0);
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
        callBack();
    }

    void CalculateMVP(int player_number, int characterChosen, int motive)
    {
        MVPManager.Instance.ShowStand(player_number, characterChosen, motive);
    }

    public void SetRound(int _round)
    {
        round = _round;
    }
}
