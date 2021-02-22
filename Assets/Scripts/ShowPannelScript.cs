using UnityEngine;
using UnityEngine.UI;

public class ShowPannelScript : MonoBehaviour
{
    public Text nameText;
    public Text subjectText;
    public Text descriptionText;
    public void SetInfo(string name, string subject, string description)
    {
        nameText.text = name;
        subjectText.text = subject;
        descriptionText.text = "      " + description;
    }
}