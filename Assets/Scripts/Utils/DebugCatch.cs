using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCatch : MonoBehaviour
{
    public Text localText;
    private int count = 0;
    private void OnEnable()
    {
        Application.logMessageReceived += Application_logMessageReceived;
    }
    private void OnDisable()
    {
        Application.logMessageReceived -= Application_logMessageReceived;
    }
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        var text = string.Empty;
        count++;
        text = type.ToString();
        localText.text = "\n" + count + " - " + text + " - " + condition + localText.text;
    }
}
