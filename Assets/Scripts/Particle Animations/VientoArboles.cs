using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VientoArboles : MonoBehaviour {

    public GameObject WindZoneParaArboles;
    public float TimeOffMax;
    public float TimeOffMin;
    public float DuracionMax;
    public float DuracionMin;
    public float IntensityMax;
    public float IntensityMin;

    VientoBanderas Banderas;
    PajarosManager Pajaros;
    ParalaxNubes Nubes;
    float restingIntensity;
    public bool EffectIsOn;
    float currentDuration;
    float currentTimeOff;
    float currentIntensity;
    float timer;

	void Start () {
        Banderas = GetComponent<VientoBanderas>();
        Pajaros = GetComponent<PajarosManager>();
        Nubes = GetComponent<ParalaxNubes>();
        restingIntensity = WindZoneParaArboles.GetComponent<WindZone>().windMain;
        ResetAndRandomizeValues();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (EffectIsOn)
        {
            WindZoneParaArboles.GetComponent<WindZone>().windMain = currentIntensity;
            if (timer > currentDuration)
            {
                ResetAndRandomizeValues();
                ChangeState();
            }
        }
        else
        {
            WindZoneParaArboles.GetComponent<WindZone>().windMain = restingIntensity;
            if (timer > currentTimeOff)
            {
                ResetAndRandomizeValues();
                ChangeState();
            }

        }
    }
    void ResetAndRandomizeValues()
    {
        timer = 0;
        currentTimeOff = Random.Range(TimeOffMin, TimeOffMax);
        currentDuration = Random.Range(DuracionMin, DuracionMax);
        currentIntensity = Random.Range(IntensityMin, IntensityMax);
    }
    void ChangeState()
    {
        EffectIsOn = EffectIsOn ? false : true;
        Nubes.ChangeState();
        Pajaros.ChangeState();
        Banderas.ChangeState();
    }
}
