using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counting : MonoBehaviour {

    public ScoreManager SM_Round;
    public List<Sprite> numbers = new List<Sprite>();
    public GameObject crystal;

    public GameObject text_round;
    public GameObject text_uno;
    public GameObject text_fight;
    public float maxValueTime;

    RectTransform _textOneValues;
    RectTransform _textRoundValues;
    RectTransform _textFightValues;
    float _timer;

    Vector3 _finalScaleUNO;
    Vector3 _finalScaleROUND;
    Vector3 _finalScaleFIGHT;
    Vector3 _cristalscale;

    Image _imgRound;
    Image _imgUno;
    Image _imgFight;

    float _fadetimer;
    float _fadetimerFight;
    public float timetofade;
    public float delaytofade;
    public float timetofadeFight;
    public float delaytofadeFight;

    void Start()
    {
        _textOneValues = text_uno.GetComponent<RectTransform>();
        _textRoundValues = text_round.GetComponent<RectTransform>();
        _textFightValues = text_fight.GetComponent<RectTransform>();
        _cristalscale = crystal.transform.localScale;

        _imgRound = text_round.GetComponent<Image>();
        _imgUno = text_uno.GetComponent<Image>();
        _imgFight = text_fight.GetComponent<Image>();

        _finalScaleUNO = _textOneValues.localScale;
        _finalScaleROUND = _textRoundValues.localScale;
        _finalScaleFIGHT = _textFightValues.localScale;
    }
    void Update()
    {
        if (SM_Round.round >= 8)
            _imgUno.sprite = numbers[numbers.Count - 1];
        else
            _imgUno.sprite = numbers[SM_Round.round];
        //_imgUno.sprite = SM_Round.round >= 9 ? numbers[SM_Round.round - 1] : numbers[numbers.Count-1]; // Update de sprite de numero 16_11_18
        _timer += Time.deltaTime;
        crystal.transform.localScale = _timer <= 2.8f ? 
            Vector3.Lerp(Vector3.zero, _cristalscale, (_timer - 0.6f) / 0.2f) :
            Vector3.Lerp(_cristalscale, Vector3.zero, (_timer - 2.8f) / 0.25f);
        _textOneValues.localScale = Vector3.Lerp(Vector3.zero, _finalScaleUNO, (_timer - 1f) / maxValueTime);
        _textRoundValues.localScale = Vector3.Lerp(Vector3.zero, _finalScaleROUND, (_timer - 0.6f) / maxValueTime);
        _textFightValues.localScale = Vector3.Lerp(Vector3.zero, _finalScaleFIGHT, (_timer - delaytofade - timetofade) / 0.05f);

        if (_fadetimer - delaytofade <= timetofade)
        {
            _fadetimer += Time.deltaTime;
            Color sarasa = new Color(1, 1, 1, Mathf.Abs((Mathf.Clamp(_fadetimer - delaytofade, 0, Mathf.Infinity) / timetofade) - 1));
            _imgRound.color = sarasa;
            _imgUno.color = sarasa;
        }
        else
        {
            Color sarasa = new Color(1, 1, 1, 0);
            _imgRound.color = sarasa;
            _imgUno.color = sarasa;
        }
        if (_fadetimerFight - delaytofadeFight <= timetofadeFight)
        {
            _fadetimerFight += Time.deltaTime;
            Color sarasa = new Color(1, 1, 1, Mathf.Abs((Mathf.Clamp(_fadetimerFight - delaytofadeFight, 0, Mathf.Infinity) / timetofadeFight) - 1));
            _imgFight.color = sarasa;
        }
        else
        {
            Color sarasa = new Color(1, 1, 1, 0);
            _imgFight.color = sarasa;
        }
    }
}
