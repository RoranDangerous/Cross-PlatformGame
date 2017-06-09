using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class shootingScript : Photon.MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject body;
    private GameObject weapon;
    private Transform parentPlayer;
    private float startAngle = -15;
    private float endAngle = 15;
    public bool shot = false;
    float timeLeft;
    private PhotonView hitTarget;
    public int hitTargetID = 0;

    void Start()
    {
        parentPlayer = transform.parent.parent.parent.transform;
        body = parentPlayer.Find("body").gameObject;
        weapon = parentPlayer.Find("weapon").gameObject;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {

    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        print("Down");
        Vector3 startPos = new Vector3(body.transform.position.x, 1, body.transform.position.z);
        for (float i = startAngle; i <= endAngle; i += 3)
        {
            Vector3 targetPos = weapon.transform.position + (Quaternion.Euler(i, 0, 0) * (weapon.transform.forward * -1)).normalized * 500;
            RaycastHit hit;
            if (Physics.Raycast(startPos, targetPos, out hit))
            {
                GameObject rootOfHit = hit.collider.gameObject.transform.root.gameObject;
                print("Hit "+ rootOfHit.name);
                if (rootOfHit.GetComponent<PhotonView>() != null && 
                    !rootOfHit.GetComponent<PhotonView>().isMine &&
                    rootOfHit.name != "Background")
                {
                    if (timeLeft <= 0)
                    {
                        print("Shot");
                        shot = true;
                        timeLeft = 3f;
                        hitTarget = rootOfHit.GetComponent<PhotonView>();
                        hitTargetID = hitTarget.photonView.owner.ID;
                    }
                }
            }
        }
    }
}
