using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightFeedbackController : MonoBehaviour {

    public ParticleSystem p1;
    public ParticleSystem fireEstela;

    public void PlayLanding()
    {
        p1.Play();
    }
}
