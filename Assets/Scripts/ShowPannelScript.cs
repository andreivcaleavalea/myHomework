using UnityEngine;
using UnityEngine.UI;

public class ShowPannelScript : MonoBehaviour
{
    public Text nameText;
    public Text subjectText;
    public Text descriptionText;
    public Text index;
    public void SetInfo(string name, string subject, string description,int index)
    {
        nameText.text = name;
        subjectText.text = subject;
        descriptionText.text = description;
        this.index.text = index.ToString();
    }
}