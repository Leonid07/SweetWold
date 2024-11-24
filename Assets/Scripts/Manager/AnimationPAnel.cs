using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPAnel : MonoBehaviour
{
    public Image parrentImage;

    public RectTransform panel; // ������ �� RectTransform ������
    public float duration = 1.0f; // ����������������� ��������
    public float delay = 1.0f; // �������� ����� �������� ���������

    private Vector2 initialOffsetMin;
    private Vector2 initialOffsetMax;

    void Start()
    {
        initialOffsetMin = panel.offsetMin;
        initialOffsetMax = panel.offsetMax;
    }
    public void StartAnimation(GameObject panel, bool isActive = false)
    {
        StartCoroutine(AnimatePanel(panel, isActive));
    }
    public void StartAnimationUnLockLevel(GameObject panel)
    {
        StartCoroutine(AnimatePanelUnlock(panel));
    }
    private IEnumerator AnimatePanelUnlock(GameObject panel)
    {
        yield return StartCoroutine(ClosePanel());


        panel.SetActive(true);

        yield return new WaitForSeconds(delay);

        yield return StartCoroutine(OpenPanel());
    }

    private IEnumerator AnimatePanel(GameObject panel, bool isActive = false)
    {
        yield return StartCoroutine(ClosePanel());

        if (isActive == false)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
        }

        yield return new WaitForSeconds(delay);

        yield return StartCoroutine(OpenPanel());
    }

    private IEnumerator ClosePanel()
    {
        parrentImage.raycastTarget = true;
        float elapsedTime = 0f;

        float startLeft = panel.offsetMin.x;
        float startRight = panel.offsetMax.x;

        float endLeft = 0f;
        float endRight = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float newLeft = Mathf.Lerp(startLeft, endLeft, t);
            float newRight = Mathf.Lerp(startRight, endRight, t);

            panel.offsetMin = new Vector2(newLeft, panel.offsetMin.y);
            panel.offsetMax = new Vector2(newRight, panel.offsetMax.y);

            yield return null;
        }

        panel.offsetMin = new Vector2(endLeft, panel.offsetMin.y);
        panel.offsetMax = new Vector2(endRight, panel.offsetMax.y);
    }

    private IEnumerator OpenPanel()
    {
        float elapsedTime = 0f;

        float startLeft = panel.offsetMin.x;
        float startRight = panel.offsetMax.x;

        float endLeft = initialOffsetMin.x;
        float endRight = initialOffsetMax.x;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            float newLeft = Mathf.Lerp(startLeft, endLeft, t);
            float newRight = Mathf.Lerp(startRight, endRight, t);

            panel.offsetMin = new Vector2(newLeft, panel.offsetMin.y);
            panel.offsetMax = new Vector2(newRight, panel.offsetMax.y);

            yield return null;
        }
        parrentImage.raycastTarget = false;
        panel.offsetMin = new Vector2(endLeft, panel.offsetMin.y);
        panel.offsetMax = new Vector2(endRight, panel.offsetMax.y);
    }
}