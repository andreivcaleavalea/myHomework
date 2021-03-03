using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Homework : MonoBehaviour
{
    public HomeworkModel homeworkModel;
    private void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        texts[0].text = homeworkModel.homeworkName;
        texts[1].text = homeworkModel.homeworkSubject;
    }
    public void SetInfo(string name, string subject, string description, int index)
    {
        homeworkModel.homeworkName = name;
        homeworkModel.homeworkSubject = subject;
        homeworkModel.homeworkDescription = description;
        homeworkModel.homeworkIndex = index;
    }
}
