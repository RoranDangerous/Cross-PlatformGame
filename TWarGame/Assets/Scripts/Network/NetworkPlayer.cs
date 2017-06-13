using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Vector3 harcPositionAtlastPacket;
    private Vector3 realHarcPosition;
    private Vector3 healthScaleAtLastPacket;
    private Vector3 realHealthScale;

    private GameObject firebtn;
    private ShootingScript shotScript;
    private Heath healthScript;
    private PhotonTransformView healthTransform;

    private Texture2D bgImage;
    private Texture2D fgImage;
    private Image newImg;
    private GameObject harc;
    private GameObject healthBar;
    private GameObject reloadBar;
    

    void Start ()
    {
        AssignObjects();      

        if (!photonView.isMine && CheckBodyAndWeapon())
        {
            UpdateLastPositionAndRotation();

            myCar = false;
            cam.SetActive(false);
        }

        if(cam != null)
            fixedRotation = cam.transform.rotation;
	}
	
	void Update () {

        UpdateCameraRotation();

        if (!photonView.isMine && CheckBodyAndWeapon())
        {
            UpdatePositionOfPlayers();
        }
    }

    void OnGUI()
    {
        UpdateHealthBar();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (CheckBodyAndWeapon())
        {
            if (stream.isWriting)
            {
                // We own this player: send the others our data
                SendStream(stream);
            }
            else
            {
                // Network player, receive data
                currentTime = 0.0;

                // Save last position
                UpdateLastPositionAndRotation();

                // Get data from stream
                realPositionBody = (Vector3)stream.ReceiveNext();
                realRotationBody = (Quaternion)stream.ReceiveNext();
                realPositionWeapon = (Vector3)stream.ReceiveNext();
                realRotationWeapon = (Quaternion)stream.ReceiveNext();
                realHealthScale =  (Vector3)stream.ReceiveNext();
                realHarcPosition = (Vector3)stream.ReceiveNext();

                // Update the time of the packet
                lastPacketTime = currentPacketTime;
                currentPacketTime = info.timestamp;                
            }
        }
    }

    [PunRPC]
    void ApplyDamage()
    {
        RemoveHealth(3000);

        if(healthScript.currentHealth < 0)
        {
            Destroy(body);
            Destroy(weapon);
            Destroy(harc);
        }
    }

    private void RemoveHealth(int num)
    {
        healthScript.currentHealth -= num;
    }

    private void SendStream(PhotonStream stream)
    {
        stream.SendNext(body.transform.position);
        stream.SendNext(body.transform.rotation);
        stream.SendNext(weapon.transform.position);
        stream.SendNext(weapon.transform.rotation);
        stream.SendNext(healthBar.transform.localScale);
        stream.SendNext(harc.transform.position);
    }

    private bool CheckBodyAndWeapon()
    {
        return body != null && weapon != null;
    }

    private void UpdateHealthBar()
    {
        healthBar.transform.localScale = new Vector3((float)(healthScript.currentHealth < 0 ? 0 : healthScript.currentHealth) / (float)healthScript.maxHealth, 1, 1);

        if (shotScript.GetTimeLeft() >= 0)
        {
            reloadBar.transform.localScale = new Vector3(1 - shotScript.GetTimeLeft() / shotScript.GetReloadTime(), 1, 1);
        }
        else
            reloadBar.transform.localScale = new Vector3(1, 1, 1);
    }

    private void UpdatePositionOfPlayers()
    {
        timeToReachGoal = currentPacketTime - lastPacketTime;
        currentTime += Time.deltaTime;
        if (currentTime != 0 && timeToReachGoal != 0)
        {
            float lerpTime = (float)(currentTime / timeToReachGoal);
            body.transform.position = Vector3.Lerp(positionAtLastPacketBody, realPositionBody, lerpTime);
            body.transform.rotation = Quaternion.Lerp(rotationAtLastPacketBody, realRotationBody, lerpTime);

            weapon.transform.position = Vector3.Lerp(positionAtLastPacketWeapon, realPositionWeapon, lerpTime);
            weapon.transform.rotation = Quaternion.Lerp(rotationAtLastPacketWeapon, realRotationWeapon, lerpTime);

            healthBar.transform.localScale = Vector3.Lerp(healthScaleAtLastPacket, realHealthScale, lerpTime);
            harc.transform.position = Vector3.Lerp(harcPositionAtlastPacket, realHarcPosition, lerpTime);
        }
    }

    private void UpdateCameraRotation()
    {
        if (cam != null)
            if (cam.transform.rotation != fixedRotation)
                cam.transform.rotation = fixedRotation;
    }

    private void AssignObjects()
    {
        body = transform.Find("body").gameObject;
        cam = transform.Find("Camera").gameObject;
        weapon = transform.Find("weapon").gameObject;
        body.AddComponent<Rigidbody>();
        body.GetComponent<Rigidbody>().isKinematic = true;
        body.GetComponent<Rigidbody>().useGravity = true;
        body.AddComponent<BoxCollider>();
        firebtn = cam.transform.Find("PlayerUI").gameObject.transform.Find("Fire").gameObject;
        shotScript = firebtn.GetComponent<ShootingScript>();
        healthScript = GetComponent<Heath>();
        harc = transform.Find("healthAndReloadCanvas").gameObject;
        healthBar = harc.transform.Find("healthBar").gameObject.transform.Find("healthMain").gameObject;
        reloadBar = harc.transform.Find("reloadBar").gameObject.transform.Find("reloadMain").gameObject;
    }

    private void UpdateLastPositionAndRotation()
    {
        positionAtLastPacketBody = body.transform.position;
        rotationAtLastPacketBody = body.transform.rotation;
        positionAtLastPacketWeapon = weapon.transform.position;
        rotationAtLastPacketWeapon = weapon.transform.rotation;
        harcPositionAtlastPacket = harc.transform.position;
        healthScaleAtLastPacket = healthBar.transform.localScale;
    }
}
