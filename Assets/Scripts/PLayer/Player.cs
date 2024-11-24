using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5f; // Скорость перемещения
    public float smoothTime = 0.3f; // Время сглаживания
    public float rotationSpeed = 5f; // Скорость вращения
    public Button continueButton; // UI кнопка для продолжения движения

    public List<Transform> points = new List<Transform>(); // Список точек

    public List<Transform> pointMain = new List<Transform>();

    public List<Transform> leftPoint = new List<Transform>();
    public List<Transform> rightPoint = new List<Transform>();

    private int currentPointIndex = 0; // Индекс текущей точки
    private bool isWaitingForButtonPress = false; // Флаг для проверки, ожидается ли нажатие кнопки
    private Coroutine moveCoroutine;

    public float totalTimeDoubleCutting;
    public float totalTimeTripleCutting;

    [Header("смена направления")]
    public GameObject panelArrow;
    public Button leftArrow;
    public Button rightArrow;

    void Start()
    {
        leftArrow.onClick.AddListener(MoveLeft);
        rightArrow.onClick.AddListener(MoveRight);

        points = pointMain;
        // Запускаем корутину для перемещения
        if (points.Count > 0)
        {
            moveCoroutine = StartCoroutine(MoveToPoints());
        }

        continueButton.gameObject.SetActive(false);
        // Привязываем метод к событию нажатия кнопки
        continueButton.onClick.AddListener(OnContinueButtonPressed);
    }

    private IEnumerator MoveToPoints()
    {
        while (true)
        {
            if (points.Count == 0)
            {
                continueButton.gameObject.SetActive(false);
                yield break;
            }

            // Переходим к следующей точке
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = points[currentPointIndex].position;
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation((targetPosition - startPosition).normalized);

            float journeyLength = Vector3.Distance(startPosition, targetPosition);
            float startTime = Time.time;

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                float distanceCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distanceCovered / journeyLength;

                transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, fractionOfJourney * rotationSpeed);

                yield return null;
            }

            // Обеспечиваем, чтобы финальная позиция и поворот точно соответствовали целевой
            transform.position = targetPosition;
            transform.rotation = targetRotation;

            // Проверка на наличие компонента StopPoint и вызов OnReached
            StopPoint stopPoint = points[currentPointIndex].GetComponent<StopPoint>();
            if (stopPoint != null && stopPoint.crossRoad == false)
            {
                stopPoint.OnReached();
                if (stopPoint.requiresPause)
                {
                    // Проверка на последнюю точку перед активацией кнопки
                    if (currentPointIndex >= points.Count - 1)
                    {
                        continueButton.gameObject.SetActive(false);
                        yield break; // Завершить корутину, если достигли последней точки
                    }

                    isWaitingForButtonPress = true;

                    // Ожидание нажатия кнопки для продолжения
                    yield return new WaitUntil(() => !isWaitingForButtonPress);
                }
            }

            if (stopPoint != null && stopPoint.crossRoad == true)
            {
                panelArrow.SetActive(true);
                stopPoint.OnReached();
                if (stopPoint.requiresPause)
                {
                    // Проверка на последнюю точку перед активацией кнопки
                    if (currentPointIndex >= points.Count - 1)
                    {
                        continueButton.gameObject.SetActive(false);
                        yield break; // Завершить корутину, если достигли последней точки
                    }

                    isWaitingForButtonPress = true;

                    // Ожидание нажатия кнопки для продолжения
                    yield return new WaitUntil(() => !isWaitingForButtonPress);
                }
            }

            // Если достигли последней точки, завершить корутину
            if (currentPointIndex >= points.Count - 1)
            {
                if (stopPoint != null && stopPoint.crossRoad == false)
                {
                    Debug.Log("Достигнута последняя точка");
                    continueButton.gameObject.SetActive(false);
                    yield break; // Завершить корутину, если достигли последней точки
                }
            }
            currentPointIndex++;
        }
    }
    public void MoveLeft()
    {
        currentPointIndex = 0;
        points = leftPoint;
        panelArrow.SetActive(false);
        isWaitingForButtonPress = false;
        StartCoroutine(MoveToPoints());
    }

    public void MoveRight()
    {
        currentPointIndex = 0;
        points = rightPoint;
        isWaitingForButtonPress = false;
        panelArrow.SetActive(false);
        StartCoroutine(MoveToPoints());
    }

    private void OnContinueButtonPressed()
    {
        // Скрываем кнопку и сбрасываем флаг ожидания
        continueButton.gameObject.SetActive(false);
        isWaitingForButtonPress = false;

        // Проверяем, находится ли персонаж на последней точке
        if (currentPointIndex >= points.Count - 1)
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    public void RestartMovement()
    {
        points = pointMain;
        // Останавливаем текущую корутину, если она запущена
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Сбрасываем индекс текущей точки и перезапускаем корутину
        currentPointIndex = 0;
        transform.position = points[0].position;
        transform.Rotate(0, 0, 0);
        moveCoroutine = StartCoroutine(MoveToPoints());
    }
}