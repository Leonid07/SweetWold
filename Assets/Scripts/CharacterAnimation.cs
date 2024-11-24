using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [Header("Настройки масштабирования")]
    public float inhaleSpeed = 1f;
    public float exhaleSpeed = 1f;

    [Header("Пределы масштабирования")]
    public float maxScaleY = 1.2f;
    public float minScaleY = 0.8f;

    private RectTransform rectTransform;
    private bool isInhaling = true;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(BreathingCoroutine());
    }

    private IEnumerator BreathingCoroutine()
    {
        while (true)
        {
            Vector3 scale = rectTransform.localScale;
            Vector3 position = rectTransform.localPosition;

            if (isInhaling)
            {
                while (scale.y < maxScaleY)
                {
                    scale.y += inhaleSpeed * Time.deltaTime;
                    position.y += exhaleSpeed * Time.deltaTime;
                    rectTransform.localScale = new Vector3(scale.x, scale.y, scale.z);
                    rectTransform.localPosition = new Vector3(position.x, position.y, position.z);
                    yield return null;
                }
                isInhaling = false;
            }
            else
            {
                while (scale.y > minScaleY)
                {
                    scale.y -= exhaleSpeed * Time.deltaTime;
                    position.y -= exhaleSpeed * Time.deltaTime;
                    rectTransform.localScale = new Vector3(scale.x, scale.y, scale.z);
                    rectTransform.localPosition = new Vector3(position.x, position.y, position.z);
                    yield return null;
                }
                isInhaling = true;
            }

            yield return null;
        }
    }
}
