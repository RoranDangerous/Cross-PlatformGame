using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Moving : Photon.MonoBehaviour {

    private new Rigidbody rigidbody;
    private float startAccelerationX, startAccelerationZ;
    public NetworkPlayer np;
    public GameObject body;

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        startAccelerationX = Input.acceleration.x;
        startAccelerationZ = Input.acceleration.y;

    }
	
	void Update () {
        if(np.myCar)
            rigidbody.velocity = new Vector3((Input.acceleration.x - startAccelerationX) * 25, 0, (Input.acceleration.y - startAccelerationZ) * (25));
    }
}
