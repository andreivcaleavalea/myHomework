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
    private List<HomeworkModel> homeworksList;
    [SerializeField]private GameObject homeworkPrefab;

#region UI Elements
    [Header("UI Elements")]
    [SerializeField] private InputField[] inputFields;
    [SerializeField] private Transform content;
    [SerializeField]
    private GameObject
        titleObject,///normal
        addButton,//normal
        addPannelFull,//normal
        nameInputHead,//light
        nameInputBody,//dark
        nameInputFull,
        subjectInputHead,//light
        subjectInputBody,//dark
        subjectInputFull,
        descriptionInputHead,//light
        descriptionInputBody,//dark
        descriptionInputFull,
        backCreateButton,//light
        backButtonHS,
        createButtonHS,
        showPannel,//normal
        showPannelFull,
        nameSubjectFull,
        nameToShow,//light
        subjectToShow,//dark
        descriptionFull,
        descriptionToShowTitle,//light
        descriptionToShowContainer,//dark
        backButton;//light
    [SerializeField] private Text errorText;
#endregion

    private void Start()
    {
        InitializeElements();
        CreateChannel();
        SendNotification();
    }
    private void InitializeElements()
    {
        //control elememts size
        int screenWidth = (int)titleObject.GetComponent<RectTransform>().rect.width;
        backButtonHS.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenWidth / 2 - 2);
        createButtonHS.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenWidth / 2 - 1);
        nameInputFull.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(screenWidth * 0.8));
        subjectInputFull.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(screenWidth * 0.8));
        descriptionInputFull.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(screenWidth * 0.8));
        nameSubjectFull.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(screenWidth * 0.8));
        descriptionFull.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)(screenWidth * 0.8));

        homeworksList = GetHomeworksFromJson();
        PopulateInScene();
    }
    private void PopulateInScene()
    {
        if (homeworksList != null)
        {
            for(int i = 0; i < homeworksList.Count; i++)
            {
                GameObject homeworkObject = Instantiate(homeworkPrefab, content);
                Homework homeworkScript = homeworkObject.GetComponent<Homework>();
                homeworkScript.SetInfo(
                    homeworksList[i].homeworkName,
                    homeworksList[i].homeworkSubject,
                    homeworksList[i].homeworkDescription,
                    homeworksList[i].homeworkIndex
                    );
                HomeworkModel homeworkModel = homeworkScript.homeworkModel;
                Button[] buttons = homeworkObject.GetComponentsInChildren<Button>();
                buttons[0].onClick.AddListener(delegate { DestroyHomework(homeworkObject, homeworkModel); });
                buttons[1].onClick.AddListener(delegate { ShowHomework(homeworkModel.homeworkName, homeworkModel.homeworkSubject, homeworkModel.homeworkDescription); });
            }
        }
    }
    private void DestroyHomework(GameObject homeworkObject, HomeworkModel homeworkModel)
    {
        homeworksList.Remove(homeworkModel);
        //homeworksList.RemoveAt(homeworkModel.homeworkIndex);
        RefreshHomeworksList(homeworksList);
        Destroy(homeworkObject);
        for (int i = 0; i < homeworksList.Count; i++)
            homeworksList[i].homeworkIndex = i;
    }
    private void ShowHomework(string name,string subject, string description)
    {
        showPannelFull.SetActive(true);
        showPannel.GetComponent<ShowPannelScript>().SetInfo(name, subject, description);
    }
    private void CreateChannel()
    {
        var notificationChannel = new AndroidNotificationChannel()
        {
            Id = "HaveHomeworks",
            Name = "HaveHomeworks",
            Importance = Importance.Default
        };
        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);
    }
    private void SendNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = "myHomework";
        notification.Text = "Hello user, you have " + homeworksList.Count + " homework to do!";
        notification.FireTime = System.DateTime.Now.AddHours(5);
        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";
        AndroidNotificationCenter.SendNotification(notification, "HaveHomeworks");

    }
    public void CreateHomework()
    {
        Debug.Log("Start create homework function with: ");
        for (int i = 0; i < homeworksList.Count; i++)
            Debug.Log(homeworksList[i].homeworkName);
        for(int i = 0; i <= 1; i++)
        {
            if (inputFields[i].text == "")
            {
                ShowToast("Please complete the name and the subject", 2);
                return;
            }
        }
        Debug.Log("Try to enter addHomework");
        AddHomework(inputFields[0].text, inputFields[1].text, inputFields[2].text);
        addPannelFull.SetActive(false);
        for(int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].text = "";
        }
    }
    private void ShowToast(string text, int duration)
    {
        StartCoroutine(showToastCOR(text, duration));
    }
    private IEnumerator showToastCOR(string text, int duration)
    {
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
            targetText.color = new Color(255, 0, 0, alpha);
            yield return null;
        }
    }
    private void AddHomework(string name,string subject,string description)
    {
        Debug.Log("Start add homework");
        GameObject homeworkObject = Instantiate(homeworkPrefab, content);
        //homeworkObject.GetComponent<RectTransform>().SetAsLastSibling();
        int index = homeworksList.Count;

        Homework homeworkScript = homeworkObject.GetComponent<Homework>();
        homeworkScript.SetInfo(name, subject, description, index);
        homeworksList.Add(homeworkScript.homeworkModel);
        Debug.Log("a add the homework so now we have: ");
        for (int i = 0; i < homeworksList.Count; i++)
            Debug.Log(homeworksList[i].homeworkName);
        RefreshHomeworksList(homeworksList);
        Button[] buttons = homeworkObject.GetComponentsInChildren<Button>();
        HomeworkModel homeworkModel = homeworkScript.homeworkModel;
        buttons[0].onClick.AddListener(delegate { DestroyHomework(homeworkObject, homeworkModel); });
        buttons[1].onClick.AddListener(delegate { ShowHomework(homeworkModel.homeworkName, homeworkModel.homeworkSubject, homeworkModel.homeworkDescription); });
        Debug.Log("add homework ends");
    }
}
