using System.Collections;
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
                charactersView[i].SetCharacterIndex(_playersInGame[i].characterChosen);
            else
                charactersView[i].SetCharacterRandom();
        }
    }

    public void OnConnectedPlayer(PlayerInputMenu player)
    {
        if (_playersInGame.Count >= 4)
            return;

        PlayerAvatar newPlayer = input.OnConnectedPlayer(player);
        if (!newPlayer)
            return;
        newPlayer.myImage.enabled = true;
        newPlayer.transform.position = SectionManager.Instance.ChangePosition(0);
        newPlayer.characterChosen = 99;
        _playersInGame.Add(newPlayer);
        newPlayer.player_number = _playersInGame.Count - 1;
        charactersView[_playersInGame.Count - 1].State = charactersView[_playersInGame.Count - 1].SelectingState;
        StartCoroutine(FixTimeConnecting(newPlayer));
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
        myAvatar.transform.position = SectionManager.Instance.ChangePosition(0);
        myAvatar.GetComponent<PlayerInputMenu>().id = 0;
        myAvatar.indexInSection = 0;
        charactersView[myAvatar.player_number].SetCharacterRandom();
        charactersView[myAvatar.player_number].State = charactersView[myAvatar.player_number].StartState;
        input.DisconnecPlayer(myAvatar.GetComponent<PlayerInputMenu>());
    }

    void OnChosenCharacter(PlayerAvatar player)
    {
        _playersReady.Add(player);
        //player.player_number funca?
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
        player.characterChosen = 99;
        _playersReady.Remove(player);
    }

    void SetUpInfo()
    {
        GameObject.FindObjectOfType<PlayersInfoManager>().SetUpInfo(_playersReady);
    }
}
