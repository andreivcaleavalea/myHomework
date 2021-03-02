using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;

public class TestingScript : MonoBehaviour
{
    public GameObject gameObject;
    private void Start()
    {
        gameObject.GetComponent<Image>().color = new Color(256, 0, 0);
    }
    public void FixedUpdate()
    {

        gameObject.GetComponent<Image>().color=new Color(System.Random.Range()
    }
}
