using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5f; // �������� �����������
    public float smoothTime = 0.3f; // ����� �����������
    public float rotationSpeed = 5f; // �������� ��������
    public Button continueButton; // UI ������ ��� ����������� ��������

    public List<Transform> points = new List<Transform>(); // ������ �����

    public List<Transform> pointMain = new List<Transform>();

    public List<Transform> leftPoint = new List<Transform>();
    public List<Transform> rightPoint = new List<Transform>();

    private int currentPointIndex = 0; // ������ ������� �����
    private bool isWaitingForButtonPress = false; // ���� ��� ��������, ��������� �� ������� ������
    private Coroutine moveCoroutine;

    public float totalTimeDoubleCutting;
    public float totalTimeTripleCutting;

    [Header("����� �����������")]
    public GameObject panelArrow;
    public Button leftArrow;
    public Button rightArrow;

    void Start()
    {
        leftArrow.onClick.AddListener(MoveLeft);
        rightArrow.onClick.AddListener(MoveRight);

        points = pointMain;
        // ��������� �������� ��� �����������
        if (points.Count > 0)
        {
            moveCoroutine = StartCoroutine(MoveToPoints());
        }

        continueButton.gameObject.SetActive(false);
        // ����������� ����� � ������� ������� ������
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

            // ��������� � ��������� �����
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

            // ������������, ����� ��������� ������� � ������� ����� ��������������� �������
            transform.position = targetPosition;
            transform.rotation = targetRotation;

            // �������� �� ������� ���������� StopPoint � ����� OnReached
            StopPoint stopPoint = points[currentPointIndex].GetComponent<StopPoint>();
            if (stopPoint != null && stopPoint.crossRoad == false)
            {
                stopPoint.OnReached();
                if (stopPoint.requiresPause)
                {
                    // �������� �� ��������� ����� ����� ���������� ������
                    if (currentPointIndex >= points.Count - 1)
                    {
                        continueButton.gameObject.SetActive(false);
                        yield break; // ��������� ��������, ���� �������� ��������� �����
                    }

                    isWaitingForButtonPress = true;

                    // �������� ������� ������ ��� �����������
                    yield return new WaitUntil(() => !isWaitingForButtonPress);
                }
            }

            if (stopPoint != null && stopPoint.crossRoad == true)
            {
                panelArrow.SetActive(true);
                stopPoint.OnReached();
                if (stopPoint.requiresPause)
                {
                    // �������� �� ��������� ����� ����� ���������� ������
                    if (currentPointIndex >= points.Count - 1)
                    {
                        continueButton.gameObject.SetActive(false);
                        yield break; // ��������� ��������, ���� �������� ��������� �����
                    }

                    isWaitingForButtonPress = true;

                    // �������� ������� ������ ��� �����������
                    yield return new WaitUntil(() => !isWaitingForButtonPress);
                }
            }

            // ���� �������� ��������� �����, ��������� ��������
            if (currentPointIndex >= points.Count - 1)
            {
                if (stopPoint != null && stopPoint.crossRoad == false)
                {
                    Debug.Log("���������� ��������� �����");
                    continueButton.gameObject.SetActive(false);
                    yield break; // ��������� ��������, ���� �������� ��������� �����
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
        // �������� ������ � ���������� ���� ��������
        continueButton.gameObject.SetActive(false);
        isWaitingForButtonPress = false;

        // ���������, ��������� �� �������� �� ��������� �����
        if (currentPointIndex >= points.Count - 1)
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    public void RestartMovement()
    {
        points = pointMain;
        // ������������� ������� ��������, ���� ��� ��������
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // ���������� ������ ������� ����� � ������������� ��������
        currentPointIndex = 0;
        transform.position = points[0].position;
        transform.Rotate(0, 0, 0);
        moveCoroutine = StartCoroutine(MoveToPoints());
    }
}