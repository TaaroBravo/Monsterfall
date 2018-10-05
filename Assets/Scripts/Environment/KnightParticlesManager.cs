using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightParticlesManager : MonoBehaviour {

    public GameObject dashPs;
    public Transform dashSpawnPos;
     
    public GameObject skillPs;
    public Transform skillSpawnPos;
    
    public void DisplayDashParticle()
    {
        var ps = Instantiate(dashPs, dashSpawnPos.position, Quaternion.identity);
        Destroy(ps, 3);
    }

    public void DisplaySkillParticle()
    {
        var ps = Instantiate(skillPs, skillSpawnPos.position, Quaternion.identity);
        Destroy(ps, 5);
    }



}
