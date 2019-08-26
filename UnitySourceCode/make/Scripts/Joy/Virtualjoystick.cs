using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Virtualjoystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

    [HideInInspector]
    protected bool pressed;
    public Animator animator;
    private float h;
    private float v;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        animator.SetFloat("h",h);
        animator.SetFloat("v",v);
        animator.Play("Cat_Run", -1, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
     
       
    }
    


    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }

}
