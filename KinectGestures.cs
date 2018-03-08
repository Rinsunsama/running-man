using UnityEngine;
using Windows.Kinect;
using System.Collections;
using System.Collections.Generic;
//using Assets.KinectScripts;

public class KinectGestures
{
    //used for jump
    private static float footy;
    private static float Hip;
    private static float ankleLeft_y;
    //usef for slide
    public interface GestureListenerInterface//�ṩһ��ʱ��������ӿ�
    {
        // Invoked when a new user is detected and tracking starts ����ұ�����������׷�ٿ�ʼʱ����
        // Here you can start gesture detection with KinectManager.DetectGesture()
        void UserDetected(long userId, int userIndex);

        // Invoked when a user is lost//���û���ʧʱ����
        // Gestures for this user are cleared automatically, but you can free the used resources
        void UserLost(long userId, int userIndex);

        // Invoked when a gesture is in progress 
        void GestureInProgress(long userId, int userIndex, Gestures gesture, float progress,
            JointType joint, Vector3 screenPos);

        // Invoked if a gesture is completed.//�������ʱ������
        // Returns true, if the gesture detection must be restarted, false otherwise
        bool GestureCompleted(long userId, int userIndex, Gestures gesture,
            JointType joint, Vector3 screenPos);

        // Invoked if a gesture is cancelled.//����ȡ��ʱ������
        // Returns true, if the gesture detection must be retarted, false otherwise
        bool GestureCancelled(long userId, int userIndex, Gestures gesture,
            JointType joint);
    }

    public enum Gestures//ö�����Ƶ�����
    {
        None = 0,
        RaiseRightHand,
        RaiseLeftHand,
        Psi,
        Tpose,
        Stop,
        Wave,
        //		Click,
        SwipeLeft,
        SwipeRight,
        SwipeUp,
        SwipeDown,
        //		RightHandCursor,
        //		LeftHandCursor,
        ZoomOut,
        ZoomIn,
        Wheel,
        Jump,//��
        Squat,
        Push,
        Pull,
        Walk,
        Myjump,
        MySwipeRight,
        MySwipeLeft,
        MyMoveRight,
        MyMoveLeft,
        MySquat,
        MySlide,
        MyRaiseUpRight,
        MyRaiseUpLeft
    }


    public struct GestureData//���Ƶĸ�������
    {
        public long userId;
        public Gestures gesture;
        public int state;
        public float timestamp;
        public int joint;
        public Vector3 jointPos;
        public Vector3 screenPos;
        public float tagFloat;
        public Vector3 tagVector;
        public Vector3 tagVector2;
        public float progress;
        public bool complete;
        public bool cancelled;
        public List<Gestures> checkForGestures;
        public float startTrackingAtTime;
    }

    // Gesture related constants, variables and functions  ����ȡ�������������
    private const int leftHandIndex = (int)JointType.HandLeft;
    private const int rightHandIndex = (int)JointType.HandRight;

    private const int leftElbowIndex = (int)JointType.ElbowLeft;
    private const int rightElbowIndex = (int)JointType.ElbowRight;

    private const int leftShoulderIndex = (int)JointType.ShoulderLeft;
    private const int rightShoulderIndex = (int)JointType.ShoulderRight;

    private const int hipCenterIndex = (int)JointType.SpineBase;
    private const int shoulderCenterIndex = (int)JointType.SpineShoulder;
    private const int leftHipIndex = (int)JointType.HipLeft;
    private const int rightHipIndex = (int)JointType.HipRight;

    private const int footleftIndex = (int)JointType.FootLeft;
    private const int footRightIndex = (int)JointType.FootRight;
    private const int head = (int)JointType.Head;

    private const int AnkleLeftIndex = (int)JointType.AnkleLeft;
    private const int AnkleRightIndex = (int)JointType.AnkleRight;
    //��Ҫ�Ĺ������ַŵ�һ��������
    private static int[] neededJointIndexes = {
		leftHandIndex, rightHandIndex, leftElbowIndex, rightElbowIndex, leftShoulderIndex, rightShoulderIndex,
		hipCenterIndex, shoulderCenterIndex, leftHipIndex, rightHipIndex,footleftIndex,footRightIndex,head,
        AnkleLeftIndex, AnkleRightIndex
	};

    //������Ҫ�Ĺ�������
    // Returns the list of the needed gesture joint indexes
    public static int[] GetNeededJointIndexes()
    {
        return neededJointIndexes;
    }

