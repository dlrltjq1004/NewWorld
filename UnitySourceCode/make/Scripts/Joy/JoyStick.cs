using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyScript : MonoBehaviour {

    protected Joystick joystick;
    protected JoyAttackbutton joyAttackbutton;
    public Animator animator;

  


    // Use this for initialization
    void Start()
    {

        joystick = FindObjectOfType<Joystick>();
        joyAttackbutton = FindObjectOfType<JoyAttackbutton>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        var rigidbody = GetComponent<Rigidbody>();

        rigidbody.velocity = Move(rigidbody);

     
        
        
    }


    private Vector3 Move(Rigidbody rigidbody)
    {
        return new Vector3(joystick.Horizontal * 7f,
                           rigidbody.velocity.y,
                           joystick.Vertical * 7f);

        /*
        return new Vector3(joystick.Horizontal * 7f,
                                                 rigidbody.velocity.y,
                                                 joystick.Vertical * 7f
                                                 );
                                                 */
       
    }


}
