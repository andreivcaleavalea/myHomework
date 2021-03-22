using System.Collections.Generic;
using UnityEngine;
using static FilesManager;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Notifications.Android;
using static AdsManager;
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
        addPannel,
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
        settingsPannel,
        toastMessage,
        addPannelText,
        settingsName,
        settingsBackButton,
        removeButton,
        indexText,
        backButton;//light
#endregion

    private void Start()
    {
        InitializeElements();
        CreateChannel();
        SendNotification();
    }
    private void InitializeElements()
    {
        InitializeAds();
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
        if (PlayerPrefs.GetString("theme") == "purple")
            ThemePurpleActivated();
        else
            ThemeGreyActivated();
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
                    homeworksList[i].homeworkIndex,
                    homeworksList[i].doneHomework
                    );
                HomeworkModel homeworkModel = homeworkScript.homeworkModel;
                Button[] buttons = homeworkObject.GetComponentsInChildren<Button>();
                buttons[0].onClick.AddListener(delegate { DoneHomework(homeworkObject, homeworkModel); });
                buttons[1].onClick.AddListener(delegate { ShowHomework(homeworkModel.homeworkName, homeworkModel.homeworkSubject, homeworkModel.homeworkDescription,homeworkModel.homeworkIndex); });
                if (homeworksList[i].doneHomework)
                    homeworkScript.DoneHomework(true);
                else
                    homeworkScript.DoneHomework(false);
            }
        }
    }
    private void DoneHomework(GameObject homeworkObject, HomeworkModel homeworkModel)
    {
        homeworkObject.GetComponent<Homework>().doneLayout.SetActive(true);
        if (!homeworkModel.doneHomework)
            homeworkObject.GetComponent<Homework>().DoneHomework(true);
        homeworkModel.doneHomework = true;
        homeworksList[homeworkModel.homeworkIndex].doneHomework = true;
        RefreshHomeworksList(homeworksList);
    }
    private void ShowHomework(string name,string subject, string description, int index)
    {
        showPannelFull.SetActive(true);
        showPannel.GetComponent<ShowPannelScript>().SetInfo(name, subject, description, index);
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
        for(int i = 0; i <= 1; i++)
        {
            if (inputFields[i].text == "")
            {
                GameObject gameObject = Instantiate(toastMessage, addPannel.transform);
                gameObject.transform.SetAsFirstSibling();
                addPannelText.transform.SetAsFirstSibling();
                toastMessage.GetComponent<ToastMessage>().SetText("Please complete the title and the subject");
                return;
            }
        }
        AddHomework(inputFields[0].text, inputFields[1].text, inputFields[2].text);
        addPannelFull.SetActive(false);
        for(int i = 0; i < inputFields.Length; i++)
        {
            inputFields[i].text = "";
        }
    }
    private void AddHomework(string name,string subject,string description)
    {
        GameObject homeworkObject = Instantiate(homeworkPrefab, content);
        //homeworkObject.GetComponent<RectTransform>().SetAsLastSibling();
        int index = homeworksList.Count;

        Homework homeworkScript = homeworkObject.GetComponent<Homework>();
        homeworkScript.SetInfo(name, subject, description, index, false);
        homeworkScript.doneLayout.SetActive(false);
        homeworksList.Add(homeworkScript.homeworkModel);
        RefreshHomeworksList(homeworksList);
        Button[] buttons = homeworkObject.GetComponentsInChildren<Button>();
        HomeworkModel homeworkModel = homeworkScript.homeworkModel;
        homeworkScript.DoneHomework(false);
        buttons[0].onClick.AddListener(delegate { DoneHomework(homeworkObject, homeworkModel); });
        buttons[1].onClick.AddListener(delegate { ShowHomework(homeworkModel.homeworkName, homeworkModel.homeworkSubject, homeworkModel.homeworkDescription,homeworkModel.homeworkIndex); });
    }
    public void DestroyHomework()
    {
        int index = int.Parse(indexText.GetComponent<Text>().text);
        homeworksList.RemoveAt(index);
        RefreshHomeworksList(homeworksList);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void ThemePurpleActivated()
    {
        Color normalColor;
        Color whiteAccentColor;
        Color darkAccentColor;
        ColorUtility.TryParseHtmlString("#8F00FF", out normalColor);
        ColorUtility.TryParseHtmlString("#DB76FF", out whiteAccentColor);
        ColorUtility.TryParseHtmlString("#7100A7", out darkAccentColor);
        titleObject.GetComponent<Image>().color = normalColor;
        addButton.GetComponent<Image>().color = normalColor;
        addPannelFull.GetComponent<Image>().color = normalColor;
        showPannel.GetComponent<Image>().color = normalColor;
        nameInputHead.GetComponent<Image>().color = whiteAccentColor;
        subjectInputHead.GetComponent<Image>().color = whiteAccentColor;
        descriptionInputHead.GetComponent<Image>().color = whiteAccentColor;
        backCreateButton.GetComponent<Image>().color = whiteAccentColor;
        nameToShow.GetComponent<Image>().color = whiteAccentColor;
        descriptionToShowTitle.GetComponent<Image>().color = whiteAccentColor;
        homeworkPrefab.GetComponent<Image>().color = normalColor;
        subjectToShow.GetComponent<Image>().color = darkAccentColor;
        descriptionToShowContainer.GetComponent<Image>().color = darkAccentColor;
        backButton.GetComponent<Image>().color = whiteAccentColor;
        nameInputBody.GetComponent<Image>().color = darkAccentColor;
        subjectInputBody.GetComponent<Image>().color = darkAccentColor;
        descriptionInputBody.GetComponent<Image>().color = darkAccentColor;
        settingsPannel.GetComponent<Image>().color = normalColor;
        Text[] texts = homeworkPrefab.GetComponentsInChildren<Text>();
        foreach(Text text in texts)
        {
            text.color = new Color(256, 256, 256);
        }
        descriptionToShowContainer.GetComponentInChildren<Text>().color = new Color(256, 256, 256);
        settingsName.GetComponent<Image>().color = whiteAccentColor;
        settingsBackButton.GetComponent<Image>().color = whiteAccentColor;
        toastMessage.GetComponent<Image>().color = whiteAccentColor;
        removeButton.GetComponent<Image>().color = whiteAccentColor;
    }
    private void ThemeGreyActivated()
    {
        Color normalColor;
        Color whiteAccentColor;
        Color darkAccentColor;
        Color mediumGrey;
        ColorUtility.TryParseHtmlString("#c4c4c4", out normalColor);
        ColorUtility.TryParseHtmlString("#a5a5a5", out whiteAccentColor);
        ColorUtility.TryParseHtmlString("#686868", out darkAccentColor);
        ColorUtility.TryParseHtmlString("#999999", out mediumGrey);

        titleObject.GetComponent<Image>().color = darkAccentColor;
        addButton.GetComponent<Image>().color = darkAccentColor;
        addPannelFull.GetComponent<Image>().color = darkAccentColor;
        nameInputHead.GetComponent<Image>().color = normalColor;
        nameInputBody.GetComponent<Image>().color = whiteAccentColor;
        subjectInputHead.GetComponent<Image>().color = normalColor;
        subjectInputBody.GetComponent<Image>().color = whiteAccentColor;
        descriptionInputHead.GetComponent<Image>().color = normalColor;
        descriptionInputBody.GetComponent<Image>().color = whiteAccentColor;
        backCreateButton.GetComponent<Image>().color = normalColor;
        showPannel.GetComponent<Image>().color = darkAccentColor;
        nameToShow.GetComponent<Image>().color = normalColor;
        subjectToShow.GetComponent<Image>().color = mediumGrey;
        descriptionToShowTitle.GetComponent<Image>().color = mediumGrey;
        descriptionToShowContainer.GetComponent<Image>().color = normalColor;
        homeworkPrefab.GetComponent<Image>().color = whiteAccentColor;
        settingsPannel.GetComponent<Image>().color = normalColor;
        settingsName.GetComponent<Image>().color = mediumGrey;
        settingsBackButton.GetComponent<Image>().color = mediumGrey;
        toastMessage.GetComponent<Image>().color = whiteAccentColor;
        removeButton.GetComponent<Image>().color = whiteAccentColor;
    }
    public void ThemePurple()
    {
        PlayerPrefs.SetString("theme", "purple");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ThemeGrey()
    {
        PlayerPrefs.SetString("theme", "grey");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
