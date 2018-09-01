using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    PlayersInfoManager infoManager;
    public List<PlayerController> myPlayers = new List<PlayerController>();
    public List<GameObject> playersObj = new List<GameObject>();
    public GameObject youWin;
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public bool startingGame;
    public bool finishedGame;
    public float timeToStart;

    private void Awake()
    {
        _instance = this;
        Cursor.visible = false;
    }

    private void Start()
    {
        StartCoroutine(StartGame(timeToStart));
        if (GameObject.FindObjectOfType<PlayersInfoManager>())
        {
            infoManager = GameObject.FindObjectOfType<PlayersInfoManager>().GetComponent<PlayersInfoManager>();
            SetUpInfoPlayers();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

    void SetUpInfoPlayers()
    {
        for (int i = 0; i < myPlayers.Count(); i++)
        {
            GameObject character = playersObj[infoManager.playersInfo[i].characterChosen];
            PlayerInput input = character.GetComponent<PlayerInput>();
            input.controller = (PlayerInput.Controller)infoManager.playersInfo[i].controller;
            input.id = infoManager.playersInfo[i].ID;
        }
    }

    IEnumerator StartGame(float x)
    {
        while (true)
        {
            startingGame = true;
            foreach (var player in myPlayers)
            {
                player.myAnim.Play("Stunned");
                player.myAnim.SetBool("Stunned", true);
                player.canMove = false;
            }
            yield return new WaitForSeconds(x);
            foreach (var player in myPlayers)
            {
                player.myAnim.SetBool("Stunned", false);
                player.canMove = true;
            }
            startingGame = false;
            break;
        }
    }

    public void FinishGame()
    {
        finishedGame = true;
        foreach (var player in myPlayers)
            player.canMove = false;
    }

}
