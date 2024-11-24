using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Education : MonoBehaviour
{
    public Button thisButton;

    public GameObject[] labels;

    public int count = 0;

    void Start()
    {
        StartCoroutine(qwe());
        thisButton= GetComponent<Button>();
        thisButton.onClick.AddListener(EducationClick);
    }
    IEnumerator qwe()
    {
        yield return new WaitForSeconds(1);
        if (DataManager.InstanceData.valueEd >= 1)
        {
            gameObject.SetActive(false);
        }
    }
    public void EducationClick()
    {
        labels[count].SetActive(false);
        count++;
        if (count < labels.Length)
        {
            labels[count].SetActive(true);
        }
        if (count >= labels.Length)
        {
            DataManager.InstanceData.valueEd++;
            gameObject.SetActive(false);
            DataManager.InstanceData.SaveEducation();
        }
    }
}
