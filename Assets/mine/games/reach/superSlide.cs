using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class superSlide : MonoBehaviour {

    public Slider horizontalSlider;
    public Slider verticalSlider;

    public bool workFlag; 

    public float lastHValue, lastVValue;
    public Vector3 initialPos;
    public float distanceMultiplicator = 1;
    public float distanceToMove = 0;

	// Use this for initialization
	void Start () {
        initialPos = this.transform.position;        		
	}

    public void slideHorizontal(float value) {
        
        distanceToMove = (value - 0.5f) * distanceMultiplicator;

        this.transform.position = new Vector3(initialPos.x + distanceToMove, initialPos.y, initialPos.z);//.Translate(new Vector3(value, 0, 0));
    }

    public void slideVertical(float value)
    {

        distanceToMove = (value - 0.5f) * distanceMultiplicator;

        this.transform.position = new Vector3(initialPos.x, initialPos.y, initialPos.z + distanceToMove);//.Translate(new Vector3(value, 0, 0));
    }

    // Update is called once per frame
    void Update () {
		
        if(workFlag == true)
        {
            float currentHValue = horizontalSlider.value;
            float currentVValue = verticalSlider.value;
            if(currentHValue != lastHValue)
            {
                slideHorizontal(currentHValue);
                lastHValue = currentHValue;
            }
            if(currentVValue != lastVValue)
            {
                slideVertical(currentVValue);
                lastVValue = currentVValue;
            }
        }


	}
}
