using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarpController : MonoBehaviour
{

    public event Action OnTeleportPlayer = delegate { };

    public WarpController parentWarp;
    public Transform zoneToRespawn;
    public Transform zoneToTeleportHook;
    public Transform zoneToReturnHook;

    public void WarpWithParent(Transform pl)
    {
        pl.transform.position = parentWarp.zoneToRespawn.position;
        OnTeleportPlayer();
        StartCoroutine(StopParticles(pl));
        if (pl.GetComponent<PlayerController>() && pl.GetComponent<PlayerController>() is Knight)
            StartCoroutine(StopParticlesKnight(pl));
    }

    IEnumerator StopParticles(Transform pl)
    {
        while (true)
        {
            if (pl.GetComponent<ParticlePuños>().puñoderecho)
            {
                pl.GetComponent<ParticlePuños>().puñoderecho.gameObject.SetActive(false);
                pl.GetComponent<ParticlePuños>().puñoizquierdo.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.25f);
            if (pl.GetComponent<ParticlePuños>().puñoderecho)
            {
                pl.GetComponent<ParticlePuños>().puñoderecho.gameObject.SetActive(true);
                pl.GetComponent<ParticlePuños>().puñoizquierdo.gameObject.SetActive(true);
            }
            break;
        }
    }

    IEnumerator StopParticlesKnight(Transform pl)
    {
        while (true)
        {
            pl.GetComponent<KnightFeedbackController>().punchFire1.gameObject.SetActive(false);
            pl.GetComponent<KnightFeedbackController>().punchFire2.gameObject.SetActive(false);
            //pl.GetComponent<KnightFeedbackController>().childFireEstela.gameObject.SetActive(false);
            pl.GetComponent<KnightFeedbackController>().childFireEstela.Stop();
            //pl.GetComponent<KnightFeedbackController>().fireEstela.gameObject.SetActive(false);
            pl.GetComponent<KnightFeedbackController>().fireEstela.Stop();
            yield return new WaitForSeconds(0.25f);
            pl.GetComponent<KnightFeedbackController>().punchFire1.Play();
            pl.GetComponent<KnightFeedbackController>().punchFire2.Play();
            pl.GetComponent<KnightFeedbackController>().punchFire1.gameObject.SetActive(true);
            pl.GetComponent<KnightFeedbackController>().punchFire2.gameObject.SetActive(true);
            //pl.GetComponent<KnightFeedbackController>().childFireEstela.gameObject.SetActive(true);
            //pl.GetComponent<KnightFeedbackController>().fireEstela.gameObject.SetActive(true);
            break;
        }
    }

    public void WarpHook(Transform hook)
    {
        hook.transform.position = parentWarp.zoneToTeleportHook.position;
    }

}
