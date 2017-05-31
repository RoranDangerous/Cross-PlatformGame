using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image outerJ;
    private Image innerJ;
    public GameObject weapon;
    public GameObject body;
    private Vector3 inputVector;
    private Vector3 startAnchoredPosition;
    public GameObject player;

    void Start () {
        outerJ = GetComponent<Image>();
        innerJ = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(outerJ.rectTransform
                       , new Vector2(ped.position.x - 250, ped.position.y + 250)
                       , ped.pressEventCamera
                       , out pos))
        {
            pos.x = (pos.x / (outerJ.rectTransform.sizeDelta.x));
            pos.y = (pos.y / (outerJ.rectTransform.sizeDelta.y));

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //Set the position of the inner joystick
            innerJ.rectTransform.anchoredPosition =
             new Vector3(inputVector.x * (outerJ.rectTransform.sizeDelta.x / 3)
              , inputVector.z * (outerJ.rectTransform.sizeDelta.y / 3));
            //Rotate the weapon
            /*Vector3 target = new Vector3();
            target.z = innerJ.rectTransform.anchoredPosition.y;
            weapon.transform.LookAt(target);*/
            //print((innerJ.rectTransform.anchoredPosition.x - player.transform.position.x)+" "+ (innerJ.rectTransform.anchoredPosition.y - player.transform.position.z));
            //weapon.transform.LookAt(new Vector3(innerJ.rectTransform.anchoredPosition.x - player.transform.position.x,0, innerJ.rectTransform.anchoredPosition.y - player.transform.position.z));
            weapon.transform.LookAt(new Vector3(-innerJ.rectTransform.anchoredPosition3D.x+14, 0, -innerJ.rectTransform.anchoredPosition3D.y+14));
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        innerJ.rectTransform.anchoredPosition = Vector3.zero;
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    /*public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {

        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }*/
}
