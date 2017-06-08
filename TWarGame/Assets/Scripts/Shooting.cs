using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shooting : Photon.MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject body;
    public GameObject weapon;
    private float startAngle = -15;
    private float endAngle = 15;
    public bool shot = false;
    float timeLeft;
    public int lives = 3;
    

    public virtual void OnPointerUp(PointerEventData ped)
    {
        //bullet.GetComponent<Rigidbody>().AddForce(weapon.transform.forward * 50);
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector3 startPos = new Vector3(body.transform.position.x, 1, body.transform.position.z);
        for (float i = startAngle; i <= endAngle; i += 3)
        {
            Vector3 targetPos = weapon.transform.position + (Quaternion.Euler(i, 0, 0) * (weapon.transform.forward * -1)).normalized * 500;
            RaycastHit hit;
            if (Physics.Raycast(startPos, targetPos, out hit))
            {

                if (hit.collider.gameObject.GetComponent<PhotonView>() != null && !hit.collider.gameObject.GetComponent<PhotonView>().isMine && hit.collider.gameObject.name != "Background")
                {
                    //print("OTHER " + hit.collider.gameObject.name);
                    if (timeLeft <= 0)
                    {
                        print("Shot");
                        shot = true;
                        timeLeft = 3f;
                    }
                }
                else
                {
                }
            }
        }
        //bullet.GetComponent<Rigidbody>().AddForce(-weapon.transform.forward * 50);
    }

    // Use this for initialization
    void Start () {
        body.name = "Body Main";
        timeLeft = 3f;
	}
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;

        print("Update lives: " + lives);
        if (lives <= 1)
        {
            print("Lives == 0");
            body.SetActive(false);
            weapon.SetActive(false);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
           // print("Writing " + info);
            if (shot)
            {
                stream.SendNext("HIT HIT HIT");
                shot = false;
                print("SENT");
                print("LIVES: " + lives);
            }
        }
        else
        {
            String received = (String)stream.ReceiveNext();
            if (received == "HIT HIT HIT")
            {
                print("Received HIT HIT HIT");
                //decreaseLife();
                GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All);
            }
        }
    }

    public void decreaseLife()
    {
        lives -= 1;
    }

    [PunRPC]
    void ApplyDamage()
    {
        print("Apply Damage!");
        print(body.GetComponent<PhotonView>().isMine);
        lives -= 1;
    }

}
