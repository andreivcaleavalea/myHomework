using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FilesManager;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.Notifications.Android;

public class GameManager : MonoBehaviour
{
    #region Declaration
    public Transform content;
    public GameObject addPannelFull;
    public GameObject addPannel;
    public GameObject showPannel;
    public GameObject showPannelFull;
    public List<HomeworkModel> homeworksList;
    public GameObject homeworkPrefab;
    public InputField[] inputFields;
    public List<string> subjectList;
    public static Resolution currentResolution;
    #region UIElementsforInitialize
    public Text errorText;
    public RectTransform backButton;
    public RectTransform createButton;
    public RectTransform backcreateRect;
    public RectTransform nameInput;
    public RectTransform subjectInput;
    public RectTransform descriptionInput;
    public RectTransform nameNdSubject;
    public RectTransform description;
    #endregion
    #endregion
    private void Start()
    {
        InitializeElements();
        inputFields = addPannel.GetComponentsInChildren<InputField>();
        homeworksList = GetHomeworksFromJson();
        Repopulate();
        CreateChannel();
        SendNotification();
    }
    private void CreateChannel()
    {
        var notificationChannel = new AndroidNotificationChannel()
        {
            Id="HaveHomeworks",
            Name="HaveHomeworks",
            Importance=Importance.Default
        };
        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);
    }
    void SendNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = "myHomework";
        notification.Text = "Hello user, you have " + homeworksList.Count + " homework to do!";
/*        notification.FireTime=System.da
*/    }
    private void InitializeElements()
    {
        int valueOfBackAndCreate = (int)backcreateRect.rect.width;
        backButton.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, valueOfBackAndCreate/2-1);
        createButton.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, valueOfBackAndCreate / 2 - 1);
        nameInput.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(0.8 * valueOfBackAndCreate));
        subjectInput.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(0.8 * valueOfBackAndCreate));
        descriptionInput.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(0.8 * valueOfBackAndCreate));
        nameNdSubject.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(0.8 * valueOfBackAndCreate));
        description.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(0.8 * valueOfBackAndCreate));
        errorText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(0.8 * valueOfBackAndCreate));
    }
    public void AddPannelButton()
    {
        addPannelFull.SetActive(true);
    }
    public void CreateHomework()
    {
        for(int i=0;i<=1;i++)
        {
            if (inputFields[i].text == "")
            {
                ShowToast("Please complete the name and subject inputs", 2);
                return;
            }
        }
        AddHomeworkItem(inputFields[0].text, inputFields[1].text, inputFields[2].text);
        addPannelFull.SetActive(false);
        Repopulate(true);
    }
    private void ShowToast(string text, int duration)
    {
        StartCoroutine(showToastCOR(text, duration));
    }
    private IEnumerator showToastCOR(string text, int duration)
    {
        /*Color originalColor = this.text.color;*/
        Color originalColor = new Vector4(255, 0, 0);
        errorText.text = text;
        errorText.enabled = true;
        errorText.gameObject.SetActive(true);
        yield return FadeInAndOut(errorText, true, 0.5f);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        yield return FadeInAndOut(errorText, false, 0.5f);
        errorText.enabled = false;
        errorText.gameObject.SetActive(false);
        errorText.color = originalColor;

    }
    IEnumerator FadeInAndOut(Text targetText, bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        Color currentColor = Color.clear;
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            /*targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);*/
            targetText.color = new Color(255, 0, 0, alpha);
            yield return null;
        }
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
