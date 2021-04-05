using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Homework : MonoBehaviour
{
    public HomeworkModel homeworkModel;
    public GameObject doneLayout;
    private void Start()
    {
        var texts = GetComponentsInChildren<Text>();
        texts[0].text = homeworkModel.homeworkName;
        texts[1].text = homeworkModel.homeworkSubject;
    }
    public void DoneHomework(bool ok)
    {
        if(ok)
        {
            doneLayout.SetActive(true);
        }
        else
        {
            doneLayout.SetActive(false);
            transform.SetAsFirstSibling();
        }
    }
    public void SetInfo(string hName, string subject, string description, int index,bool doneHomework)
    {
        homeworkModel.homeworkName = hName;
        homeworkModel.homeworkSubject = subject;
        homeworkModel.homeworkDescription = description;
        homeworkModel.homeworkIndex = index;
        homeworkModel.doneHomework = doneHomework;
    }
}
