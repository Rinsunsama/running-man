using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class pengzhuang : MonoBehaviour{
    public GameObject moShiBuKeHuiShou;
    public GameObject moShiKeHuiShou;  
    public GameObject ControlLight;
    public AudioSource AuBG;
    public AudioClip AuZhangai;
    public AudioClip AUShifen;
    public AudioClip AuDefen;
    public AudioClip AuDaojishi;
    bool bAuDaojiShi = true;
    public AudioClip AuDaoJiShiMoWei;
    bool bAuDaoJiShiMoWei = true;
    public GameObject[] Hp = new GameObject[5];
    static public int hp = 5;
    static public int score = 0;
    private float now_Time = 0f;
    bool ifkehuishou = true;
    bool yugao_Moshi = true;
    bool yugao_MoshiB = false;
    public Text Kehuishou_number;
    public Text Bukehuishou_number;
    public Text Xkehuishou_number;
    public Text Xbukehuishou_number;
    public Text tCuoWuTiShi;
    public float timeForTips=2;
    bool btimerForTips=false;
    public GameObject pForTips;
    public GameObject Main;
    public GameObject Over;
    static public int kehuishou_number = 0;
    static public int bukehuishou_number = 0;
    static public int xkehuishou_number = 0;
    static public int xbukehuishou_number = 0;
    //-------------为障碍碎片建造一个数组
    public GameObject[] Suipianpref;
    GameObject[] Suipian = new GameObject[100];
    public GameObject[] zhangai_kind;
    public  GameObject[] laji_kind;
    public GameObject Moshi_Kehuishou_UI;
    public GameObject Moshi_Bukehuishou_UI;
    void Start()
    {

        Kehuishou_number.text = "X 0";
        Bukehuishou_number.text = "X 0";
        Xkehuishou_number.text = "X 0";
        Xbukehuishou_number.text = "X 0";
        int x = Random.Range(0, 2);
        if (x == 0)
        {
            ifkehuishou = true;
         //   show_Moshi.text = "请回收可回收垃圾";
            Moshi_Kehuishou_UI.SetActive(true);
            ControlLight.GetComponent<Light>().color = Color.white;
        }
        else
        {
            ifkehuishou = false;
          //  show_Moshi.text = "请回收不可回收垃圾";
            Moshi_Bukehuishou_UI.SetActive(true);
            Moshi_Kehuishou_UI.SetActive(false);
            ControlLight.GetComponent<Light>().color = Color.yellow;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hp<0)
        {
            animation.Stop();
        }
        if (playerMovement.bOkToRestartAndRunDet)//2s 后再次检测到跑步
        {
            Restart();
            playerMovement.bOkToRestartAndRunDet = false;
        } now_Time += Time.deltaTime;
        if (now_Time >= 15f)
        {
            if (!yugao_MoshiB)
            {
                int x = Random.Range(0, 2);
                if (x == 0)
                {
                    yugao_Moshi = true;
                    moShiKeHuiShou.SetActive(true);
                }
                else
                {
                    yugao_Moshi = false;
                    moShiBuKeHuiShou.SetActive(true);
                }
                yugao_MoshiB = true;
            }
             //----------------------------------------------------------倒计时音效
                AuBG.volume = 0.2f;
                if (bAuDaojiShi)
               {
                AudioSource.PlayClipAtPoint(AuDaojishi, transform.position);
                bAuDaojiShi = false;
               }
                if(now_Time>=19.8f)
                {
                     if (bAuDaoJiShiMoWei)
                  {
                    AudioSource.PlayClipAtPoint(AuDaoJiShiMoWei, transform.position);
                    bAuDaoJiShiMoWei = false;
                  }
                }
            }
        else
        {
            AuBG.volume = 1f;
            if (moShiKeHuiShou.activeSelf)
                moShiKeHuiShou.SetActive(false);
            if (moShiBuKeHuiShou.activeSelf)
                moShiBuKeHuiShou.SetActive(false);
            bAuDaojiShi = true;
            bAuDaoJiShiMoWei = true;
            yugao_MoshiB = false;
        }


        if (now_Time >= 20f)
        {
            //  daoJiShi[0].SetActive(false);
            ifkehuishou = yugao_Moshi;
            if (ifkehuishou)
            {
              //  show_Moshi.text = "请回收可回收垃圾";
                ControlLight.GetComponent<Light>().color = Color.white;
                Moshi_Kehuishou_UI.SetActive(true);
                Moshi_Bukehuishou_UI.SetActive(false);
            }
            else
            {
            //    show_Moshi.text = "请回收不可回收垃圾";
                ControlLight.GetComponent<Light>().color = Color.yellow;
                Moshi_Kehuishou_UI.SetActive(false);
                Moshi_Bukehuishou_UI.SetActive(true);
            }
            now_Time = 0;
        }
        if(btimerForTips)//错误提示字体将在2s后消失
        {
      
            timeForTips += Time.deltaTime;
            if (timeForTips >= 5)
            {
                pForTips.SetActive(false);
                btimerForTips = false;
                timeForTips = 0;
            }
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (ifkehuishou)
        {
            if (collision.transform.tag == "kehuishoulaji")
            {
                AudioSource.PlayClipAtPoint(AuDefen, this.transform.position);
                Destroy(collision.gameObject);
                kehuishou_number += 1;
                Kehuishou_number.text = "X " + kehuishou_number;
            }
            else if (collision.transform.tag == "bukehuishoulaji")//---------此处要添加提示
            {
                AudioSource.PlayClipAtPoint(AUShifen, this.transform.position);
                Destroy(collision.gameObject);
                xbukehuishou_number += 1;
                Xbukehuishou_number.text = "X " + xbukehuishou_number;
                Tips(collision);
                pForTips.SetActive(true);
                btimerForTips = true;
               
            }
        }
        else
        {
            if (collision.transform.tag == "kehuishoulaji")//----------------此处要添加提示
            {
                Destroy(collision.gameObject);
                xkehuishou_number += 1;
                Xkehuishou_number.text = "X " + xkehuishou_number;
                AudioSource.PlayClipAtPoint(AUShifen, this.transform.position);
                Tips(collision);
                pForTips.SetActive(true);
                btimerForTips = true;
            }
            else if (collision.transform.tag == "bukehuishoulaji")
            {
                Destroy(collision.gameObject);
                bukehuishou_number += 1;
                Bukehuishou_number.text = "X " + bukehuishou_number;
                AudioSource.PlayClipAtPoint(AuDefen, this.transform.position);
            }
        }
        if (collision.transform.tag == "zhangai")                 //如果人物碰到的是障碍，添加一个粉碎特效
        {
            AudioSource.PlayClipAtPoint(AuZhangai, this.transform.position);
            //-------添加粉碎特效
            Transform position = collision.transform;         //记录将粉碎的障碍的位置
            Destroy(collision.gameObject);
            for (int i = 0; i < 4; i++)
            {
                if (position.transform.position.y == zhangai_kind[i].transform.position.y)
                    for (int j = 0; j < 10; j++)
                        if (Suipianpref[i * 10 + j] != null)
                        {
                            Suipian[j] = Instantiate(Suipianpref[i * 10 + j], position.transform.position, transform.rotation) as GameObject;
                            Destroy(Suipian[j], 2.0f);
                        }
            }
            hp--;
            Hp[hp].SetActive(false);
            if (hp == 0)
            score = kehuishou_number * 100 + bukehuishou_number * 100 + xkehuishou_number * -100 + xbukehuishou_number * -100;
        }
    }
    public void Restart()//重新开始游戏,点击重新开始游戏按钮会触发的函数
    {
      
        hp = 5;
        for (int i = 0; i < 5; i++)
            Hp[i].SetActive(true);
        xkehuishou_number = 0;
        kehuishou_number = 0;
        bukehuishou_number = 0;
        xbukehuishou_number = 0;
        Kehuishou_number.text = "X " + kehuishou_number;
        Xbukehuishou_number.text = "X " + xbukehuishou_number;
        Xkehuishou_number.text = "X " + xkehuishou_number;
        Bukehuishou_number.text = "X " + bukehuishou_number; 
        Over.SetActive(false);
        Main.SetActive(true);
        Time.timeScale = 1;
        animation.Stop();
        AuBG.Play();
    }

    public void Tips(Collision go)
    {
        if(go.gameObject.transform.position.y==laji_kind[0].transform.position.y)
        {
            tCuoWuTiShi.text = "纸箱，报纸等是可回收垃圾";
        }
        if (go.gameObject.transform.position.y == laji_kind[1].transform.position.y)
        {
            tCuoWuTiShi.text = "纸箱，报纸等是可回收垃圾"; 
        } if (go.gameObject.transform.position.y == laji_kind[2].transform.position.y)
        {
            tCuoWuTiShi.text = "可乐瓶，饮料瓶等是可回收垃圾";
        } if (go.gameObject.transform.position.y == laji_kind[3].transform.position.y)
        {
            tCuoWuTiShi.text = "可乐瓶，饮料瓶等是可回收垃圾";
        } if (go.gameObject.transform.position.y == laji_kind[4].transform.position.y)
        {
            tCuoWuTiShi.text = "电池是可回收垃圾";
        } if (go.gameObject.transform.position.y == laji_kind[5].transform.position.y)
        {
            tCuoWuTiShi.text = "塑料袋是可回收垃圾";
        } if (go.gameObject.transform.position.y == laji_kind[6].transform.position.y)
        {
            tCuoWuTiShi.text = "骨头菜叶等厨余垃圾是不可回收垃圾";
        } if (go.gameObject.transform.position.y == laji_kind[7].transform.position.y)
        {
            tCuoWuTiShi.text = "骨头菜叶等厨余垃圾是不可回收垃圾";
        } if (go.gameObject.transform.position.y == laji_kind[8].transform.position.y)
        {
            tCuoWuTiShi.text = "香蕉，果皮等是不可回收垃圾哦";
        } 
    }

}

