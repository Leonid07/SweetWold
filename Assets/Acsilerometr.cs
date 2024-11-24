using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acsilerometr : MonoBehaviour
{
    public RectTransform imageTransform;  // ѕрив€зка UI изображени€
    public float rotationSpeed = 100f;    // —корость вращени€ изображени€
    public float rotationLimit = 5f;      // ќграничение угла поворота
    public bool simulateZeroAcceleration = false; // ‘лаг дл€ симул€ции 0 значений акселерометра

    void Update()
    {
        Vector3 acceleration;

        // ≈сли мы в режиме симул€ции, устанавливаем акселерометр на 0
        if (simulateZeroAcceleration || !Application.isMobilePlatform)
        {
            acceleration = Vector3.zero;
        }
        else
        {
            // ѕолучаем данные с акселерометра
            acceleration = Input.acceleration;
        }

        // –ассчитываем угол поворота по ос€м X и Y в зависимости от данных акселерометра
        float rotationX = Mathf.Clamp(acceleration.y * rotationSpeed, -rotationLimit, rotationLimit);
        float rotationY = Mathf.Clamp(-acceleration.x * rotationSpeed, -rotationLimit, rotationLimit); // "-" дл€ инверсии движени€

        // ѕримен€ем поворот к изображению, использу€ только оси X и Y
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
