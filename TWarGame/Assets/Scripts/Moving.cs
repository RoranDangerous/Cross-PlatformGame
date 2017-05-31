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

    void Update() {
        if (np.myCar)
        {
            rigidbody.velocity = new Vector3((Input.acceleration.x - startAccelerationX) * 20, 0, (Input.acceleration.y - startAccelerationZ) * 20);
            /*if((Input.acceleration.x - startAccelerationX) > 0.1 || (Input.acceleration.y - startAccelerationZ) > 0.1)
                body.transform.LookAt(new Vector3(body.transform.position.x + (Input.acceleration.x - startAccelerationX), body.transform.position.y, body.transform.position.z + (Input.acceleration.y - startAccelerationZ)));*/
            
        }
    }
}
