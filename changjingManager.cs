using UnityEngine;
using System.Collections;

public class changjingManager : MonoBehaviour
{
    int lajihouxu;
    static int zhangaiNumber = 20;
    static int LajiNumber = 50;
    public int Leftborder = 14;
    public int Righborder = 16;
    public int SceneLength = 50;
    public float SceneWideth = 30;
    private GameObject scene1;
    private GameObject scene2;
    private GameObject scene3;
    public GameObject scene_prefab;
    public GameObject player;
    float laji_pos_z;
    int laji_pos_x;
    int laji_kind_number;
    int zhangai_kind_number;
    GameObject[] laji = new GameObject[LajiNumber + 50]; // 用来存储产生的垃圾
    GameObject[] zhangai = new GameObject[zhangaiNumber];//用来存储产生的障碍
    GameObject cuncuqi1;              //用来作为生成物的父物体，即储存器，方便清除。
    GameObject cuncuqi2;
    GameObject cuncuqi3;
    public GameObject empty;        //储存器的预设，为空物体
    public GameObject[] laji_kind;// 垃圾类型
    public GameObject[] zhangai_kind;//障碍类型
    bool bOkToNewLaJi=false;
    bool bOkToNewDiXing = false;
    void Start()
    {
        //起始先生成2个地形
        scene1 = GameObject.Instantiate(scene_prefab, new Vector3(scene_prefab.transform.position.x, scene_prefab.transform.position.y, scene_prefab.transform.position.z), transform.rotation) as GameObject;
        cuncuqi1 = GameObject.Instantiate(empty, new Vector3(0, 0, 0), transform.rotation) as GameObject;

        //先随机产生障碍  
        for (int j = 0; j < zhangaiNumber; j++)
        {
            laji_pos_x = Random.Range(Leftborder, Righborder + 1);//Random 只能为整数
            laji_pos_z = Random.Range(10, SceneLength);
            zhangai_kind_number = Random.Range(0, 4);
            if (zhangai_kind_number == 1 || zhangai_kind_number == 3)
            {

                zhangai[j] = Instantiate(zhangai_kind[zhangai_kind_number], new Vector3(15, zhangai_kind[zhangai_kind_number].transform.position.y, laji_pos_z), zhangai_kind[zhangai_kind_number].transform.rotation) as GameObject;
                zhangai[j].transform.parent = cuncuqi1.transform;
            }
            else
            {
                // Debug.Log("生成了");
                zhangai[j] = Instantiate(zhangai_kind[zhangai_kind_number], new Vector3(laji_pos_x, zhangai_kind[zhangai_kind_number].transform.position.y, laji_pos_z), zhangai_kind[zhangai_kind_number].transform.rotation) as GameObject;
                zhangai[j].transform.parent = cuncuqi1.transform;
            }
        }
        //Destory10米内的障碍
        for (int k = 0; k < zhangaiNumber - 1; k++)
            for (int j = k + 1; j < zhangaiNumber; j++)
                // if (zhangai[k].transform.position.x == zhangai[j].transform.position.x)//不要求ｘ相同，更好玩一点
                // {
                if (Mathf.Abs(zhangai[k].transform.position.z - zhangai[j].transform.position.z) < 10)//10米之内的删除
                {
                    DestroyImmediate(zhangai[k].gameObject);
                    break;
                }
        //}
        //随机产生垃圾
        for (int i = 0; i < LajiNumber; i++)
        {
            // Debug.Log("why");
            laji_pos_x = Random.Range(Leftborder, Righborder + 1);//Random 只能为整数
            laji_pos_z = Random.Range(10, SceneLength);
            laji_kind_number = Random.Range(0, 9);
            laji[i] = Instantiate(laji_kind[laji_kind_number], new Vector3(laji_pos_x, laji_kind[laji_kind_number].transform.position.y, laji_pos_z), laji_kind[laji_kind_number].transform.rotation) as GameObject;
            //  Debug.Log("生成的垃圾数量");
           laji[i].transform.parent = cuncuqi1.transform;
        }
        //Destory5m以内的垃圾
        for (int k = 0; k < LajiNumber - 1; k++)
            for (int j = k + 1; j < LajiNumber; j++)
                if (Mathf.Abs(laji[k].transform.position.z - laji[j].transform.position.z) < 5)//5米之内的删除(在同一个轨道)
                {
                    DestroyImmediate(laji[k].gameObject);//立即销毁，不能用Destory;
                    //Debug.Log("Destory掉的垃圾数");
                    break;
                }

        //在垃圾的后面在随机生成0-4个垃圾
        lajihouxu = LajiNumber - 1;            //在已经删除相近的垃圾数组后面添加垃圾，这个是数组序号。
        for (int i = 0; i < LajiNumber; i++)
        {

            if (laji[i] != null)
            {
                // Debug.Log("剩下的垃圾数");
                int j = Random.Range(0, 5);
                for (int k = 0; k < j; k++)
                {
                    // Debug.Log("兄弟你为什么只输出一次啊");
                    lajihouxu++;
                    laji[lajihouxu] = Instantiate(laji[i], new Vector3(laji[i].transform.position.x, laji[i].transform.position.y, laji[i].transform.position.z + 1 + k), laji[i].transform.rotation) as GameObject;
                    laji[lajihouxu].transform.parent = cuncuqi1.transform;
                }
            }
        }

        //-----------清除靠的太近的物体  
        for (int k = 0; k < laji.Length; k++)
            for (int j = 0; j < zhangaiNumber; j++)
            {
                if (laji[k] && zhangai[j])
                    if (Mathf.Abs(laji[k].transform.position.z - zhangai[j].transform.position.z) < 0.95f)
                    {
                        //  Debug.Log("Destory靠的太近的obj");
                        Destroy(laji[k].gameObject);
                        break;
                    }
            }
        scene2 = GameObject.Instantiate(scene_prefab, new Vector3(scene_prefab.transform.position.x, scene_prefab.transform.position.y, scene_prefab.transform.position.z + SceneLength), transform.rotation) as GameObject;
       cuncuqi2 = GameObject.Instantiate(empty, new Vector3(0, 0, 0), transform.rotation) as GameObject;
        for (int j = 0; j < zhangaiNumber; j++)
        {

            laji_pos_x = Random.Range(Leftborder, Righborder + 1);//Random 只能为整数
            laji_pos_z = Random.Range(scene2.transform.position.z + 5, scene2.transform.position.z + SceneLength);
            zhangai_kind_number = Random.Range(0, 4);
            if (zhangai_kind_number == 1 || zhangai_kind_number == 3)
            {

                zhangai[j] = Instantiate(zhangai_kind[zhangai_kind_number], new Vector3(15, zhangai_kind[zhangai_kind_number].transform.position.y, laji_pos_z), zhangai_kind[zhangai_kind_number].transform.rotation) as GameObject;
                zhangai[j].transform.parent = cuncuqi2.transform;
            }
            else
            {
                // Debug.Log("生成了");
                zhangai[j] = Instantiate(zhangai_kind[zhangai_kind_number], new Vector3(laji_pos_x, zhangai_kind[zhangai_kind_number].transform.position.y, laji_pos_z), zhangai_kind[zhangai_kind_number].transform.rotation) as GameObject;
               zhangai[j].transform.parent = cuncuqi2.transform;
            }
        }
        //Destory10米内的障碍
        for (int k = 0; k < zhangaiNumber - 1; k++)
            for (int j = k + 1; j < zhangaiNumber; j++)
                //  if (zhangai[k].transform.position.x == zhangai[j].transform.position.x)
                // {
                if (Mathf.Abs(zhangai[k].transform.position.z - zhangai[j].transform.position.z) < 10)//10米之内的删除
                {

                    DestroyImmediate(zhangai[k].gameObject);
                    break;
                }
        //  }
        //随机产生垃圾
        for (int i = 0; i < LajiNumber; i++)
        {
            laji_pos_x = Random.Range(Leftborder, Righborder + 1);//Random 只能为整数
            laji_pos_z = Random.Range(scene2.transform.position.z + 5, scene2.transform.position.z + SceneLength);
            laji_kind_number = Random.Range(0, 9);
            laji[i] = Instantiate(laji_kind[laji_kind_number], new Vector3(laji_pos_x, laji_kind[laji_kind_number].transform.position.y, laji_pos_z), laji_kind[laji_kind_number].transform.rotation) as GameObject;
            //Debug.Log("生成的垃圾数量");
           laji[i].transform.parent = cuncuqi2.transform;
        }
        //Destory5m以内的垃圾
        for (int k = 0; k < LajiNumber - 1; k++)
            for (int j = k + 1; j < LajiNumber; j++)
                if (Mathf.Abs(laji[k].transform.position.z - laji[j].transform.position.z) < 5)//5米之内的删除(在同一个轨道)
                {
                    DestroyImmediate(laji[k].gameObject);//立即销毁，不能用Destory;
                    //   Debug.Log("Destory掉的垃圾数");
                    break;
                }

        //在垃圾的后面在随机生成0-4个垃圾
        lajihouxu = LajiNumber - 1;            //在已经删除相近的垃圾数组后面添加垃圾，这个是数组序号。
        for (int i = 0; i < LajiNumber; i++)
        {

            if (laji[i] != null)
            {
                // Debug.Log("剩下的垃圾数");
                int j = Random.Range(0, 5);
                for (int k = 0; k < j; k++)
                {
                    //Debug.Log("兄弟你为什么只输出一次啊");
                    lajihouxu++;
                    laji[lajihouxu] = Instantiate(laji[i], new Vector3(laji[i].transform.position.x, laji[i].transform.position.y, laji[i].transform.position.z + 1 + k), laji[i].transform.rotation) as GameObject;
                    laji[lajihouxu].transform.parent = cuncuqi2.transform;
                }
            }
        }
        //-----------清除靠的太近的物体
        for (int k = 0; k < laji.Length; k++)
            for (int j = 0; j < zhangaiNumber; j++)
            {
                if (laji[k] && zhangai[j])
                    if (Mathf.Abs(laji[k].transform.position.z - zhangai[j].transform.position.z) < 0.95f)
                    {
                        //   Debug.Log("Destory靠的太近的obj");
                        Destroy(laji[k].gameObject);
                        break;
                    }
            }
    }
    void Update()
    {
        //如果人物超过第一个地形，则销毁第一个地形，在第二个地形后方继续产生地形和障碍
        if (player.transform.position.z > scene1.transform.position.z + SceneLength+3)
        {
            bOkToNewLaJi = true;
            bOkToNewDiXing = true;
            //DestroyImmediate(scene1.gameObject);
            DestroyImmediate(cuncuqi1.gameObject);
            scene3 = scene1;
            scene1 = scene2;
            
        }
        if(player.transform.position.z>scene1.transform.position.z+10 && bOkToNewDiXing)
        {
            scene3.transform.position = new Vector3(scene3.transform.position.x, scene3.transform.position.y, scene3.transform.position.z + SceneLength*2);
           // scene2 = GameObject.Instantiate(scene_prefab, new Vector3(scene_prefab.transform.position.x, scene_prefab.transform.position.y, scene2.transform.position.z + SceneLength), transform.rotation) as GameObject;
            cuncuqi2 = GameObject.Instantiate(empty, new Vector3(0, 0, 0), transform.rotation) as GameObject;
            bOkToNewDiXing = false;
            scene2 = scene3;
        }
        if (player.transform.position.z > scene1.transform.position.z + 20 && bOkToNewLaJi )//尝试让地形销毁产生，垃圾产生分开进行。
            {
            
                Debug.Log("生成垃圾");
                for (int j = 0; j < zhangaiNumber; j++)
                {

                    laji_pos_x = Random.Range(Leftborder, Righborder + 1);//Random 只能为整数
                    laji_pos_z = Random.Range(scene2.transform.position.z + 5, scene2.transform.position.z + SceneLength);
                    zhangai_kind_number = Random.Range(0, 4);
                    if (zhangai_kind_number == 1 || zhangai_kind_number == 3)
                    {

                        zhangai[j] = Instantiate(zhangai_kind[zhangai_kind_number], new Vector3(15, zhangai_kind[zhangai_kind_number].transform.position.y, laji_pos_z), zhangai_kind[zhangai_kind_number].transform.rotation) as GameObject;
                      zhangai[j].transform.parent = cuncuqi2.transform;
                    }
                    else
                    {
                        // Debug.Log("生成了");
                        zhangai[j] = Instantiate(zhangai_kind[zhangai_kind_number], new Vector3(laji_pos_x, zhangai_kind[zhangai_kind_number].transform.position.y, laji_pos_z), zhangai_kind[zhangai_kind_number].transform.rotation) as GameObject;
                      zhangai[j].transform.parent = cuncuqi2.transform;
                    }
                }
                //Destory10米内的障碍
                for (int k = 0; k < zhangaiNumber - 1; k++)
                    for (int j = k + 1; j < zhangaiNumber; j++)
                        //  if (zhangai[k].transform.position.x == zhangai[j].transform.position.x)
                        //   {
                        if (Mathf.Abs(zhangai[k].transform.position.z - zhangai[j].transform.position.z) < 10)//10米之内的删除
                        {
                            DestroyImmediate(zhangai[k].gameObject);
                            break;
                        }
                //   }
                //随机产生垃圾
                for (int i = 0; i < LajiNumber; i++)
                {
                    laji_pos_x = Random.Range(Leftborder, Righborder + 1);//Random 只能为整数
                    laji_pos_z = Random.Range(scene2.transform.position.z + 5, scene2.transform.position.z + SceneLength);
                    laji_kind_number = Random.Range(0, 9);
                    laji[i] = Instantiate(laji_kind[laji_kind_number], new Vector3(laji_pos_x, laji_kind[laji_kind_number].transform.position.y, laji_pos_z), laji_kind[laji_kind_number].transform.rotation) as GameObject;
                   laji[i].transform.parent = cuncuqi2.transform;
                }
                for (int k = 0; k < LajiNumber - 1; k++)
                    for (int j = k + 1; j < LajiNumber; j++)
                        if (Mathf.Abs(laji[k].transform.position.z - laji[j].transform.position.z) < 5)//5米之内的删除(在同一个轨道)
                        {
                            DestroyImmediate(laji[k].gameObject);
                            break;
                        }
                lajihouxu = LajiNumber - 1;
                for (int i = 0; i < LajiNumber; i++)
                {

                    if (laji[i] != null)
                    {

                        int j = Random.Range(0, 5);
                        for (int k = 0; k < j; k++)
                        {

                            lajihouxu++;
                            laji[lajihouxu] = Instantiate(laji[i], new Vector3(laji[i].transform.position.x, laji[i].transform.position.y, laji[i].transform.position.z + 1 + k), laji[i].transform.rotation) as GameObject;
                           laji[lajihouxu].transform.parent = cuncuqi2.transform;
                        }
                    }
                }
                //-----------清除靠的太近的物体
                for (int k = 0; k < laji.Length; k++)
                    for (int j = 0; j < zhangaiNumber; j++)
                    {
                        if (laji[k] && zhangai[j])
                            if (Mathf.Abs(laji[k].transform.position.z - zhangai[j].transform.position.z) < 0.95f)
                            {
                                Destroy(laji[k].gameObject);
                                break;
                            }
                    }
                bOkToNewLaJi = false;
            }
        }
 }

