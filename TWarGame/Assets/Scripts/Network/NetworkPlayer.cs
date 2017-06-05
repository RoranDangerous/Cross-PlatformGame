using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {

    public GameObject cam;
    public bool myCar = true;
    private Quaternion fixedRotation;

	void Start () {
        if (!photonView.isMine)
        {
            myCar = false;
            cam.SetActive(false);
        }
        fixedRotation = cam.transform.rotation;
	}
	
	void Update () {
        if (cam.transform.rotation != fixedRotation)
            cam.transform.rotation = fixedRotation;
	}
}
