using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeCountDown : MonoBehaviour
{
    private float CountDownTime = 0;
    public Image filledImage;
    // Use this for initialization
    void Start()
    {
        // filledImage = transform.Find("moshi_bukehuishou_filled").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CountDownTime <= 5f)
        {
            filledImage.fillAmount = 1 - CountDownTime / 5;
            CountDownTime += Time.deltaTime;
        }
        else
        {
            CountDownTime = 0;
        }
    }
}
