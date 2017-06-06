using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shooting : Photon.MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject body;
    public GameObject weapon;
    public GameObject bullet;
    private float startAngle = -15;
    private float endAngle = 15;
    

    public virtual void OnPointerUp(PointerEventData ped)
    {
        //bullet.GetComponent<Rigidbody>().AddForce(weapon.transform.forward * 50);
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        print("Down");
        
        //bullet.GetComponent<Rigidbody>().AddForce(-weapon.transform.forward * 50);
    }

    // Use this for initialization
    void Start () {
        body.name = "Body Main";
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 startPos = new Vector3(body.transform.position.x, 1, body.transform.position.z);
        for(float i = startAngle; i <= endAngle; i += 5)
        {
            Vector3 targetPos = weapon.transform.position + (Quaternion.Euler(i, 0, 0) * (weapon.transform.forward * -1)).normalized * 500;
            RaycastHit hit;
            if (Physics.Raycast(startPos,  targetPos, out hit))
            {
                if(hit.collider.gameObject.name != "Background")
                    print("HIT: " + hit.collider.gameObject.name);
            }
        }
    }
}
