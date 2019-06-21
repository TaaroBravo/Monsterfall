using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

public class MainMenu_Selector : MonoBehaviour
{

    MainMenu_Manager MM_Manager;
    public List<Transform> positions;
    public float rotation_speed;
    int currentindex;
    int currentindexInOptions;
    float currentindexInSlider;
    Dictionary<int, Slider> slidePositions;

    bool loading;
    bool pauseMenu;
    bool controlsMenu;

    public GameObject controlsCanvas;
    public GameObject optionsCanvas;

    bool controlsCanvasActive;
    bool optionsCanvasActive;

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    PlayerIndex[] playerIndices;

    bool _cooldown = true;

    public Slider generalSlider;
    public Slider soundSlider;
    public Slider musicSlider;
    public Slider graphicsSlider;

    public Image[] handle;
    public Sprite handleActive;
    public Sprite handleDisable;

    public AudioMixer mixer;

    void Start()
    {
        MM_Manager = GetComponent<MainMenu_Manager>();
        currentindex = 0;
        MM_Manager.selector.transform.position = positions[currentindex].position;
        playerIndices = new PlayerIndex[1];
        slidePositions = new Dictionary<int, Slider>();
        slidePositions.Add(0, generalSlider);
        slidePositions.Add(1, soundSlider);
        slidePositions.Add(2, musicSlider);
        slidePositions.Add(3, graphicsSlider);
        generalSlider.value = 0f;
        soundSlider.value = 0f;
        musicSlider.value = 0f;
        if (PlayerPrefs.HasKey("GeneralVolume"))
            generalSlider.value = PlayerPrefs.GetFloat("GeneralVolume");
        else
        {
            PlayerPrefs.SetFloat("GeneralVolume",0f);
            generalSlider.value = 0f;
            mixer.SetFloat("GeneralVolume", 0f);
        }
        if (PlayerPrefs.HasKey("SoundsVolume"))
            soundSlider.value = PlayerPrefs.GetFloat("SoundsVolume");
        else
        {
            PlayerPrefs.SetFloat("SoundsVolume", 0f);
            mixer.SetFloat("SoundsVolume", 0f);
            soundSlider.value = 0f;
        }
        if (PlayerPrefs.HasKey("MusicVolume"))
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", 0f);
            musicSlider.value = 0f;
            mixer.SetFloat("MusicVolume", 0f);
        }

    