    //�������ƽڵ�
    private static void SetGestureJoint(ref GestureData gestureData, float timestamp, int joint, Vector3 jointPos)
    {
        gestureData.joint = joint;
        gestureData.jointPos = jointPos;
        gestureData.timestamp = timestamp;
        gestureData.state++;
    }
    //��������ȡ��
    private static void SetGestureCancelled(ref GestureData gestureData)
    {
        gestureData.state = 0;
        gestureData.progress = 0f;
        gestureData.cancelled = true;
    }
    //��������Ƿ����
    private static void CheckPoseComplete(ref GestureData gestureData, float timestamp, Vector3 jointPos, bool isInPose, float durationToComplete)
    {
        if (isInPose)
        {
            float timeLeft = timestamp - gestureData.timestamp;
            gestureData.progress = durationToComplete > 0f ? Mathf.Clamp01(timeLeft / durationToComplete) : 1.0f;

            if (timeLeft >= durationToComplete)
            {
                gestureData.timestamp = timestamp;
                gestureData.jointPos = jointPos;
                gestureData.state++;
                gestureData.complete = true;
            }
        }
        else
        {
            SetGestureCancelled(ref gestureData);
        }
    }
    //��������Ļ�ϵ�λ�ã�������������
    private static void SetScreenPos(long userId, ref GestureData gestureData, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        Vector3 handPos = jointsPos[rightHandIndex];
        //		Vector3 elbowPos = jointsPos[rightElbowIndex];
        //		Vector3 shoulderPos = jointsPos[rightShoulderIndex];
        bool calculateCoords = false;

        if (gestureData.joint == rightHandIndex)
        {
            if (jointsTracked[rightHandIndex] /**&& jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]*/)
            {
                calculateCoords = true;
            }
        }
        else if (gestureData.joint == leftHandIndex)
        {
            if (jointsTracked[leftHandIndex] /**&& jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex]*/)
            {
                handPos = jointsPos[leftHandIndex];
                //				elbowPos = jointsPos[leftElbowIndex];
                //				shoulderPos = jointsPos[leftShoulderIndex];

                calculateCoords = true;
            }
        }

        if (calculateCoords)
        {
            //			if(gestureData.tagFloat == 0f || gestureData.userId != userId)
            //			{
            //				// get length from shoulder to hand (screen range)
            //				Vector3 shoulderToElbow = elbowPos - shoulderPos;
            //				Vector3 elbowToHand = handPos - elbowPos;
            //				gestureData.tagFloat = (shoulderToElbow.magnitude + elbowToHand.magnitude);
            //			}

            if (jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] &&
                jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex])
            {
                Vector3 shoulderToHips = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
                Vector3 rightToLeft = jointsPos[rightShoulderIndex] - jointsPos[leftShoulderIndex];

                gestureData.tagVector2.x = rightToLeft.x; // * 1.2f;
                gestureData.tagVector2.y = shoulderToHips.y; // * 1.2f;

                if (gestureData.joint == rightHandIndex)
                {
                    gestureData.tagVector.x = jointsPos[rightShoulderIndex].x - gestureData.tagVector2.x / 2;
                    gestureData.tagVector.y = jointsPos[hipCenterIndex].y;
                }
                else
                {
                    gestureData.tagVector.x = jointsPos[leftShoulderIndex].x - gestureData.tagVector2.x / 2;
                    gestureData.tagVector.y = jointsPos[hipCenterIndex].y;
                }
            }

            //			Vector3 shoulderToHand = handPos - shoulderPos;
            //			gestureData.screenPos.x = Mathf.Clamp01((gestureData.tagFloat / 2 + shoulderToHand.x) / gestureData.tagFloat);
            //			gestureData.screenPos.y = Mathf.Clamp01((gestureData.tagFloat / 2 + shoulderToHand.y) / gestureData.tagFloat);

            if (gestureData.tagVector2.x != 0 && gestureData.tagVector2.y != 0)
            {
                Vector3 relHandPos = handPos - gestureData.tagVector;
                gestureData.screenPos.x = Mathf.Clamp01(relHandPos.x / gestureData.tagVector2.x);
                gestureData.screenPos.y = Mathf.Clamp01(relHandPos.y / gestureData.tagVector2.y);
            }

