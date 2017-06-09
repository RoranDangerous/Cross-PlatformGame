using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {

    private GameObject cam;
    private GameObject body;
    private GameObject weapon;
    public bool myCar = true;
    private Quaternion fixedRotation;
    private double currentTime = 0.0;
    private double currentPacketTime = 0.0;
    private double lastPacketTime = 0.0;
    private double timeToReachGoal = 0.0;

    private Vector3 positionAtLastPacketBody;
    private Quaternion rotationAtLastPacketBody;
    private Vector3 positionAtLastPacketWeapon;
    private Quaternion rotationAtLastPacketWeapon;
    private Vector3 realPositionBody;
    private Quaternion realRotationBody;
    private Vector3 realPositionWeapon;
    private Quaternion realRotationWeapon;
    private GameObject firebtn;
    private shootingScript shotScript;

    int lives = 3;
    

    void Start ()
    {
        body = transform.Find("body").gameObject;
        cam = transform.Find("Camera").gameObject;
        weapon = transform.Find("weapon").gameObject;
        body.AddComponent<Rigidbody>();
        body.GetComponent<Rigidbody>().isKinematic = true;
        body.GetComponent<Rigidbody>().useGravity = true;
        body.AddComponent<BoxCollider>();
        firebtn = cam.transform.Find("PlayerUI").gameObject.transform.Find("Fire").gameObject;
        shotScript = firebtn.GetComponent<shootingScript>();

        if (!photonView.isMine && body != null && weapon != null)
        {
            positionAtLastPacketBody = body.transform.position;
            rotationAtLastPacketBody = body.transform.rotation;
            positionAtLastPacketWeapon = weapon.transform.position;
            rotationAtLastPacketWeapon = weapon.transform.rotation;

            myCar = false;
            cam.SetActive(false);
        }

        if(cam != null)
            fixedRotation = cam.transform.rotation;
	}
	
	void Update () {
        
        if(cam != null)
            if (cam.transform.rotation != fixedRotation)
                cam.transform.rotation = fixedRotation;

        if (!photonView.isMine && body != null && weapon != null)
        {
            timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;
            if ((float)(currentTime / timeToReachGoal) != 0 && currentTime!=0 && timeToReachGoal !=0)
            {
                body.transform.position = Vector3.Lerp(positionAtLastPacketBody, realPositionBody, (float)(currentTime / timeToReachGoal));
                body.transform.rotation = Quaternion.Lerp(rotationAtLastPacketBody, realRotationBody, (float)(currentTime / timeToReachGoal));

                weapon.transform.position = Vector3.Lerp(positionAtLastPacketWeapon, realPositionWeapon, (float)(currentTime / timeToReachGoal));
                weapon.transform.rotation = Quaternion.Lerp(rotationAtLastPacketWeapon, realRotationWeapon, (float)(currentTime / timeToReachGoal));
            }
        }

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (body != null && weapon != null)
        {

            if (stream.isWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(body.transform.position);
                stream.SendNext(body.transform.rotation);
                stream.SendNext(weapon.transform.position);
                stream.SendNext(weapon.transform.rotation);

                if (shotScript.shot)
                {
                    stream.SendNext("HIT HIT HIT");
                    stream.SendNext(shotScript.hitTargetID);
                    shotScript.shot = false;
                }
            }
            else
            {
                // Network player, receive data
                currentTime = 0.0;

                positionAtLastPacketBody = body.transform.position;
                rotationAtLastPacketBody = body.transform.rotation;
                positionAtLastPacketWeapon = weapon.transform.position;
                rotationAtLastPacketWeapon = weapon.transform.rotation;

                realPositionBody = (Vector3)stream.ReceiveNext();
                realRotationBody = (Quaternion)stream.ReceiveNext();
                realPositionWeapon = (Vector3)stream.ReceiveNext();
                realRotationWeapon = (Quaternion)stream.ReceiveNext();

                lastPacketTime = currentPacketTime;
                currentPacketTime = info.timestamp;

                if(stream.ToArray().Length > 7)
                {
                    string received = (string)stream.ReceiveNext();
                    if (received == "HIT HIT HIT")
                    {
                        int pID = (int)stream.ReceiveNext();
                        print("Received HIT HIT HIT " + pID);
                        if (pID == PhotonNetwork.player.ID)
                        {
                            print("Player == true");
                            GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, pID);
                        }
                    }
                }
                
            }
        }
    }

    [PunRPC]
    void ApplyDamage(int pID)
    {
        if (pID == PhotonNetwork.player.ID)
        {
            print("Apply Damage! " + lives);
            lives -= 1;
            if (lives <= 0)
                PhotonNetwork.Disconnect();
        }
    }
}
