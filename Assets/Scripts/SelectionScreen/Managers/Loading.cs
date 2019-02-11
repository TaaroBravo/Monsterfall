using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{

    public static Loading Instance { get; private set; }

    [SerializeField]
    private string sceneToLoad;

    public GameObject HUD;
    public GameObject loadScreen;
    public Transform loadingBarA;
    public Transform loadingBarB;
    public Transform finalPosBarA;
    public Transform finalPosBarB;
    public Image loadingText;

    public Transform[] conditions;
    public Transform[] conditionsFinalPos;

    bool active;
    bool loading;

    float lerpValueIn;

    public Image[] dots;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(loading)
        {
            if (lerpValueIn >= 1)
            {
                loadingBarA.position = finalPosBarA.position;
                loadingBarB.position = finalPosBarB.position;
                OnLoadingTranscition();
                loading = false;
                for (int i = 0; i < conditions.Length; i++)
                    conditions[i].position = conditionsFinalPos[i].position;
            }
            else
            {
                lerpValueIn += Time.deltaTime * 2;
                loadingBarA.position = Vector3.Lerp(loadingBarA.position, finalPosBarA.position, lerpValueIn);
                loadingBarB.position = Vector3.Lerp(loadingBarB.position, finalPosBarB.position, lerpValueIn);
                for (int i = 0; i < conditions.Length; i++)
                    conditions[i].position = Vector3.Lerp(conditions[i].position, conditionsFinalPos[i].position, lerpValueIn);
            }
        }
    }

    public void ChangeScene()
    {
        if(!active)
        {
            HUD.SetActive(false);
            loadScreen.SetActive(true);
            active = true;
            loading = true;
            //StartCoroutine(LoadScene());
        }
    }

    void OnLoadingTranscition()
    {
        StartCoroutine(Dots());
        loadingText.enabled = true;
        StartCoroutine(LoadScene());
    }

    IEnumerator Dots()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.3f);
            dots[0].enabled = true;
            yield return new WaitForSeconds(0.3f);
            dots[1].enabled = true;
            yield return new WaitForSeconds(0.3f);
            dots[2].enabled = true;
            yield return new WaitForSeconds(0.3f);
            foreach (var item in dots)
                item.enabled = false;
        }
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation loading;
        loading = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);

        loading.allowSceneActivation = false;
        yield return new WaitUntil(() => loading.progress >= 0.9f);
        //while (loading.progress < 0.9f)
        //{

        //    percentText.text = string.Format("{0}%", Mathf.RoundToInt(loading.progress * 100));

        //    progressImage.fillAmount = loading.progress;

        //    yield return null;
        //}
        //progressImage.fillAmount = 1;
        loading.allowSceneActivation = true;
    }

}
