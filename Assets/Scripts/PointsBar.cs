using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsBar : MonoBehaviour {

    public Image pointsBar;
    public Image characterPortrait;

    public Color[] playersColors = new Color[0];
    public SkullManager sm;

    private void Awake()
    {
        SetupDictionary();
        sm = GetComponentInChildren<SkullManager>();
    }

    public enum Characters
    {
        Knight,
        Pirate,
        Rogue,
        Berserker
    }

    public Sprite[] characterSprites = new Sprite[0];

    Dictionary<Characters, Sprite> characterPortraitsDictionary = new Dictionary<Characters, Sprite>();
    Dictionary<int, Color> playerColorsDictionary = new Dictionary<int, Color>();


    void SetupDictionary()
    {
        characterPortraitsDictionary.Add(Characters.Pirate, characterSprites[(int)Characters.Pirate]);
        characterPortraitsDictionary.Add(Characters.Knight, characterSprites[(int)Characters.Knight]);
        characterPortraitsDictionary.Add(Characters.Berserker, characterSprites[(int)Characters.Berserker]);
        characterPortraitsDictionary.Add(Characters.Rogue, characterSprites[(int)Characters.Rogue]);

        for (int i = 0; i < 4; i++)
        {
            playerColorsDictionary.Add(i, playersColors[i]);
        }
    }

    public void SetCharacterSpriteAndColor(PlayerInfo plInfo)
    {
        pointsBar.color = playersColors[plInfo.player_number];
        characterPortrait.sprite = characterPortraitsDictionary[(Characters)plInfo.characterChosen];
    }
}