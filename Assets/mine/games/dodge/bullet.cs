using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    public bool collided = false;
    public string collisionObjectName;
    public GameObject collisionObject;

    private void OnCollisionStay(Collision collision)
    {
        if(collided == false)
        {
            collisionObjectName = collision.collider.name;
            collisionObject = collision.collider.gameObject;
            //Debug.Log(this.name + "collided with " + collision.collider.name);
            collided = true;
        }
    }

    // Use this for initialization
    void Start () {
        collided = false;
        collisionObjectName = null;
        collisionObject = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
