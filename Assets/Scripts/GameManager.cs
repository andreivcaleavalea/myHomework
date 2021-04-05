using System.Collections.Generic;
using UnityEngine;
using static FilesManager;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Notifications.Android;
using Utils;
using static Utils.AdsManager;
public class GameManager : MonoBehaviour
{
    private List<HomeworkModel> homeworksList;
    [SerializeField]private GameObject homeworkPrefab;

    private bool wichList = true;
    public List<GameObject> homeworks = new List<GameObject>();
    #region UI Elements
    [Header("UI Elements")]
    [SerializeField] private InputField[] inputFields;
    [SerializeField] private Transform content;
    [SerializeField]
    private GameObject
        titleObject,//normal
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
        backButtonHs,
        createButtonHs,
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
        var screenWidth = (int)titleObject.GetComponent<RectTransform>().rect.width;
        backButtonHs.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenWidth / 2 - 2);
        createButtonHs.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenWidth / 2 - 1);
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
            foreach (var homework in homeworksList)
            {
                var homeworkObject = Instantiate(homeworkPrefab, content);
                homeworks.Add(homeworkObject);
                var homeworkScript = homeworkObject.GetComponent<Homework>();
                homeworkScript.SetInfo(
                    homework.homeworkName,
                    homework.homeworkSubject,
                    homework.homeworkDescription,
                    homework.homeworkIndex,
                    homework.doneHomework
                );
                var homeworkModel = homeworkScript.homeworkModel;
                var buttons = homeworkObject.GetComponentsInChildren<Button>();
                buttons[0].onClick.AddListener(delegate { DoneHomework(homeworkObject, homeworkModel); });
                buttons[1].onClick.AddListener(delegate { ShowHomework(homeworkModel.homeworkName, homeworkModel.homeworkSubject, homeworkModel.homeworkDescription,homeworkModel.homeworkIndex); });
                homeworkScript.DoneHomework(homework.doneHomework);
            }
        }
        wichList = true;
        SwitchList();
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
    private void ShowHomework(string hName,string subject, string description, int index)
    {
        showPannelFull.SetActive(true);
        showPannel.GetComponent<ShowPannelScript>().SetInfo(hName, subject, description, index);
    }
    private static void CreateChannel()
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
        var notification = new AndroidNotification
        {
            Title = "myHomework",
            Text = "Hello user, you have " + homeworksList.Count + " homework to do!",
            FireTime = System.DateTime.Now.AddHours(5),
            SmallIcon = "icon_0",
            LargeIcon = "icon_1"
        };
        AndroidNotificationCenter.SendNotification(notification, "HaveHomeworks");

    }
    public void CreateHomework()
    {
        for(var i = 0; i <= 1; i++)
        {
            if (inputFields[i].text != "") continue;
            var instantiate = Instantiate(toastMessage, addPannel.transform);
            instantiate.transform.SetAsFirstSibling();
            addPannelText.transform.SetAsFirstSibling();
            toastMessage.GetComponent<ToastMessage>().SetText("Please complete the title and the subject");
            return;
        }
        AddHomework(inputFields[0].text, inputFields[1].text, inputFields[2].text);
        addPannelFull.SetActive(false);
        foreach (var inputField in inputFields)
        {
            inputField.text = "";
        }
    }
    private void AddHomework(string hName,string subject,string description)
    {
        var homeworkObject = Instantiate(homeworkPrefab, content);
        homeworks.Add(homeworkObject);
        //homeworkObject.GetComponent<RectTransform>().SetAsLastSibling();
        var index = homeworksList.Count;

        var homeworkScript = homeworkObject.GetComponent<Homework>();
        homeworkScript.SetInfo(hName, subject, description, index, false);
        homeworkScript.doneLayout.SetActive(false);
        homeworksList.Add(homeworkScript.homeworkModel);
        RefreshHomeworksList(homeworksList);
        var buttons = homeworkObject.GetComponentsInChildren<Button>();
        var homeworkModel = homeworkScript.homeworkModel;
        homeworkScript.DoneHomework(false);
        buttons[0].onClick.AddListener(delegate { DoneHomework(homeworkObject, homeworkModel); });
        buttons[1].onClick.AddListener(delegate { ShowHomework(homeworkModel.homeworkName, homeworkModel.homeworkSubject, homeworkModel.homeworkDescription,homeworkModel.homeworkIndex); });
    }
    public void DestroyHomework()
    {
        var index = int.Parse(indexText.GetComponent<Text>().text);
        homeworksList.RemoveAt(index);
        RefreshHomeworksList(homeworksList);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void ThemePurpleActivated()
    {
        ColorUtility.TryParseHtmlString("#8F00FF", out var normalColor);
        ColorUtility.TryParseHtmlString("#DB76FF", out var whiteAccentColor);
        ColorUtility.TryParseHtmlString("#7100A7", out var darkAccentColor);
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
        homeworkPrefab.GetComponent<Image>().color = whiteAccentColor;
        subjectToShow.GetComponent<Image>().color = darkAccentColor;
        descriptionToShowContainer.GetComponent<Image>().color = darkAccentColor;
        backButton.GetComponent<Image>().color = whiteAccentColor;
        nameInputBody.GetComponent<Image>().color = darkAccentColor;
        subjectInputBody.GetComponent<Image>().color = darkAccentColor;
        descriptionInputBody.GetComponent<Image>().color = darkAccentColor;
        settingsPannel.GetComponent<Image>().color = normalColor;
        var texts = homeworkPrefab.GetComponentsInChildren<Text>();
        foreach(var text in texts)
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
        ColorUtility.TryParseHtmlString("#c4c4c4", out var normalColor);
        ColorUtility.TryParseHtmlString("#a5a5a5", out var whiteAccentColor);
        ColorUtility.TryParseHtmlString("#686868", out var darkAccentColor);
        ColorUtility.TryParseHtmlString("#999999", out var mediumGrey);

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
        settingsPannel.GetComponent<Image>().color = darkAccentColor;
        settingsName.GetComponent<Image>().color = mediumGrey;
        settingsBackButton.GetComponent<Image>().color = normalColor;
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
    public void SwitchList()
    {
        wichList = !wichList;
        foreach(var homework in homeworks)
        {
            if (homework.GetComponent<Homework>().homeworkModel.doneHomework)
                homework.SetActive(wichList);
            else
                homework.SetActive(!wichList);
        }
    }
}
