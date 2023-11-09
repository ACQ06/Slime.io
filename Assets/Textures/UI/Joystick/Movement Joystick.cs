using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementJoystick : MonoBehaviour
{
    public GameObject joystick, joystickBackgroud;
    public Vector2 joystickVector;

    private Vector2 joystickTouchPos, joystickOriginalPos;
    private float joystickRadius;

    void Start()
    {
        joystickOriginalPos = joystickBackgroud.transform.position;
        joystickRadius = joystickBackgroud.GetComponent<RectTransform>().sizeDelta.y / 4;
    }

    public void PointerDown()
    {
        joystick.transform.position = Input.mousePosition;
        joystickBackgroud.transform.position= Input.mousePosition;
        joystickTouchPos = Input.mousePosition;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;
        joystickVector = (dragPos - joystickTouchPos).normalized;

        float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);

        if(joystickDist < joystickRadius)
        {
            joystick.transform.position = joystickTouchPos + joystickVector * joystickDist;
        }

        else
        {
            joystick.transform.position = joystickTouchPos + joystickVector * joystickRadius;
        }
    }

    public void PointerUp()
    {
        joystickVector = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
        joystickBackgroud.transform.position = joystickOriginalPos;
    }
}
