using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookChain : MonoBehaviour
{

    public Hook hook;

    private void Update()
    {
        transform.up = -hook.transform.up;
        ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();
        renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
    }
}
