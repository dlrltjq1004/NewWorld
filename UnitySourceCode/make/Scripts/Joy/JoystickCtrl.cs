using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickCtrl : MonoBehaviour ,IPointerDownHandler, IDragHandler, IPointerUpHandler {

    RectTransform pad;
    RectTransform stick;
    Vector3 axis;
    Vector3 defaultCenter;
    float radius;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            // Player Update
if (InputManager.Inst.IsJoystickMove) {
    var axis = InputManager.Inst.Axis;
    if (axis != Vector2.zero) {
        // Animator
     //   aniController.SetTrigger("Run");
        var angle = new Vector3(transform.eulerAngles.x, Mathf.Atan2(axis.x, axis.y) * Mathf.Rad2Deg, transform.eulerAngles.z);
    transform.eulerAngles = angle;
    }
// transform.Translate(Vector3.forward* speed * Time.deltaTime);
} else {
  //  aniController.SetTrigger("Stand");
}
    }

    void Awake()
    {
        pad = transform.Find("Pad").GetComponent<RectTransform>();
    //    stick = transform.FindTransform("Stick").GetComponent<RectTransform>();
        radius = stick.sizeDelta.y * 0.5f;
        pad.gameObject.SetActive(false);
        stick.gameObject.SetActive(false);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        pad.gameObject.SetActive(true);
        stick.gameObject.SetActive(true);

        pad.transform.position = eventData.position;
        stick.transform.position = eventData.position;
        defaultCenter = eventData.position;
        InputManager.Inst.IsJoystickMove = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Move(eventData);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        axis = Vector3.zero;
        stick.position = defaultCenter;
        InputManager.Inst.IsJoystickMove = false;
        InputManager.Inst.Axis = axis;

        pad.gameObject.SetActive(false);
        stick.gameObject.SetActive(false);
    }

    void Move(PointerEventData eventData)
    {
        Vector3 touchPosition = eventData.position;
        axis = (touchPosition - defaultCenter).normalized;

        float distance = Vector3.Distance(touchPosition, defaultCenter);

        if (distance > radius)
            stick.position = defaultCenter + axis * radius;
        else
            stick.position = defaultCenter + axis * distance;

        // axis를 이용해 이동처리
        InputManager.Inst.Axis = axis;
    }


 



}


