﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class SelectorPlayerManager : MonoBehaviour
{

    private static SelectorPlayerManager _instance;
    public static SelectorPlayerManager Instance
    {
        get
        {
            return _instance;
        }
    }

    InputManager input = new InputManager();

    public List<PlayerAvatar> players = new List<PlayerAvatar>();
    List<PlayerAvatar> _playersInGame = new List<PlayerAvatar>();
    List<PlayerAvatar> _playersReady = new List<PlayerAvatar>();

    public List<CharacterUI> charactersView = new List<CharacterUI>();

    public Vector2 initialPos;

    private void Awake()
    {
        _instance = this;
        Cursor.visible = false;
    }

    void Start()
    {
        List<PlayerInputMenu> playerInputs = new List<PlayerInputMenu>();
        foreach (var player in players)
        {
            player.OnSelectedCharacter += x => OnChosenCharacter(x);
            player.OnRejectedCharacter += x => OnRejectedCharacter(x);
            playerInputs.Add(player.GetComponent<PlayerInputMenu>());
        }
        input.Initializate(playerInputs);
    }

    void Update()
    {
        if (_playersInGame.Count() > 1 && _playersReady.Count() == _playersInGame.Count())
            SetUpInfo();

        for (int i = 0; i < _playersInGame.Count; i++)
        {
            if (_playersInGame[i].characterChosen != 99)
                charactersView[_playersInGame[i].player_number].SetCharacterIndex(_playersInGame[i].characterChosen);
            else
                charactersView[_playersInGame[i].player_number].SetCharacterRandom();
        }
    }

    public void OnConnectedPlayer(PlayerInputMenu player, int ID)
    {
        if (CheckID(ID))
            return;
        if (_playersInGame.Count >= 4)
            return;

        PlayerAvatar newPlayer = input.OnConnectedPlayer(player, ID);
        if (!newPlayer)
            return;
        newPlayer.myImage.enabled = true;
        newPlayer.currentPosition = initialPos;
        newPlayer.transform.position = SectionManager.Instance.ChangePosition(initialPos);
        newPlayer.characterChosen = 99;
        _playersInGame.Add(newPlayer);
        charactersView[newPlayer.player_number].State = charactersView[newPlayer.player_number].SelectingState;
        charactersView[newPlayer.player_number].SetCharacterRandom();
        StartCoroutine(FixTimeConnecting(newPlayer));
        AudioManager.Instance.CreateSound("HitPlayer");
    }

    IEnumerator FixTimeConnecting(PlayerAvatar player)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            player.inGame = true;
            break;
        }
    }

    public void OnDisconnectedPlayer(PlayerInputMenu player)
    {
        PlayerAvatar myAvatar = input.PickMyAvatar(player);
        if (!myAvatar)
            return;
        else if (myAvatar.ready)
            return;

        _playersInGame.Remove(myAvatar);
        myAvatar.inGame = false;
        myAvatar.myImage.enabled = false;
        myAvatar.transform.position = SectionManager.Instance.ChangePosition(initialPos);
        myAvatar.GetComponent<PlayerInputMenu>().id = 0;
        myAvatar.currentPosition = initialPos;
        myAvatar.characterChosen = 99;
        charactersView[myAvatar.player_number].State = charactersView[myAvatar.player_number].StartState;
        charactersView[myAvatar.player_number].SetCharacterRandom();
        input.DisconnecPlayer(myAvatar.GetComponent<PlayerInputMenu>());
    }

    void OnChosenCharacter(PlayerAvatar player)
    {
        _playersReady.Add(player);
        charactersView[player.player_number].State = charactersView[player.player_number].ReadyState;

        if (player.characterChosen != 99)
            charactersView[player.player_number].SetCharacterIndex(player.characterChosen);
        else
        {
            charactersView[player.player_number].SetCharacterRandom();
            player.SelectRandomCharacter();
        }
    }

    void OnRejectedCharacter(PlayerAvatar player)
    {
        charactersView[player.player_number].State = charactersView[player.player_number].SelectingState;
        _playersReady.Remove(player);
    }

    void SetUpInfo()
    {
        GameObject.FindObjectOfType<PlayersInfoManager>().SetUpInfo(_playersReady);
    }

    bool CheckID(int ID)
    {
        foreach (var player in _playersInGame)
        {
            if (player.GetComponent<PlayerInputMenu>().id == ID)
                return true;
        }
        return false;
    }
}
