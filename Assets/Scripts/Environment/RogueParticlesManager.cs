using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueParticlesManager : MonoBehaviour {

    public GameObject skillPs;
    public Transform skillSpawnPosition;

    public GameObject dashPs;
    public Transform dashSpawnPos;
    
    public void DisplayRogueSkill()
    {
        var ps = Instantiate(skillPs, skillSpawnPosition.position, Quaternion.identity);
        Destroy(ps, 5);
    }

    public void DisplayRogueDash()
    {
        var ps = Instantiate(dashPs, dashSpawnPos.position, Quaternion.identity);
        Destroy(ps, 5);
    }
}
