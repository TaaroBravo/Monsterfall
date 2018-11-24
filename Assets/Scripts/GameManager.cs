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
    public List<PlayerController> alivePlayers = new List<PlayerController>();
    public List<GameObject> playersObj = new List<GameObject>();
    public GameObject youWin;
    public static GameManager Instance { get; private set; }

    public bool startingGame;
    public bool finishedGame;
    public float timeToStart;

    public Collider[] limits;

    private List<Vector3> initialPos = new List<Vector3>();
    private List<Vector3> lastPos = new List<Vector3>();

    public GameObject victoryCanvas;
    public GameObject finishCanvas;
    public GameObject inGameCanvas;

    public Transform crown;
    public Vector3 cube;

    private void Awake()
    {
        Instance = this;
        Cursor.visible = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(cube, Vector3.one);
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
            CreatorRays.Instance.SetPlayers(myPlayers.ToArray());
            ScoreManager.Instance.SetRound(infoManager.playersInfo.First().round);
        }
        alivePlayers = myPlayers;
        foreach (var item in myPlayers)
            item.OnDestroyCharacter += x => LoseCharacter(x);
        lastPos = initialPos;
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

        if (infoManager.playersInfo.First().round > 0)
        {
            PlayerInfo firstPlayer = new PlayerInfo();
            var numberOfKills = infoManager.playersInfo.Where(x => myPlayers[x.ID - 1] != null).Select(x => x.newKills + x.previousKills);
            foreach (var info in infoManager.playersInfo.Where(x => myPlayers[x.ID - 1] != null))
            {
                if (info.newKills + info.previousKills == numberOfKills.OrderByDescending(z => z).First())
                    firstPlayer = info;
            }
            if (myPlayers[firstPlayer.ID - 1] != null)
                crown.position = myPlayers[firstPlayer.ID - 1].transform.position + (Vector3.up * 7);
            else
                crown.position = Vector3.zero;
        }
    }

    public void SetKills(int player_number)
    {
        foreach (var info in infoManager.playersInfo)
        {
            if (info.player_number == player_number)
                info.newKills++;
        }
    }

    public void LoseCharacter(PlayerController p)
    {
        alivePlayers.Remove(p);
    }

    #region Start Game
    List<PlayerController> CallSpawnHeroes()
    {
        List<PlayerController> heroes = new List<PlayerController>();
        for (int i = 0; i < _playersCount; i++)
        {
            var hero = SpawnerHeroes.Instance.SpawnHero(infoManager.playersInfo[i].characterChosen, infoManager.playersInfo[i].player_number);
            initialPos.Add(hero.transform.position);
            //hero.myLifeUI = FindObjectsOfType<PlayerHPHud>().Where(x => x.player_number == infoManager.playersInfo[i].player_number).First();
            //hero.myLifeUI.maxHP = hero.myLife;
            //hero.myLifeUI.character_chosen = infoManager.playersInfo[i].characterChosen;
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
            for (int i = 0; i < myPlayers.Count; i++)
            {
                var position = lastPos[i];
                if (myPlayers[i] && !OutOfLimits(myPlayers[i].transform.position))
                    position = myPlayers[i].transform.position;
                yield return new WaitForSeconds(0.1f);
                if (myPlayers[i] && OutOfLimits(myPlayers[i].transform.position))
                {
                    myPlayers[i].transform.position = position;
                    myPlayers[i].DisableAll();
                    myPlayers[i].DisableStun();
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

    public void RegisterLastPos(PlayerController player, Vector3 pos)
    {
        for (int i = 0; i < myPlayers.Count; i++)
        {
            if (myPlayers[i] == player)
                lastPos[i] = pos;
        }
    }
    #endregion

    #region Finish Game

    public void FinishGame()
    {
        foreach (var player in alivePlayers)
        {
            player.canMove = false;
            if (player)
            {
                foreach (var info in infoManager.playersInfo)
                {
                    if (player.GetComponent<PlayerInput>().id == info.ID)
                    {
                        if (info.newKills + info.previousKills >= 10)
                        {
                            if (alivePlayers.Contains(player))
                                WinTheGame();
                            else
                                info.newKills = 0;
                        }
                    }
                }
            }
        }
        if (!finishedGame)
        {
            finishCanvas.SetActive(true);
            inGameCanvas.SetActive(false);
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
            yield return new WaitForSeconds(6f);
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
        int winner_color = winner.player_number;
        int winner_character = winner.characterChosen;

        finishCanvas.SetActive(false);
        inGameCanvas.SetActive(false);
        victoryCanvas.SetActive(true);
        victoryCanvas.GetComponent<VictoryManager>().AssignValues(winner_color, winner_character);
        foreach (var player in myPlayers)
            player.canMove = false;
        finishedGame = true;
        StartCoroutine(StartNewGame());
    }

    public Vector3 ClosesPlayer(Vector3 pos)
    {
        Vector3 closes = Vector3.zero;
        float distance = 100000;
        foreach (var player in alivePlayers)
        {
            if ((player.transform.position - pos).magnitude < distance)
            {
                closes = player.transform.position;
                distance = (player.transform.position - pos).magnitude;
            }
        }
        return closes;
    }



}
