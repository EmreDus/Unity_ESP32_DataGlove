using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public Camera cam;

    private float speed = 1f;

    public float sensitivity = 80f;
    public Transform Camera;

    private float xRotation = 45f;
    private float yRotation = 90f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse1)){
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xRotation -= mouseY;
            yRotation -= mouseX;

            Camera.transform.localRotation = Quaternion.Euler(xRotation, -yRotation, 0f); 

            //Camera Movement
            float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
            float updown = Input.GetAxis("updown") * Time.deltaTime * speed;

            Vector3 direction = cam.transform.forward * vertical + cam.transform.right * horizontal;
            direction = direction.normalized; // normalize the direction vector

            transform.Translate(horizontal, updown, -vertical);
            
        }
    }
}