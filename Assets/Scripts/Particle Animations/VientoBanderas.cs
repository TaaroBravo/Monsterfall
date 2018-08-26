using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VientoBanderas : MonoBehaviour {

    public List<GameObject> Banderas = new List<GameObject>();
    List<Cloth> ClothComponentBanderas = new List<Cloth>();
    public float DerAceleration;
    public float IzqAceleration;
    public float TempestAceleration;
    public float ChangeDirTimerMax;
    public float ChangeDirTimerMin;
    public float TempestChangeDirTimer;
    float timer;
    float ChangeTimer;
    bool TempestBool;
    bool ChangeValues;

	void Start () {
        for (int i = 0; i < Banderas.Count; i++)
            ClothComponentBanderas.Add(Banderas[i].GetComponent<Cloth>());
        ChangeTimer = Random.Range(ChangeDirTimerMin, ChangeDirTimerMax);
	}

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > ChangeTimer && !TempestBool || timer > TempestChangeDirTimer && TempestBool)
        {
            timer = 0;
            ChangeValues = false;
        }
        if (!ChangeValues)
        {
            ChangeTimer = Random.Range(ChangeDirTimerMin, ChangeDirTimerMax);
            var temp = Random.Range(IzqAceleration, DerAceleration);
            if (TempestBool)
            {
                for (int i = 0; i < ClothComponentBanderas.Count; i++)
                    ClothComponentBanderas[i].externalAcceleration = new Vector3(temp* TempestAceleration, -10, 0);
            }
            else
            {
                for (int i = 0; i < ClothComponentBanderas.Count; i++)
                    ClothComponentBanderas[i].externalAcceleration = new Vector3(temp, -10, 0);
            }
            ChangeValues = true;
        }
    }
    public void ChangeState()
    {
        TempestBool = TempestBool ? false : true;
        ChangeValues = false;
    }
}
