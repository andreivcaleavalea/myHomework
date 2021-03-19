using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class ToastMessage : MonoBehaviour
{
    private Image image;
    private float counter;
    private float counterMax = 3f;
    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        counter = 0f;
    }
    private void Update()
    {
        counter += Time.deltaTime;
        if (counter > counterMax) 
        {
            Destroy(gameObject);
        }
    }
    public void SetText(string text)
    {
        gameObject.GetComponentInChildren<Text>().text = text;
    }
}
