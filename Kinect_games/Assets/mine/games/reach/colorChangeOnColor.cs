using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorChangeOnColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionStay(Collision collision)
    {
        this.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
