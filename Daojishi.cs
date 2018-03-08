using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Daojishi : MonoBehaviour
{
    private float daoJiShiTime = 0;
    public Image filledImage;
    // Use this for initialization
    void Start()
    {
        // filledImage = transform.Find("moshi_bukehuishou_filled").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (daoJiShiTime <= 5f)
        {
            filledImage.fillAmount = 1 - daoJiShiTime / 5;
            daoJiShiTime += Time.deltaTime;
        }
        else
        {
            daoJiShiTime = 0;
        }
    }
}
