using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayersManager : MonoBehaviour
{
    bool alreadyFinish;
    bool gameStarted;
    public List<PlayerController> myPlayers = new List<PlayerController>();

    private void Awake()
    {
        GameManager.Instance.OnSpawnCharacters += x => SetPlayers(x);
    }

    void Update()
    {
        if (!myPlayers.Any() || !gameStarted)
            return;
        var alivePlayers = myPlayers.Where(x => x != null && x.GetComponent<PlayerController>());
        if (alivePlayers.Count() <= 1)
        {
            if (alivePlayers.Any())
                alivePlayers.First().myAnim.Play("Victory");
            if (!alreadyFinish)
                StartCoroutine(WaitToFinish());
        }
    }

    void SetPlayers(List<PlayerController> heroes)
    {
        myPlayers = heroes;
        gameStarted = true;
    }

    IEnumerator WaitToFinish()
    {
        alreadyFinish = true;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.FinishGame();
    }
}
