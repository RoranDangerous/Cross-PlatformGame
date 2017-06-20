using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image outerJ;
    private Image innerJ;
    private GameObject weapon;
    private Transform parentPlayer;
    private Vector3 inputVector;
    private Vector3 startAnchoredPosition;
    private Vector3 lookAtVector;

    void Start () {
        AssignObjects();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
		print ("OnDrag");
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(outerJ.rectTransform
                       , new Vector2(ped.position.x - outerJ.GetComponent<RectTransform>().rect.width/2, ped.position.y + outerJ.GetComponent<RectTransform>().rect.width/2)
                       , ped.pressEventCamera
                       , out pos))
        {
            pos.x = (pos.x / (outerJ.rectTransform.sizeDelta.x));
            pos.y = (pos.y / (outerJ.rectTransform.sizeDelta.y));

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            SetInnerJoystickPosition(inputVector);

            RotateWeapon();
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
	{
		print("OnPointerUp");
        SetJoystickPositionZero();
    }

    public void OnPointerDown(PointerEventData ped)
	{
		print("OnPointerDown");
        OnDrag(ped);
    }

    private void AssignObjects()
    {
        outerJ = GetComponent<Image>();
        outerJ.rectTransform.anchoredPosition = new Vector2((transform.parent.GetComponent<RectTransform>().rect.width * .5f / 3), transform.parent.GetComponent<RectTransform>().rect.height * .8f / 3);
        innerJ = transform.GetChild(0).GetComponent<Image>();
		parentPlayer = transform.root.transform;
        weapon = parentPlayer.Find("weapon").gameObject;
    }

    private void SetJoystickPositionZero()
    {
        inputVector = Vector3.zero;
        innerJ.rectTransform.anchoredPosition = Vector3.zero;
    }

    private void RotateWeapon()
    {
        lookAtVector = new Vector3(weapon.transform.position.x - innerJ.rectTransform.anchoredPosition.x, 0, weapon.transform.position.z - innerJ.rectTransform.anchoredPosition.y);
        weapon.transform.LookAt(lookAtVector);
    }

    private void SetInnerJoystickPosition(Vector3 inputVector)
    {
        innerJ.rectTransform.anchoredPosition =
             new Vector3(inputVector.x * (outerJ.rectTransform.sizeDelta.x / 3)
              , inputVector.z * (outerJ.rectTransform.sizeDelta.y / 3));
    }
}
