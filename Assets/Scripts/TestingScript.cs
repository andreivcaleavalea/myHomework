using UnityEngine;
public class TestingScript : MonoBehaviour
{
    public GameObject toastMessage;
    public Transform canvas;
    public void ToastMessageGive(string text)
    {
        GameObject gameObject = Instantiate(toastMessage,canvas);
        gameObject.GetComponent<ToastMessage>().SetText(text);
    }
}
