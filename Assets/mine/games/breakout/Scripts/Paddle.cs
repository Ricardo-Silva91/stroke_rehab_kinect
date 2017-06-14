using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float paddleSpeed = 1f;

    private Vector3 playerPos = new Vector3(0, -9.5f, 0);

    void Update()
    {
        float xPos = transform.position.x;

        if (myKinectManager.instance.IsAvailable && myKinectManager.instance.closestBody!=null)
        {
            xPos = myKinectManager.RescalingToRangesB(-1, 1, -8, 8, myKinectManager.instance.closestBody.Lean.X);
            //xPos = myKinectManager.instance.PaddlePosition;
        }
        else
        {
            xPos = transform.position.x + (Input.GetAxis("Horizontal") * paddleSpeed);
        }
        
        playerPos = new Vector3(Mathf.Clamp(xPos, -8f, 8f), -9.5f, 0f);

        transform.position = playerPos;
    }
}



