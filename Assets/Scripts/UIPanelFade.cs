using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelFade : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    public CanvasGroup canvasGroup;

    [Header("Эффект конфети")]
    public bool isEffectPlay = false;
    public GameObject[] uiElementPrefabs;
    public RectTransform spawnPoint;
    public int spawnCount = 10;
    public float spawnInterval = 0.5f;
    public float gravityScale = 100f;

    private int currentPrefabIndex = 0;

    [Header("Анимация приза за прохождения уровня")]
    public float duration = 1.0f;
    public RectTransform imageReward;
    private Vector3 initialScale;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Deffoult()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void FadeIn()
    {
        if (isEffectPlay == true)
        {
            StartCoroutine(SpawnUIElements());
        }
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, fadeDuration));
    }

    public void FadeOut()
    {
        if (isEffectPlay == true)
        {
            StartCoroutine(SpawnUIElements());
        }
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, fadeDuration));
    }

    public void ActiveAnimReward()
    {
        imageReward.localScale = new Vector3(1,1,1);
        initialScale = imageReward.localScale;
        StartCoroutine(ScaleDown());
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cg.alpha = end;
        cg.interactable = (end == 1);
        cg.blocksRaycasts = (end == 1);
    }
    IEnumerator SpawnUIElements()
    {
        RectTransform parentRect = spawnPoint.parent.GetComponent<RectTransform>();

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject uiElementPrefab = uiElementPrefabs[currentPrefabIndex];
            float rotate = Random.Range(-180, 180);

            Vector2 randomPosition = new Vector2(
                Random.Range(0, parentRect.rect.width) - parentRect.rect.width * 0.5f,
                Random.Range(0, parentRect.rect.height) - parentRect.rect.height * 0.5f
            );

            GameObject uiElement = Instantiate(uiElementPrefab, spawnPoint.position, Quaternion.identity, spawnPoint);
            RectTransform elementRect = uiElement.GetComponent<RectTransform>();
            elementRect.localPosition = randomPosition;
            elementRect.localEulerAngles = new Vector3(0, 0, rotate);

            Rigidbody2D rb = uiElement.AddComponent<Rigidbody2D>();
            rb.gravityScale = gravityScale;

            currentPrefabIndex = (currentPrefabIndex + 1) % uiElementPrefabs.Length;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private IEnumerator ScaleDown()
    {
        Vector3 targetScale = Vector3.zero;
        Vector3 currentScale = imageReward.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            imageReward.localScale = Vector3.Lerp(currentScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        imageReward.localScale = targetScale;
    }
}
