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

    public Image pedestal;

    public Image sphere;
    public Sprite onSphere;
    public Sprite offSphere;

    public Image characterBar;
    public Sprite readyBar;
    public Sprite selectBar;

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
        readyLight.transform.Rotate(0, 0, 1);
    }

    public void StartState()
    {
        pedestal.gameObject.SetActive(false);
        sphere.gameObject.SetActive(false);
        characterBar.gameObject.SetActive(false);
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
        characterBar.sprite = selectBar;
        sphere.sprite = offSphere;
        readyLight.SetActive(false);
    }

    public void SelectingState()
    {
        pedestal.gameObject.SetActive(true);
        sphere.gameObject.SetActive(true);
        characterBar.gameObject.SetActive(true);
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
        sphere.sprite = offSphere;
        characterBar.sprite = selectBar;
        readyLight.SetActive(false);
    }

    public void ReadyState()
    {
        readyLight.SetActive(true);
        sphere.sprite = onSphere;
        characterBar.sprite = readyBar;
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
