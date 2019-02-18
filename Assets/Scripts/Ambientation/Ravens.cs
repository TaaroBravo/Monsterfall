using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ravens : MonoBehaviour {

    AudioSource _audio;
	void Start () {
        _audio = GetComponent<AudioSource>();
        StartCoroutine(PlayAudio());
    }
	
	IEnumerator PlayAudio()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 15f));
            _audio.Play();
        }
    }
}
