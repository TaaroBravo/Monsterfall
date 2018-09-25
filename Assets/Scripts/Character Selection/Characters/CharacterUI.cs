using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CharacterUI : MonoBehaviour
{
    public delegate void CharacterState();
    public CharacterState State;

    public Image randomCharacter;
    public List<Image> characters = new List<Image>();
    int characterIndex;

    public Transform pressStart;
    public Transform pressPos;
    public Transform selectingPos;

    public GameObject readyLight;

    float lerpValueIn;
    float lerpValueOut;

    bool random;

    private void Start()
    {
        State = StartState;
    }

    private void Update()
    {
        State();
    }

    public void StartState()
    {
        if (lerpValueIn >= 1)
        {
            lerpValueOut = 0;
            pressStart.position = pressPos.position;
        }
        else
        {
            lerpValueIn += Time.deltaTime * 2;
            pressStart.position = Vector3.Lerp(pressStart.position, pressPos.position, lerpValueIn);
        }
        foreach (var _char in characters)
            _char.enabled = false;
        randomCharacter.enabled = false;
        readyLight.SetActive(false);
    }

    public void SelectingState()
    {
        if (lerpValueOut >= 1)
        {
            lerpValueIn = 0;
            pressStart.position = selectingPos.position;
        }
        else
        {
            lerpValueOut += Time.deltaTime * 2;
            pressStart.position = Vector3.Lerp(pressStart.position, selectingPos.position, lerpValueOut);
        }
        if (!random)
        {
            randomCharacter.enabled = false;
            for (int i = 0; i < characters.Count; i++)
            {
                if (i == characterIndex)
                    characters[characterIndex].enabled = true;
                else
                    characters[i].enabled = false;
            }
        }
        else
        {
            randomCharacter.enabled = true;
            foreach (var _char in characters)
                _char.enabled = false;
        }
        readyLight.SetActive(false);
    }

    public void ReadyState()
    {
        readyLight.SetActive(true);
    }

    public void SetCharacterIndex(int index)
    {
        characterIndex = index;
        random = false;
    }

    public void SetCharacterRandom()
    {
        random = true;
    }

}
