using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectorMainManager : MonoBehaviour {

	void Update ()
    {
        var infoPlayers = FindObjectsOfType<PlayersInfoManager>().Count();
        if (infoPlayers > 1)
            Destroy(FindObjectsOfType<PlayersInfoManager>().Skip(1).First().gameObject);
	}
}
