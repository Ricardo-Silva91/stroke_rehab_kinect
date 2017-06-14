using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Windows.Kinect;

public class reach_gameMaster : MonoBehaviour
{

    public Vector3[] goalPlaces;
    public int goalIndex = 0;
    public int gameState = 0;

    public myKinectManager myKinManager;
    public FacetrackingManager faceTracker;
    public GameObject mySphere, goalSphere;
    public GameObject handRight;
    public GameObject handLeft;
    public GameObject affectedHand;
    public Windows.Kinect.JointType jointToFollow;
    public Text instruction;
    public UnityEngine.AudioSource biteSound;

    public bool workFlag;
    public bool waiting = false;
    public int secsToWait;
    public int secsWaited;
    public float startWaitingTime;
    public float joinyPosAjustX = 0.07f, joinyPosAjustY = -0.1f, joinyPosAjustZ = 0.2f;
    public float minDistance;
    public float minDistanceHead;
    public float currentDist;
    public bool hand_left;
    public float leanMargin;

    private Vector3 GetJointPos(Windows.Kinect.JointType joint)
    {
        KinectManager manager = KinectManager.Instance;

        Vector3 jointPos = manager.GetJointPosition((long)myKinManager.closestBody.TrackingId, (int)joint);
        jointPos.z = 1 - jointPos.z;

        jointPos.x += joinyPosAjustX;
        jointPos.y += joinyPosAjustY;
        jointPos.z += joinyPosAjustZ;

        return jointPos;
    }
    // Use this for initialization
    void Start()
    {

    }



    void waitFor(int seconds)
    {
        secsToWait = seconds;
        waiting = true;
        startWaitingTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (workFlag == true)
        {
            if (myKinManager == null)
            {
                myKinManager = myKinectManager.instance;
            }

            if (faceTracker == null)
            {
                faceTracker = FacetrackingManager.Instance;
            }

            if (waiting)
            {
                float currentTime = Time.time;
                secsWaited = (int)(currentTime - startWaitingTime);
                if (secsWaited >= secsToWait)
                {
                    waiting = false;
                }
            }
            else
            {
                if (gameState == 0)
                {
                    instruction.text = "Welcome";
                    gameState = 1;

                    if (hand_left == true)
                    {
                        affectedHand = handLeft;
                        jointToFollow = JointType.HandLeft;
                    }
                    else
                    {
                        affectedHand = handRight;
                        jointToFollow = JointType.HandRight;
                    }


                    waitFor(2);
                }
                else if (gameState == 1)
                {
                    goalSphere.transform.position = goalPlaces[goalIndex];

                    //Vector3 myPos = GetJointPos(jointToFollow);
                    Vector3 myPos = affectedHand.transform.position;

                    mySphere.transform.position = myPos;

                    currentDist = Vector3.Distance(myPos, goalPlaces[goalIndex]);

                    instruction.text = "touch sphere " + goalIndex + ". Distance " + currentDist;

                    if (currentDist < minDistance)
                    {
                        gameState = 2;
                    }
                }
                else if (gameState == 2)
                {

                    Vector3 myPos = affectedHand.transform.position;

                    goalSphere.transform.position = myPos;

                    //goalSphere.transform.position = GetJointPos(jointToFollow);
                    currentDist = myKinManager.jointDistanceCalc(myKinManager.closestBody.Joints[jointToFollow].Position, myKinManager.closestBody.Joints[JointType.Head].Position);
                    //currentDist = myKinManager.jointDistanceCalc(myKinManager.closestBody.Joints[jointToFollow].Position, myKinManager.closestBody.Joints[JointType.Neck].Position);

                    instruction.text = "bring sphere to head. Distance " + currentDist + ".\nKeep your back straight: " + myKinManager.closestBody.Lean.X + " : " + myKinManager.closestBody.Lean.Y;

                    if (currentDist < minDistanceHead && Mathf.Abs(myKinManager.closestBody.Lean.Y) < leanMargin && Mathf.Abs(myKinManager.closestBody.Lean.X) < leanMargin)
                    {
                        goalIndex++;
                        biteSound.PlayOneShot(biteSound.clip);

                        if (goalIndex < goalPlaces.Length)
                        {
                            gameState = 3;
                        }
                        else
                        {
                            gameState = 99;
                        }
                    }
                }
                else if (gameState == 3)
                {
                    instruction.text = "Well done!";
                    gameState = 1;
                    waitFor(2);

                }
                else if (gameState == 99)
                {
                    instruction.text = "Well Done!! The Game is Over";
                    workFlag = false;
                }
            }
        }
    }
}
