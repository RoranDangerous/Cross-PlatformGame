using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {

    public GameObject cam;
    public bool myCar = true;
    private Quaternion fixedRotation;
    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;
    public double currentTime = 0.0;
    public double currentPacketTime = 0.0;
    public double lastPacketTime = 0.0;
    public double timeToReachGoal = 0.0;
    public Vector3 positionAtLastPacket;
    public Vector3 realPosition = Vector3.zero;
    public Quaternion rotationAtLastPacket;
    public Quaternion realRotation;

    void Start () {
        if (!photonView.isMine)
        {
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;

            myCar = false;
            cam.SetActive(false);
            //transform.position = Vector3.Lerp(transform.position, realPosition, Time.deltaTime * 5);
            //transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);

            //transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
            //transform.rotation = Quaternion.Lerp(rotationAtLastPacket, realRotation, (float)(currentTime / timeToReachGoal));
        }
        fixedRotation = cam.transform.rotation;
	}
	
	void Update () {
        if (cam.transform.rotation != fixedRotation)
            cam.transform.rotation = fixedRotation;
        if (!photonView.isMine)
        {
            timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, realRotation, (float)(currentTime / timeToReachGoal));
            /*transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, Time.deltaTime * 10000);
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, realRotation, Time.deltaTime * 10000);*/
        }

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Network player, receive data
            //this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            currentTime = 0.0;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();

            lastPacketTime = currentPacketTime;
            currentPacketTime = info.timestamp;
            /*timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;*/

            if (!photonView.isMine)
            {
                /*transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));
                transform.rotation = Quaternion.Lerp(rotationAtLastPacket, realRotation, (float)(currentTime / timeToReachGoal));*/
                /*transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, Time.deltaTime * 10000);
                transform.rotation = Quaternion.Lerp(rotationAtLastPacket, realRotation, Time.deltaTime * 10000);*/
            }

        }
    }
}
