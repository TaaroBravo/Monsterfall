using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPoints : MonoBehaviour
{
    public bool isAvailable;
    SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isAvailable)
            _sr.enabled = true;
        else
            _sr.enabled = false;
    }
}
