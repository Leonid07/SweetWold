using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimUnlockLevel : MonoBehaviour
{
    [Header("Свечение")]
    public RectTransform lightPanel;
    public float rotationSpeed = 100f;

    [Header("Замок")]
    public RectTransform panel;
    public float rotationDuration = 1f;
    public Image panelImage;
    public Sprite newSprite;

    [Header("Свечение")]
    public RectTransform panelImageLight;
    public Vector2 targetSize;
    public float duration = 1f;
    public float fadeDuration = 1f;

    public void StartAnimUnlockLevel()
    {
        panelImageLight.sizeDelta = Vector2.zero;
        StartCoroutine(RotateCoroutine());
        StartCoroutine(RotatePanel());
        SoundManager.InstanceSound.soundLevelUnlock.Play();
    }

    IEnumerator RotateCoroutine()
    {
        while (true) // бесконечный цикл
        {
            lightPanel.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RotatePanel()
    {
        yield return RotateToAngle(40f, rotationDuration);
        yield return RotateToAngle(-40f, rotationDuration);
        yield return RotateToAngle(0f, rotationDuration);
        yield return StartCoroutine(AnimateResize(panelImageLight, targetSize, duration));
        ChangePanelSprite();
        yield return StartCoroutine(FadeOutAndDisableRaycast());
        gameObject.SetActive(false);
    }

    IEnumerator AnimateResize(RectTransform rectTransform, Vector2 endSize, float time)
    {
        Vector2 startSize = rectTransform.sizeDelta;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            rectTransform.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            yield return null;
        }
        rectTransform.sizeDelta = endSize;
    }

    IEnumerator RotateToAngle(float targetAngle, float duration)
    {
        float startAngle = panel.eulerAngles.z;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float zRotation = Mathf.Lerp(startAngle, targetAngle, elapsedTime / duration);
            panel.eulerAngles = new Vector3(0f, 0f, zRotation);
            yield return null;
        }

        panel.eulerAngles = new Vector3(0f, 0f, targetAngle);
    }

    void ChangePanelSprite()
    {
        if (panelImage != null && newSprite != null)
        {
            panelImage.sprite = newSprite;
        }
    }

    IEnumerator FadeOutAndDisableRaycast()
    {
        float elapsedTime = 0f;
        CanvasGroup lightPanelGroup = lightPanel.GetComponent<CanvasGroup>();
        if (lightPanelGroup == null)
        {
            lightPanelGroup = lightPanel.gameObject.AddComponent<CanvasGroup>();
        }
        CanvasGroup panelGroup = panel.GetComponent<CanvasGroup>();
        if (panelGroup == null)
        {
            panelGroup = panel.gameObject.AddComponent<CanvasGroup>();
        }
        CanvasGroup panelImageLightGroup = panelImageLight.GetComponent<CanvasGroup>();
        if (panelImageLightGroup == null)
        {
            panelImageLightGroup = panelImageLight.gameObject.AddComponent<CanvasGroup>();
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            if (lightPanelGroup)
                lightPanelGroup.alpha = 1 - t;
            if (panelGroup)
                panelGroup.alpha = 1 - t;
            if (panelImageLightGroup)
                panelImageLightGroup.alpha = 1 - t;
            yield return null;
        }

        if (lightPanelGroup)
            lightPanelGroup.alpha = 0;
        if (panelGroup)
            panelGroup.alpha = 0;
        if (panelImageLightGroup)
            panelImageLightGroup.alpha = 0;

        if (lightPanelGroup)
            lightPanelGroup.blocksRaycasts = false;
        if (panelGroup)
            panelGroup.blocksRaycasts = false;
        if (panelImageLightGroup)
            panelImageLightGroup.blocksRaycasts = false;
    }
}
