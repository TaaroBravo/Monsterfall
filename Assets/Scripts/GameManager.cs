using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using FrameworkGoat.ObjectPool;

public class GameManager : MonoBehaviour
{
    public event Action<List<PlayerController>> OnSpawnCharacters = delegate { };

    private int _playersCount;

    public PlayersInfoManager infoManager;
    public List<PlayerController> myPlayers = new List<PlayerController>();
    public List<GameObject> playersObj = new List<GameObject>();
    public GameObject youWin;
    public static GameManager Instance { get; private set; }

    public bool startingGame;
    public bool finishedGame;
    public float timeToStart;

    public Collider[] limits;

    private List<Vector3> initialPos = new List<Vector3>();

    public GameObject victoryCanvas;
    public GameObject finishCanvas;
    public GameObject inGameCanvas;

    private void Awake()
    {
        Instance = this;
        Cursor.visible = false;
    }

    private void Start()
    {
        infoManager = PlayersInfoManager.Instance;
        if (infoManager)
        {
            _playersCount = infoManager.playersCount;
            myPlayers.Clear();
            playersObj.Clear();
            myPlayers = CallSpawnHeroes();
            foreach (var hero in myPlayers)
                playersObj.Add(hero.gameObject);
            SetUpHUD(infoManager.playersInfo);
            OnSpawnCharacters(myPlayers);
            ScoreManager.Instance.SetRound(infoManager.playersInfo.First().round);
        }
        StartCoroutine(StartGame(timeToStart));
        StartCoroutine(OutOfLimitsPlayer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (FindObjectOfType<PlayersInfoManager>())
                Destroy(FindObjectOfType<PlayersInfoManager>().gameObject);
            ObjectPoolManager.Instance.Clean();
            SceneManager.LoadScene(0);
        }
    }

    public void SetKills(int player_number)
    {
        foreach (var info in infoManager.playersInfo)
        {
            if (info.player_number == player_number)
            {
                info.newKills++;
                if(info.newKills + info.previousKills >= 10)
                    WinTheGame();
            }
        }
    }

    #region Start Game
    List<PlayerController> CallSpawnHeroes()
    {
        List<PlayerController> heroes = new List<PlayerController>();
        for (int i = 0; i < _playersCount; i++)
        {
            var hero = SpawnerHeroes.Instance.SpawnHero(infoManager.playersInfo[i].characterChosen, infoManager.playersInfo[i].player_number);
            initialPos.Add(hero.transform.position);
            hero.myLifeUI = FindObjectsOfType<PlayerHPHud>().Where(x => x.player_number == infoManager.playersInfo[i].player_number).First();
            hero.myLifeUI.maxHP = hero.myLife;
            hero.myLifeUI.character_chosen = infoManager.playersInfo[i].characterChosen;
            SetCooldownHUD(hero.transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), infoManager.playersInfo[i].characterChosen);
            SetUpInfoPlayers(hero, i);
            heroes.Add(hero);
        }
        return heroes;
    }

    void SetUpInfoPlayers(PlayerController hero, int i)
    {
        PlayerInput input = hero.GetComponent<PlayerInput>();
        input.controller = (PlayerInput.Controller)infoManager.playersInfo[i].controller;
        input.id = infoManager.playersInfo[i].ID;
        input.player_number = infoManager.playersInfo[i].player_number;
    }

    void SetUpHUD(List<PlayerInfo> players)
    {
        HUDManager.Instance.SetUpHUD(players);
    }

    void SetCooldownHUD(CdHUDChecker cooldownHUD, int character)
    {
        cooldownHUD.character_chosen = character;
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

    #endregion

    #region Checks
    IEnumerator OutOfLimitsPlayer()
    {
        while (true)
        {
            foreach (var item in myPlayers)
            {
                var position = initialPos[0];
                if (item && !OutOfLimits(item.transform.position))
                    position = item.transform.position;
                yield return new WaitForSeconds(0.1f);
                if (item && OutOfLimits(item.transform.position))
                {
                    item.transform.position = position;
                    item.DisableAll();
                    item.DisableStun();
                }
            }
        }
    }

    public bool OutOfLimits(Vector3 pos)
    {
        foreach (var limit in limits)
        {
            if (limit.bounds.Contains(pos))
                return true;
        }
        return false;
    }
    #endregion

    #region Finish Game

    public void FinishGame()
    {
        finishCanvas.SetActive(true);
        inGameCanvas.SetActive(false);
        foreach (var player in myPlayers)
            player.canMove = false;
        if(!finishedGame)
        {
            if (infoManager.playersInfo.First().round >= 8)
                WinTheGame();
            else
                StartCoroutine(StartNewRound());
        }
        finishedGame = true;
    }

    bool scoreFinished;
    IEnumerator StartNewRound()
    {
        while (true)
        {
            ScoreManager.Instance.LoadBars(infoManager.playersInfo, AllScoreFinished);
            yield return new WaitUntil(() => scoreFinished == true);
            yield return new WaitForSeconds(6f);
            ObjectPoolManager.Instance.Clean();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            break;
        }
    }

    //TODO: Falta hacer que cuando se termine la ronda o cuando llegue a 10 kils ganes y se reinicie todo.

    void AllScoreFinished()
    {
        scoreFinished = true;
    }

    IEnumerator StartNewGame()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (FindObjectOfType<PlayersInfoManager>())
                Destroy(FindObjectOfType<PlayersInfoManager>().gameObject);
            ObjectPoolManager.Instance.Clean();
            SceneManager.LoadScene(0);
            break;
        }
    }
    #endregion

    void WinTheGame()
    {
        var winner = infoManager.playersInfo.OrderByDescending(x => x.newKills + x.previousKills).First();
        int player_color = winner.player_number;
        int character = winner.characterChosen;

        victoryCanvas.SetActive(true);
        victoryCanvas.GetComponent<VictoryManager>().AssignValues(player_color, character);
        finishCanvas.SetActive(false);
        inGameCanvas.SetActive(false);
        foreach (var player in myPlayers)
            player.canMove = false;
        finishedGame = true;
    }

}
