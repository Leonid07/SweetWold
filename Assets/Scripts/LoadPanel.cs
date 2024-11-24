using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadPanel : MonoBehaviour
{
    public TMP_Text loadingText;
    private string baseText = "LOADING";
    private int dotCount = 0;
    public float loadingDuration = 5f;

    public CanvasGroup loadingPanelCanvasGroup;
    private float alphaIncreaseDuration = 1.5f;

    private void Start()
    {
        StartCoroutine(AnimateLoadingText());
        StartCoroutine(IncreaseAlpha());
    }

    private IEnumerator AnimateLoadingText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadingDuration)
        {
            dotCount = (dotCount + 1) % 4;
            loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f);
            elapsedTime += 0.5f;
        }
        gameObject.SetActive(false);
    }
    private IEnumerator IncreaseAlpha()
    {
        float targetAlpha = 1f;
        float startAlpha = loadingPanelCanvasGroup.alpha;
        float elapsedTime = 0f;

        yield return new WaitForSeconds(loadingDuration * 0.3f);

        while (elapsedTime < alphaIncreaseDuration)
        {
            elapsedTime += Time.deltaTime;
            loadingPanelCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / alphaIncreaseDuration);
            yield return null;
        }
        loadingPanelCanvasGroup.alpha = targetAlpha;
    }
}
