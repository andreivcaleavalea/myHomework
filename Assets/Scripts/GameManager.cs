using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FilesManager;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public Transform content;
    public GameObject addPannelFull;
    public GameObject addPannel;
    public GameObject showPannel;
    public GameObject showPannelFull;
    public List<HomeworkModel> homeworksList;
    public GameObject homeworkPrefab;
    public InputField[] inputFields;
    public List<string> subjectList;
    private void Start()
    {
        inputFields = addPannel.GetComponentsInChildren<InputField>();
        homeworksList = GetHomeworksFromJson();
        Repopulate();
    }

    public void AddPannelButton()
    {
        addPannelFull.SetActive(true);
    }
    public void CreateHomework()
    {
        foreach (var content in inputFields)
        {
            if (content.text == "")
                return;
        }
        AddHomeworkItem(inputFields[0].text, inputFields[1].text, inputFields[2].text);
        addPannelFull.SetActive(false);
        Repopulate(true);
    }
    public void AddHomeworkItem(string name, string subject, string description)
    {
        GameObject item = Instantiate(homeworkPrefab, content);
        int index = homeworksList.Count;

        Homework homework = item.GetComponent<Homework>();
        homework.SetInfo(name, subject, description, index);
        homeworksList.Add(homework.homeworkModel);
        RefreshHomeworksList(homeworksList);
    }
    void DestroyHomework(GameObject item, HomeworkModel homeworkModel)
    {
        Debug.Log("homework destroyed");
        /*homeworksList.Remove(homeworkModel);*/
        homeworksList.RemoveAt(homeworkModel.homeworkIndex);
        foreach (var x in homeworksList)
            Debug.Log(x.homeworkName);
        RefreshHomeworksList(homeworksList);
        Destroy(item);
        
        /*Repopulate(true);*/
    }
    public void Repopulate(bool index = false)
    {
        if (index != false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        /*homeworksList = GetHomeworksFromJson();*/
        if (homeworksList != null)
        {
            for (int i = 0; i < homeworksList.Count; i++)
            {
                GameObject homeworkItem = Instantiate(homeworkPrefab, content);
                Homework homework = homeworkItem.GetComponent<Homework>();
                homework.SetInfo(
                    homeworksList[i].homeworkName,
                    homeworksList[i].homeworkSubject,
                    homeworksList[i].homeworkDescription,
                    ///homeworksList[i].homeworkDeadline,
                    homeworksList[i].homeworkIndex);
                HomeworkModel homeworkModel = homeworkItem.GetComponent<Homework>().homeworkModel;
                Button[] btn = homeworkItem.GetComponentsInChildren<Button>();
                Debug.Log(btn[0]);
                Debug.Log(btn[1]);
                btn[0].onClick.AddListener(delegate
                {
                    DestroyHomework(homeworkItem, homeworkModel);
                });
                btn[1].onClick.AddListener(delegate
                {
                    ShowHomework(homeworkModel.homeworkName, homeworkModel.homeworkSubject, homeworkModel.homeworkDescription);
                });
            }
        }
    }
    void ShowHomework(string name, string subject, string description)
    {
        showPannelFull.SetActive(true);
        showPannel.GetComponent<ShowPannelScript>().SetInfo(name, subject, description);
    }
}
