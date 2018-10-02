using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{

    [SerializeField]
    private string sceneToLoad;

    [SerializeField]
    private Text percentText;

    [SerializeField]
    private Image progressImage;

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation loading;
        loading = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);

        loading.allowSceneActivation = false;
        while (loading.progress < 0.9f)
        {

            percentText.text = string.Format("{0}%", Mathf.RoundToInt(loading.progress * 100));

            progressImage.fillAmount = loading.progress;

            yield return null;
        }
        percentText.text = "100%";
        progressImage.fillAmount = 1;
        loading.allowSceneActivation = true;
    }

}
