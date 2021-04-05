using UnityEngine;
using UnityEngine.UI;

public class ShowPannelScript : MonoBehaviour
{
    public Text nameText;
    public Text subjectText;
    public Text descriptionText;
    public Text index;
    public void SetInfo(string hName, string subject, string description,int hIndex)
    {
        nameText.text = hName;
        subjectText.text = subject;
        descriptionText.text = description;
        this.index.text = hIndex.ToString();
    }
}