using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Moving : Photon.MonoBehaviour {
    
    private float startAccelerationX, startAccelerationZ;
    public NetworkPlayer np;
    private GameObject body;
    private Vector3 lastPosition;
    private GameObject sphereO;
    private float threshold = 0.01f;

    void Start () {
        body = transform.Find("body").gameObject;
        startAccelerationX = Input.acceleration.x;
        startAccelerationZ = Input.acceleration.y;
        lastPosition = body.transform.position;
        sphereO = body.transform.Find("SphereTarget").gameObject;
        sphereO.SetActive(false);
    }

    void Update() {
        if (np.myCar)
        {
            GetComponent<Rigidbody>().velocity = new Vector3((Input.acceleration.x - startAccelerationX) * 20, 0, (Input.acceleration.y - startAccelerationZ) * 20);
            
            if(body.transform.position.x - lastPosition.x > threshold || body.transform.position.x - lastPosition.x < -threshold || body.transform.position.z - lastPosition.z > threshold || body.transform.position.z - lastPosition.z < -threshold)
            {
                Vector3 lookPosition = new Vector3((body.transform.position.x - lastPosition.x) * 20 + body.transform.position.x, lastPosition.y, (body.transform.position.z - lastPosition.z) * 20 + body.transform.position.z);
                body.transform.LookAt(lookPosition);
                lastPosition = body.transform.position;
                sphereO.transform.position = lookPosition;
            }
            lastPosition = body.transform.position;
        }
    }
}
