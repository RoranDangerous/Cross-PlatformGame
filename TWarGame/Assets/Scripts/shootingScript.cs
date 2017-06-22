using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootingScript : Photon.MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameObject weapon;
    private float startAngle = 15;
    private float endAngle = -15;
    private float timeLeft = 0;
    private float reloadTime = 3f;
    private GameObject hitObject;
    private bool buttonPressed = false;
	public bool shot = false;

    void Start()
    {
        SetReloadTime(3f);
        AssignObjects();
    }

    private void Update()
    {
        DecreaseTime();

        if (buttonPressed)
        {
            Shoot();
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        AnimateButtonUP();
        buttonPressed = false;
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        AnimateButtonDown();
        buttonPressed = true;
    }

    private void AssignObjects()
    {
        weapon = transform.root.transform.Find("weapon").gameObject;
        GetComponent<Image>().rectTransform.anchoredPosition = new Vector3(-(transform.parent.GetComponent<RectTransform>().rect.width * .4f / 3), transform.parent.GetComponent<RectTransform>().rect.height * .6f / 3, 0);
    }

    private void DecreaseTime()
    {
        timeLeft -= Time.deltaTime;
    }

    private void Shoot()
    {
        if (Reloaded())
        {
			EnableAnimation ();
			shot = true;
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
        //Vector3 targetPos = weapon.transform.position + (Quaternion.Euler(angle, 0, 0) * (weapon.transform.forward * -1)).normalized * 500;
		Vector3 targetPos = (Quaternion.Euler(angle, 0, 0) * (-weapon.transform.forward));
        RaycastHit hit;
		Debug.DrawRay(weapon.transform.position, targetPos * 1000f, Color.white, 20, true);
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

    private void AnimateButtonDown()
    {
        GetComponent<Image>().rectTransform.localScale = new Vector3(.9f,.9f,0);
    }

    private void AnimateButtonUP()
    {
        GetComponent<Image>().rectTransform.localScale = new Vector3(1, 1, 0);
    }

	private void EnableAnimation()
	{
		weapon.GetComponent<Animator> ().Play("playAnim");
	}
}