        graphicsSlider.value = QualitySettings.GetQualityLevel();
        Time.timeScale = 1;
        _cooldown = true;
        optionsCanvasActive = false;
        controlsCanvasActive = false;
        GetGamepadInputs();
    }

    void Update()
    {
        GetGamepadInputs();
        //MM_Manager.selector.transform.Rotate(0, 0, 50 * Time.deltaTime);
        SelectionFeedback.Instance.SetKey(currentindex);
        if (optionsCanvasActive)
        {
            for (int i = 0; i < handle.Length; i++)
            {
                handle[i].sprite = handleDisable;
            }
            handle[currentindexInOptions].sprite = handleActive;
        }
    }

    void GetGamepadInputs()
    {
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 1; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    playerIndex = testPlayerIndex;
                    playerIndices[i] = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        foreach (var item in playerIndices)
        {
            prevState = state;
            state = GamePad.GetState(item);
            if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
            {
                AudioManager.Instance.CreateSound("NavigationHUD");
                Accept();
            }

            if ((prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed) || (prevState.Buttons.Back == ButtonState.Released && state.Buttons.Back == ButtonState.Pressed))
            {
                AudioManager.Instance.CreateSound("NavigationHUD");
                Back();
            }

            if (state.ThumbSticks.Left.Y != 0 && _cooldown)
            {
                if (!optionsCanvasActive && !controlsCanvasActive)
                {
                    //AudioManager.Instance.CreateSound("NavigationHUD");
                    Move(state.ThumbSticks.Left.Y == 0 ? 0 : state.ThumbSticks.Left.Y > 0 ? -1 : 1);
                    StartCoroutine(CoolDown());
                }
                else if (optionsCanvasActive)
                {
                    //AudioManager.Instance.CreateSound("NavigationHUD");
                    MoveOnOptions(state.ThumbSticks.Left.Y == 0 ? 0 : state.ThumbSticks.Left.Y > 0 ? -1 : 1);
                    StartCoroutine(CoolDown());
                }
            }

            if (state.ThumbSticks.Left.X != 0 && _cooldown)
            {
                if (optionsCanvasActive)
                {
                    //AudioManager.Instance.CreateSound("NavigationHUD");
                    SlideOptions(state.ThumbSticks.Left.X == 0 ? 0 : state.ThumbSticks.Left.X < 0 ? -1 : 1);
                    StartCoroutine(CoolDown());
                }
            }
        }
    }

    void Accept()
    {
        if (currentindex == 0)
        {
            PlayGame();
        }
        else if (currentindex == 1)
        {
            ShowOptions(true);
        }
        else if (currentindex == 2)
        {
            ShowControls(true);
        }
        else if (currentindex == 3)
        {
            QuitGame();
        }
    }

    void Back()
    {
        if (optionsCanvasActive)
        {
            ShowOptions(false);
        }
        else if (controlsCanvasActive)
        {
            ShowControls(false);
        }
    }

    void MoveOnOptions(int x)
    {
        currentindexInOptions += x;
        if (currentindexInOptions > 3)
        {
            currentindexInOptions = 3;
        }
        else if (currentindexInOptions < 0)
        {
            currentindexInOptions = 0;
        }
        else
        {
            AudioManager.Instance.CreateSound("NavigationHUD");
        }
    }

    void SlideOptions(int x)
    {
        currentindexInSlider = slidePositions[currentindexInOptions].value;
        if (currentindexInOptions == slidePositions.Count - 1)
        {
            currentindexInSlider += x;
            if (currentindexInSlider < 0)
                currentindexInSlider = 0;
            if (currentindexInSlider >= 2)
                currentindexInSlider = 2;
            slidePositions[currentindexInOptions].value = currentindexInSlider;
            QualitySettings.SetQualityLevel((int)currentindexInSlider);
            AudioManager.Instance.CreateSound("NavigationHUD");
        }
        else
        {
            currentindexInSlider += x * 5;
            if (currentindexInSlider < -80)
                currentindexInSlider = -80;
            if (currentindexInSlider >= 0)
                currentindexInSlider = 0;
            slidePositions[currentindexInOptions].value = currentindexInSlider;
            if(currentindexInOptions == 0)
            {
                mixer.SetFloat("GeneralVolume", currentindexInSlider);
                PlayerPrefs.SetFloat("GeneralVolume", currentindexInSlider);
            }
            else if (currentindexInOptions == 1)
            {
                mixer.SetFloat("SoundsVolume", currentindexInSlider);
                PlayerPrefs.SetFloat("SoundsVolume", currentindexInSlider);
            }
            else
            {
                mixer.SetFloat("MusicVolume", currentindexInSlider);
                PlayerPrefs.SetFloat("MusicVolume", currentindexInSlider);
            }
            AudioManager.Instance.CreateSound("NavigationHUD");
        }
    }

    void Move(int x)
    {
        var tempIndex = currentindex;
        currentindex += x;
        if (currentindex == positions.Count)
        {
            currentindex = positions.Count - 1;
        }
        else if (currentindex < 0)
        {
            currentindex = 0;
        }
        if (currentindex != tempIndex)
        {
            MM_Manager.selector.transform.position = positions[currentindex].position;
            MM_Manager.selector.transform.rotation = positions[currentindex].rotation;
            AudioManager.Instance.CreateSound("NavigationHUD");
        }
    }

    IEnumerator CoolDown()
    {
        _cooldown = false;
        yield return new WaitForSeconds(0.2f);
        _cooldown = true;
    }

    void GoDown()
    {
        if (currentindex < 3)
        {
            currentindex++;
            //MM_Manager.selector.transform.position = Camera.main.WorldToScreenPoint(positions[currentindex].position);
            MM_Manager.selector.transform.position = positions[currentindex].position;
            MM_Manager.selector.transform.rotation = positions[currentindex].rotation;

        }
    }
    void GoUp()
    {
        if (currentindex > 0)
        {
            currentindex--;
            //MM_Manager.selector.transform.position = Camera.main.WorldToScreenPoint(positions[currentindex].position);
            MM_Manager.selector.transform.position = positions[currentindex].position;
            MM_Manager.selector.transform.rotation = positions[currentindex].rotation;
        }
    }

    public void ShowControls(bool state)
    {
        if (state)
        {
            controlsCanvas.SetActive(true);
            controlsCanvasActive = true;
        }
        else
        {
            controlsCanvas.SetActive(false);
            controlsCanvasActive = false;

        }
    }

    public void ShowOptions(bool state)
    {
        if (state)
        {
            optionsCanvas.SetActive(true);
            optionsCanvasActive = true;
        }
        else
        {
            optionsCanvas.SetActive(false);
            optionsCanvasActive = false;

        }
    }

    void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
