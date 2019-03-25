using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeHUDAppear : MonoBehaviour {
    //public List<GameObject> RHUD = new List<GameObject>();
    //public List<GameObject> BHUD = new List<GameObject>();
    //public List<GameObject> YHUD = new List<GameObject>();
    //public List<GameObject> GHUD = new List<GameObject>();
    public List<Image> _RImg = new List<Image>();
    public List<Image> _BImg = new List<Image>();
    public List<Image> _YImg = new List<Image>();
    public List<Image> _GImg = new List<Image>();
    float _fadetimer;
    float _fadetimer2;
    public float timetofade;
    public float delaytofade;
    public float timetofade2;
    public float delaytofade2;

    // Use this for initialization
    void Start () {
        //_RImg = RHUD.GetComponent<Image>();
        //_BImg = BHUD.GetComponent<Image>();
        //_YImg = YHUD.GetComponent<Image>();
        //_GImg = GHUD.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_fadetimer - delaytofade <= timetofade)
        {
            _fadetimer += Time.deltaTime;
            for (int i = 0; i < _RImg.Count; i++)
            {
                Color sarasa = new Color(_RImg[i].color.r, _RImg[i].color.g, _RImg[i].color.b, Mathf.Abs((Mathf.Clamp(_fadetimer - delaytofade, 0, Mathf.Infinity) / timetofade)/* - 1*/));
                _RImg[i].color = sarasa;
            }
            for (int i = 0; i < _BImg.Count; i++)
            {
                Color sarasa = new Color(_BImg[i].color.r, _BImg[i].color.g, _BImg[i].color.b, Mathf.Abs((Mathf.Clamp(_fadetimer - delaytofade, 0, Mathf.Infinity) / timetofade)/* - 1*/));
                _BImg[i].color = sarasa;
            }
        }
        if (_fadetimer2 - delaytofade2 <= timetofade2)
        {
            _fadetimer2 += Time.deltaTime;
            for (int i = 0; i < _YImg.Count; i++)
            {
                Color sarasa = new Color(_YImg[i].color.r, _YImg[i].color.g, _YImg[i].color.b, Mathf.Abs((Mathf.Clamp(_fadetimer2 - delaytofade2, 0, Mathf.Infinity) / timetofade2)/* - 1*/));
                _YImg[i].color = sarasa;
            }
            for (int i = 0; i < _GImg.Count; i++)
            {
                Color sarasa = new Color(_GImg[i].color.r, _GImg[i].color.g, _GImg[i].color.b, Mathf.Abs((Mathf.Clamp(_fadetimer2 - delaytofade2, 0, Mathf.Infinity) / timetofade2)/* - 1*/));
                _GImg[i].color = sarasa;
            }
        }
    }
}
