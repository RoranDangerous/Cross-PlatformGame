using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        print("Update");
        if (Input.anyKeyDown)
            print("Any key down");
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            print("arrow was pressed");
    }
}
