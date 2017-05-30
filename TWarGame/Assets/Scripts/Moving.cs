using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Moving : MonoBehaviour {

    private new Rigidbody rigidbody;
    private float startAccelerationX, startAccelerationZ;

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        startAccelerationX = Input.acceleration.x;
        startAccelerationZ = Input.acceleration.z;
    }
	
	void Update () {
        //Vector3 movement = new Vector3(Input.acceleration.x - startAccelerationX, 0, Input.acceleration.z - startAccelerationZ);
        rigidbody.velocity = new Vector3((Input.acceleration.x - startAccelerationX) * 25, 0, (Input.acceleration.z - startAccelerationZ) * (-25));
    }
}
