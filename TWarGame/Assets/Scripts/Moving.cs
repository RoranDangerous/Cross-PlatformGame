using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Moving : Photon.MonoBehaviour {

    //private new Rigidbody rigidbody;
    private float startAccelerationX, startAccelerationZ;
    public NetworkPlayer np;
    private GameObject body;

    void Start () {
        //rigidbody = GetComponent<Rigidbody>();
        body = transform.Find("body").gameObject;
        body.AddComponent<Rigidbody>();
        startAccelerationX = Input.acceleration.x;
        startAccelerationZ = Input.acceleration.y;
    }

    void Update() {
        if (np.myCar)
        {
           GetComponent<Rigidbody>().velocity = new Vector3((Input.acceleration.x - startAccelerationX) * 20, 0, (Input.acceleration.y - startAccelerationZ) * 20);
            //Vector3 targetDir = new Vector3(body.transform.position.x + (Input.acceleration.x - startAccelerationX), body.transform.position.y, body.transform.position.z + (Input.acceleration.y - startAccelerationZ));
            //body.transform.position += body.transform.forward * Time.deltaTime * 1;
            //if (((Input.acceleration.x - startAccelerationX) > 0.04 || (Input.acceleration.x - startAccelerationX) < -0.04) && ((Input.acceleration.y - startAccelerationZ) > 0.04 || (Input.acceleration.y - startAccelerationZ) < -0.04))
            //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 100, 0.0F);
            //body.transform.LookAt(targetDir);
            //body.transform.rotation = Quaternion.LookRotation(targetDir);

        }
    }
}
