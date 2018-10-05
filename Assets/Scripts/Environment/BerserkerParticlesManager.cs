using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerParticlesManager : MonoBehaviour {

    public GameObject berserkerSkill;
    public Transform berserkerFoot;

    public GameObject berserkerDash;
    public Transform berserkerBack;

    GameObject currentDashPS;
    

    public void DisplayBerserkerSkill()
    {
        Debug.Log("Skill");
        var ps = GameObject.Instantiate(berserkerSkill,berserkerFoot.position,Quaternion.identity);
        ps.transform.SetParent(berserkerFoot, true);
        Destroy(ps,3);
    }

    public void DisplayBerserkerCharge()
    {
        Debug.Log("Charge");
        var ps = Instantiate(berserkerDash, berserkerBack.position, Quaternion.identity);

        ps.transform.SetParent(berserkerBack, true);
        currentDashPS = ps;
    }

    public void StopCharge()
    {
        Debug.Log("ChargeStop");
        Destroy(currentDashPS);
        currentDashPS = null;
    }


}
