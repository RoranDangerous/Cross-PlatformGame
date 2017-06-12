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
    private shootingScript shotScript;
    private heath healthScript;
    private PhotonTransformView healthTransform;

    private int maxHealth = 100;
    private int curHealth = 100;

    private Texture2D bgImage;
    private Texture2D fgImage;
    private Image newImg;
    private GameObject harc;
    private GameObject healthBar;
    private GameObject reloadBar;
    private bool temp = false;
    
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
        healthScript = GetComponent<heath>();
        //harc = cam.transform.Find("PlayerUI").gameObject.transform.Find("healthAndReloadCanvas").gameObject;
        harc = transform.Find("healthAndReloadCanvas").gameObject;
        
        //harc.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2, 100);
        //harc.transform.position = new Vector3(body.transform.position.x, body.transform.position.y + 2, body.transform.position.z);
        //harc.transform.Rotate(new Vector3(0,0,180));
        //harcPos = harc.transform.position;

        healthBar = harc.transform.Find("healthBar").gameObject.transform.Find("healthMain").gameObject;
        //healthBar.AddComponent<PhotonTransformView>();
        //healthTransform = healthBar.GetComponent<PhotonTransformView>();

        reloadBar = harc.transform.Find("reloadBar").gameObject.transform.Find("reloadMain").gameObject;

        //healthBarLength = Screen.width / 2;

        if (!photonView.isMine && body != null && weapon != null)
        {
            positionAtLastPacketBody = body.transform.position;
            rotationAtLastPacketBody = body.transform.rotation;
            positionAtLastPacketWeapon = weapon.transform.position;
            rotationAtLastPacketWeapon = weapon.transform.rotation;
            healthScaleAtLastPacket = healthBar.transform.localScale;
            harcPositionAtlastPacket = harc.transform.position;

            myCar = false;
            cam.SetActive(false);
        }

        if(cam != null)
            fixedRotation = cam.transform.rotation;
	}
	
	void Update () {
        
        if (cam != null)
            if (cam.transform.rotation != fixedRotation)
                cam.transform.rotation = fixedRotation;

        if (!photonView.isMine && body != null && weapon != null)
        {
            timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;
            if ((float)(currentTime / timeToReachGoal) != 0 && currentTime!=0 && timeToReachGoal !=0)
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

    }

    void OnGUI()
    {
        healthBar.transform.localScale = new Vector3((float)healthScript.health/ (float)3,1,1);
        if(shotScript.timeLeft >= 0)
        {
            reloadBar.transform.localScale = new Vector3(1-(float)shotScript.timeLeft / (float)shotScript.reloadTime, 1, 1);
        }
        else
            reloadBar.transform.localScale = new Vector3(1, 1, 1);
    }

    public void AddjustCurrentHealth(int adj)
    {
        curHealth += adj;

        if (curHealth < 0)
            curHealth = 0;

        if (curHealth > maxHealth)
            curHealth = maxHealth;

        if (maxHealth < 1)
            maxHealth = 1;
        
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (body != null && weapon != null)
        {
            if (stream.isWriting)
            {
                print(temp + " " + lives);
                // We own this player: send the others our data
                stream.SendNext(body.transform.position);
                stream.SendNext(body.transform.rotation);
                stream.SendNext(weapon.transform.position);
                stream.SendNext(weapon.transform.rotation);
                stream.SendNext(healthBar.transform.localScale);
                stream.SendNext(harc.transform.position);

                if (shotScript.shot)
                {
                    stream.SendNext("HIT HIT HIT");
                    stream.SendNext(shotScript.hitTargetID);
                    stream.SendNext(shotScript.rootOfHitID);
                    shotScript.shot = false;
                }
            }
            else
            {
                // Network player, receive data
                currentTime = 0.0;

                print("stream.ToArray().Length: " + stream.ToArray().Length);

                positionAtLastPacketBody = body.transform.position;
                rotationAtLastPacketBody = body.transform.rotation;
                positionAtLastPacketWeapon = weapon.transform.position;
                rotationAtLastPacketWeapon = weapon.transform.rotation;
                harcPositionAtlastPacket = harc.transform.position;
                healthScaleAtLastPacket = healthBar.transform.localScale;

                realPositionBody = (Vector3)stream.ReceiveNext();
                realRotationBody = (Quaternion)stream.ReceiveNext();
                realPositionWeapon = (Vector3)stream.ReceiveNext();
                realRotationWeapon = (Quaternion)stream.ReceiveNext();
                realHealthScale =  (Vector3)stream.ReceiveNext();
                realHarcPosition = (Vector3)stream.ReceiveNext();

                lastPacketTime = currentPacketTime;
                currentPacketTime = info.timestamp;

                if(stream.ToArray().Length > 9)
                {
                    string received = (string)stream.ReceiveNext();
                    if (received == "HIT HIT HIT")
                    {
                        int pID = (int)stream.ReceiveNext();
                        int rootID = (int)stream.ReceiveNext();
                        print("Received HIT HIT HIT " + pID);
                        if (pID == PhotonNetwork.player.ID)
                        {
                            //realHealthScale = healthBar.transform.localScale = new Vector3(.75f, 1, 1);

                            print("this.name: "+this.name);
                            GameObject[] arr = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                            foreach(GameObject i in arr)
                            {
                                print("PhotonView.IsMine " + i +" " +i.name);
                                if(i.name == "PlayerNew2")
                                {
                                    i.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, pID, rootID);
                                    realHealthScale = new Vector3(.5f, 1, 1);
                                }
                            }
                            //GetComponent<PhotonView>().RPC("ApplyDamage", PhotonTargets.All, pID,rootID);
                        }
                    }
                }
                
            }
        }
    }

    [PunRPC]
    void ApplyDamage(int pID, int rootID)
    {
        if (pID == PhotonNetwork.player.ID)
        {
            print("Apply Damage! " + lives);
            lives -= 1;
            AddjustCurrentHealth(-34);
            //if (lives <= 0)
            //print(PhotonNetwork.player.NickName);
            healthScript.health -= 1;
           
            healthBar.transform.localScale = new Vector3(.75f,1,1);
            //healthBar.GetComponent<PhotonTransformView>().gameObject.transform.localScale = new Vector3(0.75f, 1, 1);

            temp = true;
        }
        else
            healthBar.transform.localScale = new Vector3(.25f, 1, 1);
    }
}
