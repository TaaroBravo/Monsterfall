using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookChain : MonoBehaviour
{

    public Hook _hook;

    public ParticleSystem _ps;

    public float distanceToEmit;
    private float _currentDistance;

    bool fired;
    bool returning;

    List<Vector3> _posEmitted = new List<Vector3>();

    ParticleSystem.Particle[] _particles;
    ParticleSystem.Particle _lastParticleEmmited = new ParticleSystem.Particle();


    void Start()
    {
        _hook = transform.parent.GetComponent<Hook>();
        //_ps = GetComponent<ParticleSystem>();
        _posEmitted = new List<Vector3>();
        _posEmitted.Add(transform.position);
        _hook.OnFailedFire += () =>
        {
            ReturningHook();
            ReturnedHook();
        };
        _hook.OnFireHook += () => OnFireHook();
        _particles = new ParticleSystem.Particle[_ps.particleCount];
    }

    private void LateUpdate()
    {
        transform.up = -_hook.transform.up;
        ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();
        renderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;


        //InitializeIfNeeded();

        if (fired)
        {
            if (!_ps.isPlaying)
                _ps.Play();
            gameObject.SetActive(true);
            //Mi distancia entre la última particula emitida y yo (el gancho)
            _currentDistance = (transform.position - _posEmitted[_posEmitted.Count - 1]).magnitude;
            if (returning)
            {
                if (_currentDistance >= distanceToEmit)
                {
                    _lastParticleEmmited.remainingLifetime = 0;
                    _particles = new ParticleSystem.Particle[_particles.Length - 1];
                    _posEmitted.RemoveAt(_posEmitted.Count - 1);
                }
            }
            else
            {
                //Si ya paso la distancia para poder emitir
                if (_currentDistance >= distanceToEmit)
                {
                    //Emití lpm.
                    _ps.Emit(1);
                    _particles = new ParticleSystem.Particle[_ps.main.maxParticles];
                    ParticleSystem.Particle p = _particles[_ps.particleCount - 1];
                    _particles[_particles.Length - 1] = p;
                    _lastParticleEmmited = _particles[_particles.Length - 1];
                    _posEmitted.Add(transform.position);
                }
            }
            _ps.SetParticles(_particles, _particles.Length);
        }
        else
            gameObject.SetActive(false);

    }

    void InitializeIfNeeded()
    {
        if (_ps == null)
            _ps = GetComponent<ParticleSystem>();

        if (_particles == null || _particles.Length < _ps.particleCount)
            _particles = new ParticleSystem.Particle[_ps.particleCount];
    }

    void OnFireHook()
    {

        fired = true;
        returning = false;
        _particles = new ParticleSystem.Particle[_ps.main.maxParticles];
    }

    void ReturningHook()
    {
        returning = true;
    }

    void ReturnedHook()
    {
        fired = false;
        returning = false;
        _particles = new ParticleSystem.Particle[_ps.main.maxParticles];
    }
}
