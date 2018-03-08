using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GUImanager : MonoBehaviour
{
    public AudioClip AUGameOver;
    public AudioSource AUBG;
    public GameObject Player;
    public GameObject Main;
    public GameObject Begin;
    public GameObject Over;
    public Text Score;
    public Text Kehuishou_Score;
    public Text Bukehuishou_Score;
    public Text XKehuishou_Score;
    public Text XBukehuishou_Score;
    private int kehuishou_score;
    private int bukehuishou_score;
    private int xkehuishou_score;
    private int xbukehuishou_score;
    public GameObject Pingjia_jiayou;
    public GameObject Pingjia_biaoyang;
    public static bool bOkToRestart=false;
    public Image kuQi;
    public Image[] pingJia;
    //Return the Instance

    // Use this for initialization
    void Start()
    {
       Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
            if (pengzhuang.hp == 0)
            {
                playerMovement.RunDet = false;
                Time.timeScale = 0;
                bOkToRestart = true;
                kehuishou_score = pengzhuang.kehuishou_number * 100;
                bukehuishou_score = pengzhuang.bukehuishou_number * 100;
                xkehuishou_score = pengzhuang.xkehuishou_number * 100;
                xbukehuishou_score = pengzhuang.xbukehuishou_number * 100;
                Kehuishou_Score.text =  kehuishou_score.ToString();
                Bukehuishou_Score.text =  bukehuishou_score.ToString();
                XKehuishou_Score.text = xkehuishou_score.ToString();
                XBukehuishou_Score.text = xbukehuishou_score.ToString();
                Score.text = pengzhuang.score.ToString();
                if (pengzhuang.score <= 0)
                    kuQi.gameObject.SetActive(true);
                else
                {
                    int i = pengzhuang.score / 500;
                    for (int j = 0; j <= i; j++)
                    {
                        pingJia[j].gameObject.SetActive(true);
                    }
                }
                DisPlayGameOver();
            }
            else
            {
                bOkToRestart = false;
                kuQi.gameObject.SetActive(false);
                for (int i = 0; i < 5; i++)
                {
                    if(pingJia[i].gameObject.activeSelf)
                    pingJia[i].gameObject.SetActive(false);
                }
            }
    }
    public void DisPlayMain()//当点击开始按钮的时候触发这个事件
    {
        Begin.SetActive(false);
        Main.SetActive(true);
        Time.timeScale = 1;
    }
    void DisPlayGameOver()
    {
        Main.SetActive(false);
        Over.SetActive(true);
        AUBG.Stop();
        AudioSource.PlayClipAtPoint(AUGameOver, Player.transform.position);
    }
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
