using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirutalJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image outerJ;
    public Image innerJ;
    public GameObject cylinder;
    public GameObject cube;
    private Vector3 inputVector;

    void Start()
    {
        outerJ = GetComponent<Image>();
        innerJ = transform.GetChild(0).GetComponent<Image>();

    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(outerJ.rectTransform
                       , new Vector2(ped.position.x-150,ped.position.y+150)
                       , ped.pressEventCamera
                       , out pos))
        {
            pos.x = (pos.x / (outerJ.rectTransform.sizeDelta.x));
            pos.y = (pos.y / (outerJ.rectTransform.sizeDelta.y));
            
            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            innerJ.rectTransform.anchoredPosition =
             new Vector3(inputVector.x * (outerJ.rectTransform.sizeDelta.x/3)
              , inputVector.z * (outerJ.rectTransform.sizeDelta.y /3));
            cylinder.transform.RotateAround(cube.transform.position,new Vector3(0, 0, 0), 10);
            //cylinder.GetComponent<Renderer>().bounds.center = cube.transform.position;
            //cylinder.transform.LookAt(innerJ.rectTransform.anchoredPosition);
            cylinder.transform.LookAt(innerJ.rectTransform.anchoredPosition);
            cube.transform.position = new Vector3(cube.transform.position.x + 0.01f, cube.transform.position.y + 0.01f, cube.transform.position.z);

            /*Vector3 targetPoint = new Vector3(innerJ.rectTransform.anchoredPosition.x, innerJ.rectTransform.anchoredPosition.y,0);
            Quaternion targetRotation = Quaternion.LookRotation(-cube.transform.position, innerJ.rectTransform.anchoredPosition);
            cylinder.transform.rotation = Quaternion.Slerp(cylinder.transform.rotation, targetRotation, Time.deltaTime * 2.0f);*/
            /*Vector3 relativePos = cylinder.transform.position - cube.transform.position;
            Quaternion rotation = Quaternion.LookRotation(new Vector3(innerJ.rectTransform.anchoredPosition.x, innerJ.rectTransform.anchoredPosition.y,0));
            //cylinder.transform.rotation = Quaternion.Slerp(cylinder.transform.rotation,rotation,10);
            cylinder.transform.rotation = rotation;*/
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }


    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        innerJ.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
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
    }
}
