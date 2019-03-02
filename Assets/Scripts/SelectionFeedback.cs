using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionFeedback : MonoBehaviour {

    public static SelectionFeedback Instance { get; private set; }

    int currentIndex;
    Dictionary<int, List<Image>> feedbackDic;
    public List<Image> play;
    public List<Image> options;
    public List<Image> controls;
    public List<Image> exit;

    public Color on;
    public Color off;
	
	void Awake ()
    {
        Instance = this;
        feedbackDic = new Dictionary<int, List<Image>>();
        feedbackDic.Add(0, play);
        feedbackDic.Add(1, options);
        feedbackDic.Add(2, controls);
        feedbackDic.Add(3, exit);
        StartCoroutine(StartChanges());
	}

    private void Update()
    {
        foreach (var item in feedbackDic)
        {
            if (item.Key != currentIndex)
                foreach (var x in item.Value)
                    x.color = on;
        }
    }

    public void SetKey(int i)
    {
        currentIndex = i;
    }

    IEnumerator StartChanges()
    {
        while (true)
        {
            yield return RepeatLerp(on, off, 1);
            yield return RepeatLerp(off, on, 1);
        }
    }

    public IEnumerator RepeatLerp(Color a, Color b, float time)
    {
        float i = 0f;
        float rate = (1f / time) * 2;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            foreach (var item in feedbackDic[currentIndex])
                item.color = Vector4.Lerp(a, b, i);
            yield return null;
        }
    }
}
