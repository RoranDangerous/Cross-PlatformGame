using System;
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
    private float speed = 70;
    private float maxSpeed = 13;
    private GameObject cam;
    private GameObject weapon;
    private GameObject healthCanv;

    void Start () {
        body = transform.Find("body").gameObject;
        cam = transform.Find("PlayerCamera").gameObject;
        weapon = transform.Find("weapon").gameObject;
        healthCanv = transform.Find("healthAndReloadCanvas").gameObject;
        //sphereO = body.transform.Find("SphereTarget").gameObject;
        //sphereO.SetActive(false);
        lastPosition = body.transform.position;

        ChangeStartAcceleration();
    }

    void Update() {
        MoveCar();
    }

    private void ChangeStartAcceleration()
    {
        startAccelerationX = Input.acceleration.x;
        startAccelerationZ = Input.acceleration.y;
    }

    private void MoveCar()
    {
        if (np.myCar)
        {
            ChangeVelocity();

            RotateCar();
        }
    }

    private void ChangeVelocity()
    {
        //if(Input.acceleration.x > limit || Input.acceleration.x < -limit || Input.acceleration.x < -limit || Input.acceleration.y > limit)
        //body.GetComponent<Rigidbody>().velocity = new Vector3(getVelocityX(),0,getVelocityZ());
        //body.GetComponent<Rigidbody>().AddForce(new Vector3(getVelocityX(), 0, getVelocityZ()));
		body.transform.position = new Vector3(body.transform.position.x+0.01f,body.transform.position.y+0.01f, body.transform.position.z);
		cam.transform.position = new Vector3(body.transform.position.x,body.transform.position.y,cam.transform.position.z);
        healthCanv.transform.position = new Vector3(body.transform.position.x, body.transform.position.y, body.transform.position.z-1);
        weapon.transform.position = new Vector3(body.transform.position.x, body.transform.position.y, body.transform.position.z);
    }

    private void RotateCar()
    {
		GameObject sphere = body.transform.parent.Find ("Sphere").gameObject;
		//body.transform.LookAt(new Vector3(sphere.x,0,sphere.y));
        //if (Math.Abs(body.transform.position.x - lastPosition.x) > threshold || Math.Abs(body.transform.position.y - lastPosition.y) > threshold)
        {
			Vector3 lookPosition = new Vector3((body.transform.position.x - lastPosition.x) * speed + body.transform.position.x, 0, (body.transform.position.y - lastPosition.y) * speed + body.transform.position.y);
			sphere.transform.position = lookPosition;
            body.transform.LookAt(lookPosition);
        }
        lastPosition = body.transform.position;
    }

    private float getVelocityX()
    {
        if ((Input.acceleration.x - startAccelerationX) * speed > maxSpeed)
        {
            return maxSpeed;
        }
        else if ((Input.acceleration.x - startAccelerationX) * speed < -maxSpeed)
        {
            return -maxSpeed;
        }
        else return (Input.acceleration.x - startAccelerationX) * speed;
    }

    private float getVelocityZ()
    {
        if ((Input.acceleration.y - startAccelerationZ) * speed > maxSpeed)
        {
            return maxSpeed;
        }
        else if ((Input.acceleration.y - startAccelerationZ) * speed < -maxSpeed)
        {
            return -maxSpeed;
        }
        else return (Input.acceleration.y - startAccelerationZ) * speed;
    }
}
