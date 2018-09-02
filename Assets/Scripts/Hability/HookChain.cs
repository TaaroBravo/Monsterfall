using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookChain : MonoBehaviour
{

    public Hook _hook;

    ParticleSystem _ps;
    ParticleSystem.Particle[] _particles;

    bool returning;
    int _numParticlesAlive;

    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _hook.OnFailedFire += () => ReturnHook();
        _hook.OnHookTarget += x => ReturnHook();
        _hook.OnFireHook += () => FireHook();
        _particles = new ParticleSystem.Particle[_ps.particleCount];
    }

    private void Update()
    {
        transform.up = -_hook.transform.up;
        ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();
        renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;

        //if(returning)
        //{
        //    _numParticlesAlive = _ps.GetParticles(_particles);
        //    _ps.SetParticles(_particles, _numParticlesAlive - 1);

        //    if (_numParticlesAlive == 0)
        //    {
        //        _ps.SetParticles(_particles, 0);
        //        returning = false;
        //        _ps.Stop();
        //    }
        //}
    }

    IEnumerator DeleteParticles()
    {
        while(true)
        {
            _numParticlesAlive = _ps.GetParticles(_particles);
            _ps.SetParticles(_particles, _numParticlesAlive - 1);

            if (_numParticlesAlive <= 0)
            {
                _ps.SetParticles(_particles, 0);
                returning = false;
                _ps.Stop();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void FireHook()
    {
        _ps.Play();
    }

    void ReturnHook()
    {
        _particles = new ParticleSystem.Particle[_ps.main.maxParticles];
        returning = true;
        StartCoroutine(DeleteParticles());
    }
}
