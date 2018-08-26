using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajarosManager : MonoBehaviour {

    public List<ParticleSystem> Pajaros = new List<ParticleSystem>();
    public float timetoreactivate;
    public int ammountofbirds;
    public float TempestTimetoreactivate;
    public int TempestAmmountofbirds;

    float timer;
    bool TempestBool;

    void Start()
    {
        for (int i = 0; i < Pajaros.Count; i++) Pajaros[i].Play();
    }
    void Update () {
        timer += Time.deltaTime;
        if (TempestBool)
        {
            if (timer > TempestTimetoreactivate)
            {
                timer = 0;
                List<ParticleSystem> temp = new List<ParticleSystem>();
                for (int i = 0; i < TempestAmmountofbirds; i++)
                {
                    ParticleSystem temp2 = Pajaros[Random.Range(0, Pajaros.Count)];
                    if (!temp.Contains(temp2)) temp.Add(temp2);
                }
                for (int i = 0; i < Pajaros.Count; i++)
                    if (temp.Contains(Pajaros[i]) && !Pajaros[i].isPlaying) Pajaros[i].Play();
            }
        }
        else
        {
            if (timer > timetoreactivate)
            {
                timer = 0;
                List<ParticleSystem> temp = new List<ParticleSystem>();
                for (int i = 0; i < ammountofbirds; i++)
                {
                    ParticleSystem temp2 = Pajaros[Random.Range(0, Pajaros.Count)];
                    if (!temp.Contains(temp2)) temp.Add(temp2);
                }
                for (int i = 0; i < Pajaros.Count; i++)
                    if (temp.Contains(Pajaros[i]) && !Pajaros[i].isPlaying) Pajaros[i].Play();
            }
        }
            
	}
    public void ChangeState()
    {
        TempestBool = TempestBool ? false : true;
        timer = TempestBool ? -3 : 0;
    }
}
