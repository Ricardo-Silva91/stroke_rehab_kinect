using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Windows.Kinect;

public class v2Examples_debug : MonoBehaviour {


    public myKinectManager kinManager;

    public Text bodyCount;
    public Text instruction;
    public Text status;

    public int bodiesDetectedCount;


    public bool workFlag;
    public bool waiting = false;
    public int secsToWait;
    public int secsWaited;
    public float startWaitingTime;
    public int gameState = 0;

    public JointType[] jointOrder;
    public bool leftHand;
    public int currentJoint = 0;
    public float jointMargin;
    public float leanMargin;
    public float currentDistance;

    public float handX, headX, lean;
    public JointType affectedHand;
    public Vector3 restingPosition;
    public Vector3 goalPosition;
    public GameObject goalSphere;
    public float goalDistance;


    // Use this for initialization
    void Start()
    {

        if(leftHand==true)
        {
            affectedHand = JointType.HandLeft;
        }
        else
        {
            affectedHand = JointType.HandRight;
        }



        instruction.text = "Welcome";
        waitFor(2);
        gameState = 1;




    }

    void waitFor(int seconds)
    {
        secsToWait = seconds;
        waiting = true;
        startWaitingTime = Time.time;
    }

    private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
        //return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    // Update is called once per frame
    void Update () {

        if (kinManager == null)
        {
            kinManager = myKinectManager.instance;
        }

        bodiesDetectedCount = kinManager.bodyCounter;                
        bodyCount.text = "number of bodies: " + bodiesDetectedCount;

        if (workFlag == true)
        {
            if (gameState != 99)
            {
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
                    if (gameState == 1)
                    {
                        instruction.text = "please reach " + jointOrder[currentJoint].ToString();

                        Vector3 goalJoint = GetVector3FromJoint(kinManager.closestBody.Joints[jointOrder[currentJoint]]);
                        //Vector3 handVector = GetVector3FromJoint(kinManager.closestBody.Joints[JointType.HandRight]);
                        Vector3 handVector = GetVector3FromJoint(kinManager.closestBody.Joints[affectedHand]);
                        //Vector3 handVector = GetVector3FromJoint(kinectManager.closestBody.Joints[JointType.WristRight]);

                        currentDistance = Vector3.Distance(goalJoint, handVector);
                        status.text = "current distance: " + currentDistance;


                        if (currentDistance < jointMargin)
                        {
                            currentJoint++;
                            if (currentJoint >= jointOrder.Length)
                            {
                                instruction.text = "WelL done!\nNext Phase";
                                waitFor(2);
                                gameState = 2;
                            }
                            else
                            {
                                instruction.text = "WelL done!\nNew Joint Coming";
                                waitFor(2);
                            }

                        }
                    }
                    else if (gameState == 2)
                    {
                        instruction.text = "please raise hand above head";
                        //handX = kinManager.closestBody.Joints[JointType.HandRight].Position.Y;
                        handX = kinManager.closestBody.Joints[affectedHand].Position.Y;
                        headX = kinManager.closestBody.Joints[JointType.Head].Position.Y;
                        lean = kinManager.closestBody.Lean.X;
                        status.text = "Lean X: " + lean;

                        if (Mathf.Abs(lean)<leanMargin && handX > headX)
                        {
                            instruction.text = "WelL done!\nThe Game is Over";
                            waitFor(3);
                            gameState = 99;
                        }
                    }
                    else if(gameState == 3)
                    {
                        instruction.text = "Move your hand forward";

                        //restingPosition = GetVector3FromJoint(kinManager.closestBody.Joints[affectedHand]);
                        //Windows.Kinect.Vector4 orientation = kinManager.closestBody.JointOrientations[affectedHand].Orientation;
                        goalPosition = new Vector3(restingPosition.x, restingPosition.y, restingPosition.z + goalDistance);
                        goalSphere.transform.position = goalPosition;
                        //goalSphere.transform.rotation = new Quaternion(orientation.X, orientation.Y, orientation.Z, orientation.W);



                    }
                }
            }
            else
            {
                workFlag = false;
            }
        }
    }

}

