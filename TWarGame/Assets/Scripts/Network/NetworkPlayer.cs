using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {

    public GameObject cam;
    public bool myCar = true;
	// Use this for initialization
	void Start () {
        if (!photonView.isMine)
        {
            myCar = false;
            cam.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
