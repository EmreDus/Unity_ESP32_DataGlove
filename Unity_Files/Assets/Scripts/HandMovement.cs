using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMovement : MonoBehaviour
{
    private float _qx, _qy, _qz, _qw;
    private Rigidbody rb;
    
    public float torqueX = 100f;
    public float torqueY = 100f;
    public float torqueZ = 100f;

    private float speed = 0.2f;
    private float maxVelocity = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //ESP Datas
        float _qx = ESP32_com.qx;
        float _qy = ESP32_com.qy;
        float _qz = ESP32_com.qz;
        float _qw = ESP32_com.qw;
        
        //Rotation
        torqueX = torqueX * _qx;
        torqueY = torqueY * _qy;
        torqueZ = torqueZ * _qz;

        transform.rotation = new Quaternion(-_qy, -_qz, _qx, _qw);
        /*
        if(_qx != 0 && _qy != 0 && _qz != 0){
            Quaternion torque = Quaternion.Euler(torqueX, torqueY, torqueZ);
            Vector3 torqueVector = torque.eulerAngles;
            rb.AddTorque(torqueVector);
        }*/

        //Movement
        if(!Input.GetKey(KeyCode.Mouse1)){ // For Camera Movement
            float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;
            float updown = Input.GetAxis("updown") * Time.deltaTime * speed;

            transform.position += new Vector3(vertical, updown, horizontal);
        }

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
}
