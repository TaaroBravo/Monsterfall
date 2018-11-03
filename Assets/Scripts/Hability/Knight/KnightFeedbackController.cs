using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightFeedbackController : MonoBehaviour {

    public ParticleSystem p1;
    public ParticleSystem fireEstela;

    public ParticleSystem punchFire1;
    public ParticleSystem punchFire2;

    public void PlayLanding()
    {
        p1.Play();
    }
}
