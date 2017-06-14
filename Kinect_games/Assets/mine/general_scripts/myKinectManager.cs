using UnityEngine;
using UnityEngine.UI;

using Windows.Kinect;

using System.Linq;

public class myKinectManager : MonoBehaviour
{
    private KinectSensor _sensor;
    private BodyFrameReader _bodyFrameReader;
    public Body[] _bodies = null;
    public Body closestBody;
    


    public bool IsAvailable;
    //public float PaddlePosition;
    //public bool IsFire;

    public int bodyCounter;

    public static myKinectManager instance = null;

    public Body[] GetBodies()
    {
        return _bodies;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        _sensor = KinectSensor.GetDefault();

        if (_sensor != null)
        {
            IsAvailable = _sensor.IsAvailable;
        
            _bodyFrameReader = _sensor.BodyFrameSource.OpenReader();

            if (!_sensor.IsOpen)
            {
                _sensor.Open();
            }

            _bodies = new Body[_sensor.BodyFrameSource.BodyCount];
        }
    }

    // Update is called once per frame


    public float jointDistanceCalc(CameraSpacePoint jointOne, CameraSpacePoint jointTwo)
    {
        float result = -1;

        Vector3 jointOneVector = new Vector3(jointOne.X, jointOne.Y, jointOne.Z);
        Vector3 jointTwoVector = new Vector3(jointTwo.X, jointTwo.Y, jointTwo.Z);

        result = Vector3.Distance(jointOneVector, jointTwoVector);
        
        return result;
    }

    void Update()
    {

        IsAvailable = _sensor.IsAvailable;

        if (_bodyFrameReader != null)
        {
            var frame = _bodyFrameReader.AcquireLatestFrame();

            if (frame != null)
            {
                frame.GetAndRefreshBodyData(_bodies);

                closestBody = null;
                float closestDistance = 99;
                bodyCounter = _bodies.Where(b => b.IsTracked).Count();

                foreach (var body in _bodies.Where(b => b.IsTracked))
                {
                    IsAvailable = true;
                    if (body.Joints[JointType.SpineMid].Position.Z < closestDistance)
                    {
                        closestBody = body;
                        closestDistance = body.Joints[JointType.SpineMid].Position.Z;
                    }
                }
                

                frame.Dispose();
                frame = null;
            }
        }
    }

    public static float RescalingToRangesB(float scaleAStart, float scaleAEnd, float scaleBStart, float scaleBEnd, float valueA)
    {
        return (((valueA - scaleAStart) * (scaleBEnd - scaleBStart)) / (scaleAEnd - scaleAStart)) + scaleBStart;
    }

    void OnApplicationQuit()
    {
        if (_bodyFrameReader != null)
        {
            _bodyFrameReader.IsPaused = true;
            _bodyFrameReader.Dispose();
            _bodyFrameReader = null;
        }

        if (_sensor != null)
        {
            if (_sensor.IsOpen)
            {
                _sensor.Close();
            }

            _sensor = null;
        }
    }
}





