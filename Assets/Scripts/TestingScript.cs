using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;
public class TestingScript : MonoBehaviour
{
    #region Declarations
    public Text text;
    public Button button;
    #endregion
    private void Start()
    {
        button.onClick.AddListener(delegate { ShowToast("hei", 1); });
        text.color = new Vector4(255, 0, 0);
        CreateChannel();
        SendNotification();
    }
    private void CreateChannel()
    {
        var not = new AndroidNotificationChannel()
        {
            Id = "notification",
            Name = "First",
            Importance = Importance.High,
            Description = "bla bla bla",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(not);
    }
    private void SendNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = "Test";
        notification.Text = "Helloo man";
        notification.FireTime = System.DateTime.Now.AddSeconds(5);
        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";
        Color color = new Color(255, 0, 0);
        notification.Color = color;
        AndroidNotificationCenter.SendNotification(notification, "notification");
        Debug.Log("Notification sent..");
    }
    private void ShowToast(string text, int duration)
    {
        StartCoroutine(showToastCOR(text, duration));
    }
    private IEnumerator showToastCOR(string text, int duration)
    {
        /*Color originalColor = this.text.color;*/
        Color originalColor = new Vector4(255, 0, 0);
        this.text.text = text;
        this.text.enabled = true;
        yield return FadeInAndOut(this.text, true, 0.5f);

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        yield return FadeInAndOut(this.text, false, 0.5f);
        this.text.enabled = false;
        this.text.color = originalColor;

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
}
