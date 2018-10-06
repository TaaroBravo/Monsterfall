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

    public Collider[] limits;

    private void Awake()
    {
        _instance = this;
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
        }
        StartCoroutine(StartGame(timeToStart));
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

    List<PlayerController> CallSpawnHeroes()
    {
        List<PlayerController> heroes = new List<PlayerController>();
        for (int i = 0; i < _playersCount; i++)
        {
            var hero = SpawnerHeroes.Instance.SpawnHero(infoManager.playersInfo[i].characterChosen, infoManager.playersInfo[i].player_number);
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

    public bool OutOfLimits(Vector3 pos)
    {
        foreach (var limit in limits)
        {
            if (limit.bounds.Contains(pos))
                return true;
        }
        return false;
    }

    public void FinishGame()
    {
        finishedGame = true;
        foreach (var player in myPlayers)
            player.canMove = false;
        StartCoroutine(StartNewGame());
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

}
