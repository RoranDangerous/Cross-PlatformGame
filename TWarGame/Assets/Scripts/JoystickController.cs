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
    public Vector3 weaponForward;

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
            weaponForward = innerJ.rectTransform.anchoredPosition;
            //Rotate the weapon
            Vector3 lookAtVector = new Vector3(weapon.transform.position.x - innerJ.rectTransform.anchoredPosition.x, 0, weapon.transform.position.z - innerJ.rectTransform.anchoredPosition.y);
            weapon.transform.LookAt(lookAtVector);
            //weapon.transform.LookAt(new Vector3(-innerJ.rectTransform.anchoredPosition3D.x+14, 0, -innerJ.rectTransform.anchoredPosition3D.y+14));
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
}
