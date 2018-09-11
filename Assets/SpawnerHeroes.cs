using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerHeroes : MonoBehaviour {

    public static SpawnerHeroes Instance { get; private set; }

    public Transform[] spawnPositions;


    public List<GameObject> knightPrefabs;
    public List<GameObject> piratePrefabs;
    public List<GameObject> roguePrefabs;
    List<List<GameObject>> heroes = new List<List<GameObject>>();

	void Awake()
    {
        Instance = this;
        heroes.Add(knightPrefabs);
        heroes.Add(piratePrefabs);
        heroes.Add(roguePrefabs);
	}

    public PlayerController SpawnHero(int hero_index, int player_number)
    {
        var hero = Instantiate(heroes[hero_index][player_number]);
        hero.transform.position = spawnPositions[player_number].position;
        return hero.GetComponent<PlayerController>();
    }
}
