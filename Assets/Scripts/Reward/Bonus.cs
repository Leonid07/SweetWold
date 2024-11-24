using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bonus : MonoBehaviour
{
    public Text dailyBonusText;
    public Text weeklyBonusText;
    public Text hourlyBonusText; // Текст для часового бонуса

    public GameObject blumeHourlyBoune;
    public GameObject blumeDailyBoune;
    public GameObject blumeWeeklyBoune;

    [Space(10)]
    public Text hourlyText; // Текст для часового бонуса
    public Text weeklyText; // Текст для часового бонуса
    public Text dailyText; // Текст для часового бонуса
    [Space(10)]
    public Button dailyBonusButton;
    public Button weeklyBonusButton;
    public Button hourlyBonusButton; // Кнопка для часового бонуса

    public GameObject dailyFG;
    public GameObject weeklyFG;
    public GameObject hourlyFG; // Графический объект для часового бонуса

    [Space(10)]
    public Sprite spriteStandartWelcome;
    public Sprite spriteStandartDaily;
    public Sprite spriteStandartWeekly;
    [Space(10)]
    public Sprite spriteOffWelcome;
    public Sprite spriteOffDaily;
    public Sprite spriteOffWeekly;

    private const string DailyBonusTimeKey = "daily_bonus_time";
    private const string WeeklyBonusTimeKey = "weekly_bonus_time";
    private const string HourlyBonusTimeKey = "hourly_bonus_time"; // Ключ для сохранения времени часового бонуса

    public int HourlyBonusCooldownInSeconds = 3600; // 1 час
    public int DailyBonusCooldownInSeconds = 86400; // 24 часа
    public int WeeklyBonusCooldownInSeconds = 604800; // 7 дней

    public int countHourly = 1; // Количество награды за часовой бонус
    public int countDaily = 5;
    public int countWeekly = 50;

    [Header("Картинка анимации")]
    public Image light_1;
    public Image light_2;
    public Image light_3;

    private void Start()
    {
        dailyBonusButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(light_1, ClaimDailyBonus, dailyBonusButton)));
        weeklyBonusButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(light_2, ClaimWeeklyBonus, weeklyBonusButton)));
        hourlyBonusButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(light_3, ClaimHourlyBonus, hourlyBonusButton)));

        StartCoroutine(UpdateBonusTextsRoutine());
    }

    private IEnumerator UpdateBonusTextsRoutine()
    {
        while (true)
        {
            UpdateBonusTexts();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void UpdateBonusTexts()
    {
        string dailyBonusTimeStr = PlayerPrefs.GetString(DailyBonusTimeKey, "0");
        string weeklyBonusTimeStr = PlayerPrefs.GetString(WeeklyBonusTimeKey, "0");
        string hourlyBonusTimeStr = PlayerPrefs.GetString(HourlyBonusTimeKey, "0");

        long dailyBonusTime = long.Parse(dailyBonusTimeStr);
        long weeklyBonusTime = long.Parse(weeklyBonusTimeStr);
        long hourlyBonusTime = long.Parse(hourlyBonusTimeStr);

        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;

        long dailyCooldown = dailyBonusTime + DailyBonusCooldownInSeconds - currentTimestamp;
        long weeklyCooldown = weeklyBonusTime + WeeklyBonusCooldownInSeconds - currentTimestamp;
        long hourlyCooldown = hourlyBonusTime + HourlyBonusCooldownInSeconds - currentTimestamp;

        dailyBonusText.text = FormatTimeDaily(dailyCooldown);
        weeklyBonusText.text = FormatTimeWeekly(weeklyCooldown);
        hourlyBonusText.text = FormatTimeHourly(hourlyCooldown);

        dailyBonusButton.interactable = dailyCooldown <= 0;
        weeklyBonusButton.interactable = weeklyCooldown <= 0;
        hourlyBonusButton.interactable = hourlyCooldown <= 0;
    }

    private string FormatTimeDaily(long seconds)
    {
        if (seconds <= 0)
        {
            dailyFG.SetActive(false);
            blumeDailyBoune.SetActive(true);
            dailyBonusButton.GetComponent<Image>().sprite = spriteStandartDaily;
            dailyText.gameObject.SetActive(true);
            return "Ready";
        }
        dailyFG.SetActive(true);
        blumeDailyBoune.SetActive(false);
        dailyBonusButton.GetComponent<Image>().sprite = spriteOffDaily;
        dailyText.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private string FormatTimeWeekly(long seconds)
    {
        if (seconds <= 0)
        {
            weeklyFG.SetActive(false);
            blumeWeeklyBoune.SetActive(true);
            weeklyBonusButton.GetComponent<Image>().sprite = spriteStandartWeekly;
            weeklyText.gameObject.SetActive(true);
            return "Ready";
        }
        weeklyFG.SetActive(true);
        blumeWeeklyBoune.SetActive(false);
        weeklyBonusButton.GetComponent<Image>().sprite = spriteOffWeekly;
        weeklyText.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        int totalHours = (int)timeSpan.TotalHours;
        return string.Format("{0:D2}:{1:D2}:{2:D2}", totalHours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private string FormatTimeHourly(long seconds)
    {
        if (seconds <= 0)
        {
            hourlyFG.SetActive(false);
            blumeHourlyBoune.SetActive(true);
            hourlyBonusButton.GetComponent<Image>().sprite = spriteStandartWelcome;
            hourlyText.gameObject.SetActive(true);
            return "Ready";
        }
        hourlyFG.SetActive(true);
        blumeHourlyBoune.SetActive(false);
        hourlyBonusButton.GetComponent<Image>().sprite = spriteOffWelcome;
        hourlyText.gameObject.SetActive(false);
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
    }

    private void ClaimDailyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameManager.InstanceGame.gold += countDaily;
        DataManager.InstanceData.SaveGold();
        PlayerPrefs.SetString(DailyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Daily Bonus Claimed!");
        Debug.Log($"New Daily Bonus Time: {currentTimestamp}");
    }

    private void ClaimWeeklyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameManager.InstanceGame.gold += countWeekly;
        DataManager.InstanceData.SaveGold();
        PlayerPrefs.SetString(WeeklyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Weekly Bonus Claimed!");
        Debug.Log($"New Weekly Bonus Time: {currentTimestamp}");
    }

    private void ClaimHourlyBonus()
    {
        long currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        GameManager.InstanceGame.gold += countHourly;
        DataManager.InstanceData.SaveGold();
        PlayerPrefs.SetString(HourlyBonusTimeKey, currentTimestamp.ToString());
        PlayerPrefs.Save();

        Debug.Log("Hourly Bonus Claimed!");
        Debug.Log($"New Hourly Bonus Time: {currentTimestamp}");
    }

    private IEnumerator HandleButtonClick(Image button, Action onAnimationComplete, Button buttonImage)
    {
        // Выполняем анимацию
        yield return StartCoroutine(PlayButtonAnimation(button, buttonImage));

        // После завершения анимации выполняем основное действие
        onAnimationComplete.Invoke();
    }

    private IEnumerator PlayButtonAnimation(Image button,  Button buttonImage)
    {
        Transform buttonTransform = button.transform;
        Transform buttnoTr = buttonImage.GetComponent<Image>().gameObject.transform;

        // Поворот на 30 градусов вправо
        Quaternion initialRotation = buttnoTr.rotation;
        Quaternion rightRotation = initialRotation * Quaternion.Euler(0, 0, 30);
        Quaternion leftRotation = initialRotation * Quaternion.Euler(0, 0, -30);
        float rotationTime = 0.25f;

        yield return RotateButton(buttnoTr, rightRotation, rotationTime);
        yield return RotateButton(buttnoTr, leftRotation, rotationTime);
        yield return RotateButton(buttnoTr, initialRotation, rotationTime);

        // Увеличение размеров изображения на кнопке
        Vector3 initialScale = buttonTransform.localScale;
        Vector3 targetScale = new Vector3(2, 2, 2);
        float scaleTime = 0.5f;

        yield return ScaleButton(buttonTransform, Vector3.zero, scaleTime * 0.5f);
        yield return ScaleButton(buttonTransform, targetScale, scaleTime);
        yield return ScaleButton(buttonTransform, initialScale, scaleTime);
    }

    private IEnumerator RotateButton(Transform buttonTransform, Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = buttonTransform.rotation;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            buttonTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            yield return null;
        }

        buttonTransform.rotation = targetRotation;
    }

    private IEnumerator ScaleButton(Transform buttonTransform, Vector3 targetScale, float duration)
    {
        Vector3 startScale = buttonTransform.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            buttonTransform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            yield return null;
        }

        buttonTransform.localScale = targetScale;
    }
}
