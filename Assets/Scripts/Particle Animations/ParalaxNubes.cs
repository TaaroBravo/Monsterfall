using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxNubes : MonoBehaviour {

    public List<GameObject> nubesFrente = new List<GameObject>();
    public List<GameObject> nubesAtras = new List<GameObject>();
    public int Limite;
    public int PuntoDeReinicio;
    public float SpeedNubesFrente;
    public float TempestSpeedNubesFrente;
    public float SpeedNubesAtras;
    public float TempestSpeedNubesAtras;
    bool TempestBool;

    void Update()
    {
        for (int i = 0; i < 2; i++) // esta puesto 2 por que se que son 2 las nubes del frente y de atras
        {
            nubesFrente[i].GetComponent<RectTransform>().position +=
                TempestBool ? new Vector3(TempestSpeedNubesFrente, 0, 0) : new Vector3(SpeedNubesFrente, 0, 0);
            nubesAtras[i].GetComponent<RectTransform>().position +=
                TempestBool ? new Vector3(TempestSpeedNubesAtras, 0, 0) : new Vector3(SpeedNubesAtras, 0, 0);

            if (nubesFrente[i].GetComponent<RectTransform>().position.x > Limite)
                nubesFrente[i].GetComponent<RectTransform>().position =
                    new Vector3(
                        PuntoDeReinicio,
                        nubesFrente[i].GetComponent<RectTransform>().position.y,
                        nubesFrente[i].GetComponent<RectTransform>().position.z);
            if (nubesAtras[i].GetComponent<RectTransform>().position.x > Limite)
                nubesAtras[i].GetComponent<RectTransform>().position =
                    new Vector3(
                        PuntoDeReinicio,
                        nubesAtras[i].GetComponent<RectTransform>().position.y,
                        nubesAtras[i].GetComponent<RectTransform>().position.z);
        }
    }
    public void ChangeState()
    {
        TempestBool = TempestBool ? false : true;
    }
}
