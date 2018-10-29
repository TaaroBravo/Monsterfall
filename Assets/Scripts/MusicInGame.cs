using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicInGame : MonoBehaviour
{

    private AudioSource source;
    public AudioClip inGame;
    public AudioClip inSelection;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level")
        {
            source.volume = 0.4f;
            source.clip = inGame;
            if (!source.isPlaying)
                source.Play();
        }
        else
        {
            source.volume = 0.4f;
            source.clip = inSelection;
            if (!source.isPlaying)
                source.Play();
        }
    }
}
