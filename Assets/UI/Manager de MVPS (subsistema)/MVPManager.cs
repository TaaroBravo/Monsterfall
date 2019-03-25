using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MVPManager : MonoBehaviour
{

    public static MVPManager Instance { get; private set; }

    public List<Image> Heroes = new List<Image>(); // 0 - knight, 1 - pirate, 2 - rogue, 3 - berserker
    public Image Motive;
    public List<Sprite> Motives = new List<Sprite>();
    // 0 - 2 azul, 0 - most damage, 1 - 2 kills, 2 - 3 kills
    // 3 - 5 rojo, 3 - most damage, 4 - 2 kills, 5 - 3 kills
    // 6 - 8 verde, 6 - most damage, 7 - 2 kills, 8 - 3 kills
    // 9 - 11 amarillo, 9 - most damage, 10 - 2 kills, 11 - 3 kills

    void Awake()
    {
        Instance = this;
    }

    public void ShowStand(int id, int hero, int motive) // Motive: 0 - most damage, 1 - 2kills, 2 - 3kills
    {
        for (int i = 0; i < Heroes.Count; i++) Heroes[i].gameObject.SetActive(false);
        Heroes[hero].gameObject.SetActive(true);
        Motive.sprite = Motives[id * 3 + motive];
    }
    //void DebugKeys()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q)) ShowStand(Random.Range(0, 4), Random.Range(0, 4), Random.Range(0, 3));
    //}
}
