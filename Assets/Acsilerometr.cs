using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acsilerometr : MonoBehaviour
{
    public RectTransform imageTransform;  // �������� UI �����������
    public float rotationSpeed = 100f;    // �������� �������� �����������
    public float rotationLimit = 5f;      // ����������� ���� ��������
    public bool simulateZeroAcceleration = false; // ���� ��� ��������� 0 �������� �������������

    void Update()
    {
        Vector3 acceleration;

        // ���� �� � ������ ���������, ������������� ������������ �� 0
        if (simulateZeroAcceleration || !Application.isMobilePlatform)
        {
            acceleration = Vector3.zero;
        }
        else
        {
            // �������� ������ � �������������
            acceleration = Input.acceleration;
        }

        // ������������ ���� �������� �� ���� X � Y � ����������� �� ������ �������������
        float rotationX = Mathf.Clamp(acceleration.y * rotationSpeed, -rotationLimit, rotationLimit);
        float rotationY = Mathf.Clamp(-acceleration.x * rotationSpeed, -rotationLimit, rotationLimit); // "-" ��� �������� ��������

        // ��������� ������� � �����������, ��������� ������ ��� X � Y
        imageTransform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        if (acceleration.x != 0 || acceleration.y != 0 || acceleration.z != 0)
        {
            GameManager.InstanceGame.isAcsi = true;
        }
        else
        {
            GameManager.InstanceGame.isAcsi = false;
        }
    }
}
