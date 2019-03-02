using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Phantom : MonoBehaviour
{

    public ParticleSystem phantomPS;
    ParticleSystem.MainModule mainPhantom;
    public Text plusOne;

    public RectTransform _uiContainer;
    public RectTransform textPlusOne;
    public Vector3 offSet;
    Vector3 initialPos;
    Color on;
    Color off;

    private void Awake()
    {
        _uiContainer.SetParent(null);
        StartCoroutine(StartChanges());
    }

    private void Update()
    {
        textPlusOne.position = RectTransformUtility.WorldToScreenPoint(Camera.main, initialPos);
    }

    public void SetColorDeath(Color c, Vector3 pos)
    {
        mainPhantom = phantomPS.main;
        mainPhantom.startColor = c;
        initialPos = pos + Vector3.up * 5 + Vector3.right;
        transform.position = pos;
    }

    public void SetColorKiller(Color c)
    {
        plusOne.color = c;
        on = c;
        off = Vector4.zero;
    }

    IEnumerator StartChanges()
    {
        yield return new WaitForSeconds(0.5f);
        yield return RepeatLerp(on, off, 3f);
        Destroy(gameObject, 5);
    }

    public IEnumerator RepeatLerp(Color a, Color b, float time)
    {
        float i = 0f;
        float rate = (1f / time) * 1;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            if (i > 0.4f)
                plusOne.color = Color.Lerp(a, b, i);
            yield return null;
        }
    }
}
