using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<PlayerController> myPlayers = new List<PlayerController>();
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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
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
