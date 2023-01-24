using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject Hand;

    float Speed = 0.3f;

    void Start()
    {
       
    }

    void Update()
    {
  
        if (Input.GetKey(KeyCode.A))
        {
            //Camera.transform.position += new Vector3(0,0,-0.03f);
            Hand.transform.Translate(Vector3.left * Speed * Time.deltaTime);         
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Camera.transform.position += new Vector3(0,0,0.03f);
            Hand.transform.Translate(Vector3.right * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            //Camera.transform.position += new Vector3(-0.03f,0,0);
            Hand.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //Camera.transform.position += new Vector3(0.03f,0,0);
            Hand.transform.Translate(Vector3.back * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            //Camera.transform.position += new Vector3(0,0.03f,0);
            Hand.transform.Translate(Vector3.up * Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //Camera.transform.position += new Vector3(0,-0.03f,0);
            Hand.transform.Translate(Vector3.down * Speed * Time.deltaTime);
        }
    }
}
