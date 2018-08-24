using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajarosManager : MonoBehaviour {

    public List<ParticleSystem> Pajaros = new List<ParticleSystem>();
    float timer;
    public float timetoreactivate;
    public int ammountofbirds;
    void Start()
    {
        for (int i = 0; i < Pajaros.Count; i++) Pajaros[i].Play();
        //timer = timetoreactivate;
    }
    void Update () {
        timer += Time.deltaTime;
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
                //else Pajaros[i].Stop();
        }
	}
}
