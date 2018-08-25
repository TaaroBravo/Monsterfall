using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpController : MonoBehaviour {

    public WarpController parentWarp;
    public Transform zoneToRespawn;
    public Transform zoneToTeleportHook;
    public Transform zoneToReturnHook;

    public void WarpWithParent(Transform pl)
    {
        pl.transform.position = parentWarp.zoneToRespawn.position;
        StartCoroutine(StopParticles(pl));
    }

    IEnumerator StopParticles(Transform pl)
    {
        while (true)
        {
            pl.GetComponent<ParticlePuños>().puñoderecho.gameObject.SetActive(false);
            pl.GetComponent<ParticlePuños>().puñoizquierdo.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
            pl.GetComponent<ParticlePuños>().puñoderecho.gameObject.SetActive(true);
            pl.GetComponent<ParticlePuños>().puñoizquierdo.gameObject.SetActive(true);
            break;
        }

    }

    public void WarpHook(Transform hook)
    {
        hook.transform.position = parentWarp.zoneToTeleportHook.position;
    }

}
