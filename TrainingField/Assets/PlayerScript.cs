using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PlayerScript : MonoBehaviour
{
    public CharacterController cCont;

    public float speed = 12f;
    public float gravity = -9.81f  * 2f;
    public float jumpHeight = 3f;

    private Vector3 velocity;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check?
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        var transform1 = transform;
        Vector3 move = transform1.right * x + transform1.forward * z;

        cCont.Move(move * (speed * Time.deltaTime));

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        velocity.y += gravity * Time.deltaTime;
        cCont.Move(velocity * Time.deltaTime);
    }
}
