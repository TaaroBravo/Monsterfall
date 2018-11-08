using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightFeedbackController : MonoBehaviour {

    public ParticleSystem p1;
    public ParticleSystem fireEstela;
    public ParticleSystem childFireEstela;

    public ParticleSystem punchFire1;
    public ParticleSystem punchFire2;

    int count;

    private void Start()
    {
        fireEstela.playOnAwake = false;
    }

    public void PlayLanding()
    {
        p1.Play();
    }
}
