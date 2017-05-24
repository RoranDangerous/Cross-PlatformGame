using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour {

    public GameObject cube;
    private new Rigidbody rigidbody;
    private float startAccelerationX, startAccelerationY;
    // Use this for initialization
    void Start () {
        print("######## START @@@@@@@");
        rigidbody = cube.GetComponent<Rigidbody>();
        startAccelerationX = Input.acceleration.x;
        startAccelerationY = Input.acceleration.y;

    }
	
	// Update is called once per frame
	void Update () {
        print("Update");
        Vector3 movement = new Vector3(Input.acceleration.x-startAccelerationX, Input.acceleration.y-startAccelerationY, 0.0f);
        rigidbody.velocity = movement * 25;
    }
}
