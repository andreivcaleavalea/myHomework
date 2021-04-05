using UnityEngine;
using Utils;

public class TestingScript : MonoBehaviour
{
    public GameObject toastMessage;
    public Transform canvas;
    public void ToastMessageGive(string text)
    {
        var instantiate = Instantiate(toastMessage,canvas);
        instantiate.GetComponent<ToastMessage>().SetText(text);
    }
}
