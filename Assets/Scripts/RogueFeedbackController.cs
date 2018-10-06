using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueFeedbackController : MonoBehaviour {
    public ParticleSystem p1;
    public void DashRogue()
    {
        p1.Play();
    }
}
