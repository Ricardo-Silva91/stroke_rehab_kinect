using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballInitialVelocity = 600f;

    private Rigidbody rb;
    private bool ballInPlay;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if ((Input.GetKeyDown("space") || (myKinectManager.instance.closestBody != null && myKinectManager.instance.closestBody.HandRightConfidence == Windows.Kinect.TrackingConfidence.High && myKinectManager.instance.closestBody.HandRightState == Windows.Kinect.HandState.Closed)) && ballInPlay == false)
        {
            transform.parent = null;
            ballInPlay = true;
            rb.isKinematic = false;
            rb.AddForce(new Vector3(ballInitialVelocity, ballInitialVelocity, 0));
            //myKinectManager.instance.IsFire = false;
        }
        else
        {
            //KinectManager.instance.IsFire = false;
        }
    }
}