using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class ToastMessage : MonoBehaviour
    {
        private float counter;
        private const float CounterMax = 3f;

        private void Start()
        {
            counter = 0f;
        }
        private void Update()
        {
            counter += Time.deltaTime;
            if (counter > CounterMax) 
            {
                Destroy(gameObject);
            }
        }
        public void SetText(string text)
        {
            gameObject.GetComponentInChildren<Text>().text = text;
        }
    }
}
