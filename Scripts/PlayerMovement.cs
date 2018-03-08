using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    public float n = 300f;
    public static bool RunDet = false;
    public float RunSpeed = 6.0F;
    static  public bool RunAnimFlag = true;
    private bool MoveLeftDet = false;
    private bool MoveRightDet = false;
    private bool MoveLeftFir_Fra = false;
    private bool MoveRightFir_Fra = false;
    public float BorderLeft, BorderRight;
    public float MoveLRSpeed = 1;
    public float MoveLRDis = 1f;
    private float EndPosLeft;
    private float EndPosRight;
    public float Accuracy = 0.1f;
    private bool JumpDet = false;
    private bool SquatDet = false;
    float dunxiapro = 0;
    float leftMovePro = 0;
    float rightMovePro = 0;
    public Image zanTing;
    bool bPause=false;
    public static bool bOkToRestartAndRunDet=false;

    void Start()
    {
    }
    void FixedUpdate()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * -3f, ForceMode.Acceleration);

        if (RunDet)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * RunSpeed);//向前奔跑
            if (RunAnimFlag && this.gameObject.transform.position.y <= 0.9f)
                animation.Play("run");
        }

        if (MoveRightDet && rightMovePro<10 && (leftMovePro==0 || leftMovePro==10))
        {
            if (Mathf.Abs(transform.position.x - BorderRight) < Accuracy)//如果是边界则不能向右走
            {
                MoveRightFir_Fra = false;
                MoveRightDet = false;
                RunAnimFlag = true;
            }
            else//否则可以往右走
            {
                if (!MoveRightFir_Fra)//标记要到达的位置
                {
                    EndPosRight = transform.position.x + 1;
                    MoveRightFir_Fra = true;
                    RunAnimFlag = false;
                    animation.Play("right");
                }
                transform.Translate(Vector3.right * Time.deltaTime * MoveLRSpeed);
                rightMovePro += Time.deltaTime;
                if (Mathf.Abs(transform.position.x - EndPosRight) < Accuracy)//走到了标记的位置
                {
                    rightMovePro = 10;
                    MoveRightDet = false;
                    MoveRightFir_Fra = false;
                    RunAnimFlag = true;
                }
            }
        }   
        //--------------------------------------------------------------------------------------向左移动 并且不在向右移动的过程中
        if (MoveLeftDet && leftMovePro<10 &&(rightMovePro==0 || rightMovePro==10))
        {
            if (Mathf.Abs(transform.position.x - BorderLeft) < Accuracy)//如果是边界则不能向左走
            {
                MoveLeftDet = false;
                MoveLeftFir_Fra = false;
               // bMoveLR = false;
                RunAnimFlag = true;
            }
            else
            {
                if (!MoveLeftFir_Fra)
                {
                    EndPosLeft = transform.position.x - 1;
                    MoveLeftFir_Fra = true;
                    RunAnimFlag = false;
                    animation.Play("left");
                }
                transform.Translate(Vector3.right * Time.deltaTime * MoveLRSpeed*-1);
                leftMovePro += Time.deltaTime;
                if (Mathf.Abs(transform.position.x - EndPosLeft) < Accuracy)//到达目的地
                {
                    leftMovePro = 10;
                    RunAnimFlag = true;
                    MoveLeftFir_Fra = false;
                    MoveLeftDet = false;

                }
            }
        }
        //-----------------------------------------------------------------------------------跳跃
        if (JumpDet && transform.position.y < 1.1f )//不会连跳
        {

            animation.Play("jump");
            this.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * n, ForceMode.Acceleration);
            JumpDet = false;
        }
        if (SquatDet && transform.position.y < 1.0f )
        {
            this.gameObject.GetComponent<BoxCollider>().center = new Vector3(0, -6, 0);
            this.gameObject.GetComponent<BoxCollider>().size = new Vector3(this.gameObject.GetComponent<BoxCollider>().size.x, 5, this.gameObject.GetComponent<BoxCollider>().size.z);
            RunAnimFlag = false;
            animation.Play("slide");
            dunxiapro += 0.02f;
            if (dunxiapro >= 1)
            {
                RunAnimFlag = true;
                SquatDet = false;
                dunxiapro = 0;
                this.gameObject.GetComponent<BoxCollider>().center = new Vector3(0, -2.9f, 0);
                this.gameObject.GetComponent<BoxCollider>().size = new Vector3(this.gameObject.GetComponent<BoxCollider>().size.x, 12.5f, this.gameObject.GetComponent<BoxCollider>().size.z);
            }
        }
    }
    //------------------------------------------------------------------------------------endofDet-----------------------------------------------------------------------------------------------------------
    public void UserDetected(long userId, int userIndex)
    {
        Debug.Log("检测到用户");
    }
    public void UserLost(long userId, int userIndex)
    {
    }
    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, Windows.Kinect.JointType joint, Vector3 screenPos)
    {
    }
    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, Windows.Kinect.JointType joint, Vector3 screenPos)
    {
        if (gesture == KinectGestures.Gestures.MySquat)
        {
            SquatDet = true;
        }
        if (gesture == KinectGestures.Gestures.Myjump)
        {
            JumpDet = true;
        }
        if (gesture == KinectGestures.Gestures.Walk)
        {
            RunDet = true;
            RunAnimFlag = true;
            if (bPause )
            {
                bPause = false;    
                Continue();
            }
            if (GUImanager.bOkToRestart)
            {
                bOkToRestartAndRunDet = true;
            }
            if (Time.timeScale != 1)
            {
                Time.timeScale = 1;
                zanTing.gameObject.SetActive(false);
            }
           }
        if (gesture == KinectGestures.Gestures.MyMoveLeft)
        {
            leftMovePro = 0; 
            MoveLeftDet = true;
        }
        if (gesture == KinectGestures.Gestures.MyMoveRight)
        {
            rightMovePro = 0;
            MoveRightDet = true;
        }
        if(gesture==KinectGestures.Gestures.MyRaiseUpLeft)
        {
            Time.timeScale = 0;
            zanTing.gameObject.SetActive(true);
            bPause = true;
        }
        // }
        return true;
    }
    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, Windows.Kinect.JointType joint)
    {
        return true;
    }
    void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.W)  || Input.GetKeyDown(KeyCode.UpArrow))
        {
            RunDet = true;
            RunAnimFlag = true;
        }
        if(Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftMovePro = 0; ;
            MoveLeftDet = true;
        }
        if(Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow))
        {
            rightMovePro = 0;
            MoveRightDet = true;
        }
        if(Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow))
        {
            SquatDet = true;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            JumpDet = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            zanTing.gameObject.SetActive(true);
        }
    }
    public void Continue()
    {
        Time.timeScale = 1;
        zanTing.gameObject.SetActive(false);
    }
    //--------------------------------------------------------------------endofGestureListenerInterface--------------------------------------------------------------------------------------
}
