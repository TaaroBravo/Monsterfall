using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightFeedbackController : MonoBehaviour {

    public ParticleSystem p1;
    public ParticleSystem fireEstela;
    public ParticleSystem childFireEstela;

    ParticleSystem.TriggerModule ps_Trigger;
    ParticleSystem.TriggerModule ps_TriggerChild;

    public ParticleSystem punchFire1;
    public ParticleSystem punchFire2;

    int count;

    private void Start()
    {
        ps_Trigger = fireEstela.trigger;
        ps_TriggerChild = childFireEstela.trigger;
        foreach (var player in GameManager.Instance.myPlayers)
        {
            if(!(player is Knight))
            {
                ps_Trigger.SetCollider(count, player.GetComponent<Collider>());
                ps_TriggerChild.SetCollider(count, player.GetComponent<Collider>());
                count++;
            }
        }
    }

    public void PlayLanding()
    {
        p1.Play();
    }

    void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        // particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

        // get
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, enter);

        // iterate
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            foreach (var item in Physics.OverlapSphere(p.position, 4))
            {
                Debug.Log("entre maso");
                if(item.GetComponent<PlayerController>())
                {
                    if (!item.GetComponent<Knight>())
                        item.GetComponent<PlayerController>().StartCoroutine(item.GetComponent<PlayerController>().BurnEstela());
                }
            }
            
        }
    }
}
