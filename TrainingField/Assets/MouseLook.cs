using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    private const float DefaultMouseSensitivity = 500f;
    public float mouseSensitivity = DefaultMouseSensitivity;
    
    public Slider sensitivitySlider;

    public Transform playerBody;

    private float xRotation = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    
    public void SenseSliderListener(System.Single speedSlideVal)
    {
        mouseSensitivity = DefaultMouseSensitivity * sensitivitySlider.value;
        //print("Slider updated.");
    }
}
