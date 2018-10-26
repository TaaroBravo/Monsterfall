using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalRays : MonoBehaviour {

    public GameObject cristal;

    public GameObject rays;

    public float activeTime;
    public float delayTime;

    bool _enabled;

	void Start ()
    {
        StartCoroutine(CristalControl(activeTime, delayTime));
	}

    private void Update()
    {
        if(enabled)
        {
            cristal.SetActive(true);
            rays.SetActive(true);
            rays.transform.Rotate(0, 0, -90 * Time.deltaTime);
        }
    }

    IEnumerator CristalControl(float active, float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            EnableCristal();
            yield return new WaitForSeconds(active);
            DisableCristal();
        }
    }

    void EnableCristal()
    {
        enabled = true;
    }

    void DisableCristal()
    {
        enabled = false;
        cristal.SetActive(false);
        rays.SetActive(false);
    }
}
