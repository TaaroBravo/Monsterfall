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

    public GameObject pauseMenuPanel;
    public bool pauseMenu;
    bool diesByPlayerHit;

    public bool weHaveAWinner;
    public bool canReturnNow;
    public Phantom phantomPrefab;

    private void Awake()
    {
        Instance = this;
        Cursor.visible = false;
        alivePlayers = new List<PlayerController>();
        myPlayers = new List<PlayerController>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(cube, Vector3.one);
    }

    private void Start()
    {
        StartCoroutine(WaitForStart());
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForEndOfFrame();
        Recalculate();
    }

    void Recalculate()
    {
        infoManager = PlayersInfoManager.Instance;
        if (infoManager)
        {
            _playersCount = infoManager.playersCount;
            myPlayers.Clear();
            playersObj.Clear();
            myPlayers = CallSpawnHeroes();
            foreach (var hero in myPlayers)
            {
                playersObj.Add(hero.gameObject);
                hero.OnDeath += x => DeathOfPlayer(x);
            }
            SetUpHUD(infoManager.playersInfo);
            OnSpawnCharacters(myPlayers);
            CreatorRays.Instance.SetPlayers(myPlayers.ToArray());
            ScoreManager.Instance.SetRound(infoManager.playersInfo.First().round);
        }
        alivePlayers = myPlayers;
        foreach (var item in myPlayers)
            item.OnDestroyCharacter += x => LoseCharacter(x);
        lastPos = initialPos;
        pauseMenu = false;
        StartCoroutine(StartGame(timeToStart));
        StartCoroutine(OutOfLimitsPlayer());
    }

    private void Update()
    {
        if (!myPlayers.Any())
            return;
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
            var order = infoManager.playersInfo.OrderByDescending(x => x.newKills + x.previousKills).ToList();
            if (infoManager.playersInfoOrder.Count > 0)
                order = infoManager.playersInfoOrder.ToList();

            firstPlayer = order.Where(x => alivePlayers.Where(y => y != null && y.GetComponent<PlayerInput>()).Select(y => y.GetComponent<PlayerInput>()).Where(y => y.player_number == x.player_number).FirstOrDefault()).FirstOrDefault();
            var secondPlayer = order.Where(x => x != null).Where(x => alivePlayers.Where(y => y != null && y.GetComponent<PlayerInput>()).Select(y => y.GetComponent<PlayerInput>()).Where(y => y.player_number == x.player_number).FirstOrDefault()).Skip(1).FirstOrDefault();
            if (finishedGame)
                return;
            var firstPlayerPosition = alivePlayers.Where(x => firstPlayer != null).Where(x => x.GetComponent<PlayerInput>().player_number == firstPlayer.player_number).FirstOrDefault();
            if (firstPlayerPosition != null && secondPlayer != null && firstPlayer.newKills + firstPlayer.previousKills != secondPlayer.newKills + secondPlayer.previousKills)
                crown.position = firstPlayerPosition.transform.position + (Vector3.up * 7);
            else
                crown.position = Vector3.zero;
        }
    }

    public void SetKills(int player_number)
    {
        foreach (var info in infoManager.playersInfo)
        {
            if (info.player_number == player_number)
            {
                if (alivePlayers.Count == 2 || alivePlayers.Count == 1)
                    diesByPlayerHit = true;
                info.newKills++;
                AudioManager.Instance.CreateSound("Kill");
                AudioManager.Instance.CreateSound("Kill");
            }
        }
    }

    public void LoseCharacter(PlayerController p)
    {
        alivePlayers.Remove(p);
    }

    void DeathOfPlayer(PlayerController p)
    {
        var phantom = Instantiate(phantomPrefab);
        List<Color> PColors = new List<Color>();
        Color outcolor;
        if (ColorUtility.TryParseHtmlString("#1996E1FF", out outcolor)) PColors.Add(outcolor); // blue - 0
        if (ColorUtility.TryParseHtmlString("#E51B1BFF", out outcolor)) PColors.Add(outcolor); // red - 1
        if (ColorUtility.TryParseHtmlString("#5CD025FF", out outcolor)) PColors.Add(outcolor); // green - 2
        if (ColorUtility.TryParseHtmlString("#FBEB11FF", out outcolor)) PColors.Add(outcolor); // yellow - 3
        if (p.lastOneWhoHittedMe)
        {
            var IDKiller = p.lastOneWhoHittedMe.GetComponent<PlayerInput>().player_number;
            phantom.SetColorKiller(PColors[IDKiller]);
        }
        else
            phantom.SetColorKiller(Vector4.zero);
        var IDDead = p.GetComponent<PlayerInput>().player_number;
        phantom.SetColorDeath(PColors[IDDead], p.transform.position);
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
            //SetCooldownHUD(hero.transform.ChildrenWithComponent<CdHUDChecker>().Where(x => x != null).First(), infoManager.playersInfo[i].characterChosen);
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
        AudioManager.Instance.CreateSound("Ready");
        startingGame = true;
        foreach (var player in myPlayers)
        {
            player.myAnim.Play("Stunned");
            player.myAnim.SetBool("Stunned", true);
            player.canInteract = false;
        }
        yield return new WaitForSeconds(x);
        AudioManager.Instance.CreateSound("Fight");
        foreach (var player in myPlayers)
        {
            player.myAnim.SetBool("Stunned", false);
            player.canInteract = true;
        }
        startingGame = false;
    }

    #endregion

    #region Checks
    IEnumerator OutOfLimitsPlayer()
    {
        while (!finishedGame)
        {
            for (int i = 0; i < myPlayers.Count; i++)
            {
                var position = lastPos[i];
                if (myPlayers[i] && !OutOfLimits(myPlayers[i].transform.position))
                    position = myPlayers[i].transform.position;
                yield return new WaitForSeconds(0.1f);
                if (finishedGame)
                    break;
                if (i < myPlayers.Count && myPlayers[i] && OutOfLimits(myPlayers[i].transform.position))
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
        StopCoroutine(OutOfLimitsPlayer());
        foreach (var player in alivePlayers)
        {
            player.canInteract = false;
            if (player)
            {
                foreach (var info in infoManager.playersInfo)
                {
                    if (player.GetComponent<PlayerInput>().id == info.ID)
                    {
                        if (info.newKills + info.previousKills >= 3)
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
            if (infoManager.playersInfo.First().round >= 5)
                WinTheGame();
            else
                StartCoroutine(StartNewRound());
        }
        finishedGame = true;
    }

    bool scoreFinished;
    IEnumerator StartNewRound()
    {
        var firstPlayer = infoManager.playersInfo.Where(x => x.player_number == alivePlayers[0].GetComponent<PlayerInput>().player_number).First();
        if (firstPlayer != null && !diesByPlayerHit)
        {
            firstPlayer.newKills++;
            if (firstPlayer.newKills + firstPlayer.previousKills >= 3)
                WinTheGame();
            else
            {
                ScoreManager.Instance.LoadBars(infoManager.playersInfo, AllScoreFinished, firstPlayer);
                yield return new WaitUntil(() => scoreFinished == true);
                yield return new WaitForSeconds(6f);
                ObjectPoolManager.Instance.Clean();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            ScoreManager.Instance.LoadBars(infoManager.playersInfo, AllScoreFinished, firstPlayer);
            yield return new WaitUntil(() => scoreFinished == true);
            yield return new WaitForSeconds(6f);
            ObjectPoolManager.Instance.Clean();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    void AllScoreFinished(List<PlayerInfo> infos)
    {
        scoreFinished = true;
        infoManager.playersInfoOrder = infos;
    }

    IEnumerator StartNewGame()
    {
        yield return new WaitUntil(() => canReturnNow == true);
        ChargeScene();
    }

    public void PauseMenu(bool state)
    {
        if (state)
        {
            Time.timeScale = 0;
            pauseMenuPanel.SetActive(true);
            AudioManager.Instance.CreateSound("Pause");
            pauseMenu = true;
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuPanel.SetActive(false);
            pauseMenu = false;
        }
    }

    public void ChargeScene()
    {
        if (infoManager)
            Destroy(infoManager.gameObject);
        if (FindObjectOfType<MusicInGame>())
            Destroy(FindObjectOfType<MusicInGame>().gameObject);
        if (FindObjectOfType<PlayersInfoManager>())
            Destroy(FindObjectOfType<PlayersInfoManager>().gameObject);
        SceneManager.LoadScene(0);
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
            player.canInteract = false;
        finishedGame = true;
        weHaveAWinner = true;
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

    public void DestroyObject(GameObject ob)
    {
        Destroy(ob, 2f);
    }



}
