using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestingScript : MonoBehaviour
{
    public Text text;
    public Button button;
    private void Start()
    {
        button.onClick.AddListener(delegate { ShowToast("hei", 1); });
        text.color = new Vector4(255, 0, 0);
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