            //Debug.Log(string.Format("{0} - S: {1}, H: {2}, SH: {3}, L : {4}", gestureData.gesture, shoulderPos, handPos, shoulderToHand, gestureData.tagFloat));
        }
    }
    //����������
    private static void SetZoomFactor(long userId, ref GestureData gestureData, float initialZoom, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        Vector3 vectorZooming = jointsPos[rightHandIndex] - jointsPos[leftHandIndex];

        if (gestureData.tagFloat == 0f || gestureData.userId != userId)
        {
            gestureData.tagFloat = 0.5f; // this is 100%
        }

        float distZooming = vectorZooming.magnitude;
        gestureData.screenPos.z = initialZoom + (distZooming / gestureData.tagFloat);
    }

    private static void SetWheelRotation(long userId, ref GestureData gestureData, Vector3 initialPos, Vector3 currentPos)
    {
        float angle = Vector3.Angle(initialPos, currentPos) * Mathf.Sign(currentPos.y - initialPos.y);
        gestureData.screenPos.z = angle;
    }

    // estimate the next state and completeness of the gesture
    public static void CheckForGesture(long userId, ref GestureData gestureData, float timestamp, ref Vector3[] jointsPos, ref bool[] jointsTracked)
    {
        if (gestureData.complete)
            return;

        switch (gestureData.gesture)
        {
       
            case Gestures.MyRaiseUpRight:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[rightHandIndex] && jointsTracked[head])//���ֺ�ͷ���ܱ�׷�ٵ�
                        {
                            Hip = jointsPos[head].y;
                            SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
                        }
                        break;
                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[head] && jointsPos[rightHandIndex].y - jointsPos[head].y > 0.15f;//���ֱ�ͷ��һ���ľ��룬������뼸������Ӧ��������ϵ���
                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                            else
                            {
                                // cancel the gesture
                                SetGestureCancelled(ref gestureData);
                            }
                            break;
                        }
                        break;
                }
                break;
            case Gestures.MyRaiseUpLeft:
                switch (gestureData.state)
                {
                    case 0:  
                        if (jointsTracked[leftHandIndex] && jointsTracked[head])//���ֺ�ͷ���ܱ�׷�ٵ�
                        {
                            Hip = jointsPos[head].y;
                            SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
                        }
                        break;
                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[head] && jointsPos[leftHandIndex].y - jointsPos[head].y > 0.15f;//���ֱ�ͷ��һ���ľ��룬������뼸������Ӧ��������ϵ���
                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                            else
                            {
                                // cancel the gesture
                                SetGestureCancelled(ref gestureData);
                            }
                            break;
                        }
                        break;
                }
                break;
            case Gestures.Myjump:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[AnkleLeftIndex] && jointsTracked[AnkleRightIndex])//���ýŸ������ý���
                        {
                            ankleLeft_y = jointsPos[AnkleLeftIndex].y;
                            SetGestureJoint(ref gestureData, timestamp, AnkleRightIndex, jointsPos[AnkleRightIndex]);
                            gestureData.progress = 0.5f;

                        }
                        break;
                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)//���ýŸ������ý���
                        {
                            bool isInPose = jointsTracked[AnkleLeftIndex] && jointsTracked[AnkleRightIndex] &&
                                (jointsPos[AnkleRightIndex].y - gestureData.jointPos.y) > 0.1f && (jointsPos[AnkleLeftIndex].y - ankleLeft_y) > 0.1f;//���׸߶Ȳ�10cm
                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;
            case Gestures.MySquat:
                switch (gestureData.state)
                {
                    case 0:  // gesture detection - phase 1
                        if (jointsTracked[hipCenterIndex])
                        {

                            SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.5f;
                        }
                        break;

                    case 1:  // gesture phase 2 = complete
                        if ((timestamp - gestureData.timestamp) < 1.5f)
                        {
                            bool isInPose = jointsTracked[hipCenterIndex] &&
                                (jointsPos[hipCenterIndex].y - gestureData.jointPos.y) < -0.2f && (jointsPos[hipCenterIndex].y - jointsPos[footRightIndex].y) < 0.4f;//�β��½�0.2m,���Һ��β��ľ����ֵ����.

                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            // cancel the gesture
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;
            case Gestures.Walk:
                switch (gestureData.state)
                {
                    case 0://��·�ĵ�һ�׶Σ�����ҽ�һ��һ��
                        if (jointsTracked[footRightIndex] && jointsTracked[footleftIndex]
                            && Mathf.Abs(jointsPos[footRightIndex].y - jointsPos[footleftIndex].y) > 0.15f)
                        {

                            SetGestureJoint(ref gestureData, timestamp, footRightIndex, jointsPos[footRightIndex]);
                            gestureData.progress = 0.5f;//����ʱ��Ϊ0.5s����֪����ʲô�ã�
                        }
                        break;
                    case 1:
                        if ((timestamp - gestureData.timestamp) < 0.5f)//����������Ƶ�ʱ������1��
                        {
                            bool isInPose = jointsTracked[footRightIndex] && jointsTracked[footleftIndex] &&
                               Mathf.Abs(jointsPos[footRightIndex].y - jointsPos[footleftIndex].y) < 0.03f;
                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;          
            case Gestures.MyMoveLeft:
                switch (gestureData.state)
                {
                    case 0:
                        if (jointsTracked[hipCenterIndex])
                        {
                            SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.1f;
                        }
                        break;
                    case 1:
                        if (timestamp - gestureData.timestamp < 1.0f)
                        {
                            bool isInPose = (jointsTracked[hipCenterIndex]) && (gestureData.jointPos.x - jointsPos[hipCenterIndex].x > 0.15f);
                            ;
                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;
            case Gestures.MyMoveRight:
                switch (gestureData.state)
                {
                    case 0:
                        if (jointsTracked[hipCenterIndex])
                        {
                            SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
                            gestureData.progress = 0.1f;
                        }
                        break;
                    case 1:
                        if (timestamp - gestureData.timestamp < 1.0f)
                        {
                            bool isInPose = (jointsTracked[hipCenterIndex]) && (gestureData.jointPos.x - jointsPos[hipCenterIndex].x < -0.15f);
                            ;
                            if (isInPose)
                            {
                                Vector3 jointPos = jointsPos[gestureData.joint];
                                CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
                            }
                        }
                        else
                        {
                            SetGestureCancelled(ref gestureData);
                        }
                        break;
                }
                break;  
        }
    }

}
