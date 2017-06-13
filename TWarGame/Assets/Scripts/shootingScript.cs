using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingScript : Photon.MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject weapon;
    private float startAngle = 15;
    private float endAngle = -15;
    private float timeLeft = 0;
    private float reloadTime = 3f;
    private GameObject hitObject;

    void Start()
    {
        SetReloadTime(3f);
        AssignObjects();
    }

    private void Update()
    {
        DecreaseTime();
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {

    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Shoot();
    }

    private void AssignObjects()
    {
        weapon = transform.root.transform.Find("weapon").gameObject;
    }

    private void DecreaseTime()
    {
        timeLeft -= Time.deltaTime;
    }

    private void Shoot()
    {
        if (Reloaded())
        {
            timeLeft = reloadTime;
            for (float i = startAngle; i >= endAngle; i -= 1)
            {
                if (ShootRaycast(i))
                    break;
            }
        }
    }

    private bool Reloaded()
    {
        return timeLeft <= 0;
    }

    private bool ShootRaycast(float angle)
    {
        Vector3 targetPos = weapon.transform.position + (Quaternion.Euler(angle, -4, 0) * (weapon.transform.forward * -1)).normalized * 500;
        RaycastHit hit;
        //Debug.DrawRay(weapon.transform.position, targetPos * 1000f, Color.green, 20, true);
        if (Physics.Raycast(weapon.transform.position, targetPos, out hit))
        {
            hitObject = hit.collider.gameObject.transform.root.gameObject;
            if ( !isThis(hitObject) )
            {
                if(hitObject.GetComponent<PhotonView>() != null)
                {
                    hitObject.GetComponent<PhotonView>().RPC("ApplyDamage", PhotonPlayer.Find(hitObject.GetComponent<PhotonView>().owner.ID));
                    return true;
                }
                
            }
        }
        return false;
    }

    private bool isThis(GameObject target)
    {
        return target == this.transform.root.gameObject;
    }

    public void SetReloadTime(float time)
    {
        reloadTime = time;
    }

    public float GetReloadTime()
    {
        return reloadTime;
    }

    public float GetTimeLeft()
    {
        return timeLeft;
    }
}
