using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
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
            count++;
            var text = type.ToString();
            localText.text = "\n" + count + " - " + text + " - " + condition + localText.text;
        }
    }
}
