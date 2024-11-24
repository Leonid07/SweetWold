using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StopPoint : MonoBehaviour
{
    public bool requiresPause = true; // Определяет, нужно ли останавливать персонажа на этой точке
    public bool lastEnemy = false;
    public bool firstPoint = false;

    public Enemy enemy;

    public float fadeDuration = 1.0f;

    [Header("Параметры игрока")]
    public GameObject spawnPoint_1;
    public GameObject buttonStart;

    public float delayDoubleAttack = 1.5f;
    public float dalayTripleAttack = 2.5f;

    // Метод для обработки достижения точки (можно добавить другие параметры и действия)
    public void OnReached()
    {
        if (requiresPause)
        {
            if (enemy != null)
            {
                if (enemy.damage >= PanelManager.InstancePanel.powerPlayer)
                {
                    SoundManager.InstanceSound.musicFon.Play();
                    SoundManager.InstanceSound.musicLevel.Stop();
                    PanelManager.InstancePanel._UIPanelFade.FadeIn();
                }
                else
                {
                    if (DataManager.InstanceData.mapNextLevel.indexLevel <= 2)
                    {
                        StartCoroutine(DoubleCutting());
                    }
                    else
                    {
                        StartCoroutine(TripleCutting());
                    }
                }
            }
        }
    }
    public IEnumerator DoubleCutting()
    {
        buttonStart.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        if (SoundManager.InstanceSound.soundDamage != null)
            SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        enemy.cutting.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (SoundManager.InstanceSound.soundDamage != null)
            SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);

        StartCoroutine(FadeAndPlaySound());
        if (lastEnemy == true)
        {
            if (DataManager.InstanceData.mapNextLevel.mapNextLevel.isLoad == 0)
            {
                //PanelManager.InstancePanel.SetActivePanel(true);
                //DataManager.InstanceData.mapNextLevel.OpenLevel();
            }
            else
            {
                PanelManager.InstancePanel.SetActivePanel(false);
            }
            SoundManager.InstanceSound.musicFon.Play();
            SoundManager.InstanceSound.musicLevel.Stop();
            GameManager.InstanceGame.gold += 1000;
            DataManager.InstanceData.SaveGold();
            Debug.Log("End Game");
            PanelManager.InstancePanel.panelWin.FadeIn();

            PanelManager.InstancePanel.panelWin.ActiveAnimReward();


            buttonStart.SetActive(false);
            yield break;
        }
        buttonStart.SetActive(true);
    }

    public IEnumerator TripleCutting()
    {
        buttonStart.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        if (SoundManager.InstanceSound.soundDamage != null)
            SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        enemy.cutting.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (SoundManager.InstanceSound.soundDamage != null)
            SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        enemy.cutting.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (SoundManager.InstanceSound.soundDamage != null)
            SoundManager.InstanceSound.soundDamage.Play();
        enemy.cutting.gameObject.SetActive(true);

        StartCoroutine(FadeAndPlaySound());
        if (lastEnemy == true)
        {
            if (DataManager.InstanceData.mapNextLevel.mapNextLevel.isLoad == 0)
            {
                //PanelManager.InstancePanel.SetActivePanel(true);
                //DataManager.InstanceData.mapNextLevel.OpenLevel();
            }
            else
            {
                Debug.Log("прохождение одного и тогоже уровня");
                PanelManager.InstancePanel.SetActivePanel(false);
            }
            SoundManager.InstanceSound.musicFon.Play();
            SoundManager.InstanceSound.musicLevel.Stop();
            GameManager.InstanceGame.gold += 1000;
            DataManager.InstanceData.SaveGold();
            Debug.Log("End Game");
            PanelManager.InstancePanel.panelWin.FadeIn();

            PanelManager.InstancePanel.panelWin.ActiveAnimReward();

            buttonStart.SetActive(false);
            yield break;
        }
        buttonStart.SetActive(true);
    }

    private IEnumerator FadeAndPlaySound()
    {
        float elapsedTime = 0f;
        Color originalColor = enemy.spriteEnemy.color;
        Color originalColorcutting = enemy.cutting.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);

            Color newColor = originalColor;
            Color newColorcutting = originalColorcutting;

            newColor.a = Mathf.Lerp(originalColor.a, 0, t);
            newColorcutting.a = Mathf.Lerp(originalColorcutting.a, 0, t);

            enemy.spriteEnemy.color = newColor;
            enemy.cutting.color = newColorcutting;

            yield return null;
        }

        Color finalColor = enemy.spriteEnemy.color;
        Color finalColorcutting = enemy.cutting.color;

        finalColor.a = 0;
        finalColorcutting.a = 0;

        enemy.spriteEnemy.color = finalColor;
        enemy.cutting.color = finalColorcutting;
    }

    [Header("Перекрёсток")]
    public bool crossRoad = false;
}