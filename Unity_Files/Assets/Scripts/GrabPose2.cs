using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabPose2 : MonoBehaviour
{
    private float pose_serce, pose_yuzuk, pose_orta, pose_isaret, pose_bas;
    public GameObject GrabZone2;
    public XRSocketInteractor socket;
    [SerializeField] private Rigidbody rb;
    [Space]
    public float pose_min = 30f;
    public float pose_max = 50f;
    [Space]
    public float pose_bas_min = 8f;
    public float pose_bas_max = 8f;


    void Start()
    {
        socket = GrabZone2.GetComponent<XRSocketInteractor>();
        rb = GetComponent<Rigidbody>();
        socket.socketActive = false;
        GrabZone2.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        float pose_isaret = FingerCalib.Cal_isaret;
        float pose_orta = FingerCalib.Cal_orta;
        float pose_yuzuk = FingerCalib.Cal_yuzuk;
        float pose_serce = FingerCalib.Cal_serce;
        float pose_bas = FingerCalib.Cal_bas;

        if(pose_isaret >= pose_min && pose_isaret <= pose_max && pose_bas >= pose_bas_min && pose_bas <= pose_bas_max && pose_orta <= pose_max 
        && pose_orta >= pose_min && pose_yuzuk <= pose_max && pose_yuzuk >= pose_min && pose_serce <= pose_max && pose_serce >= pose_min){
            
            socket.socketActive = true;
            GrabZone2.GetComponent<Renderer>().enabled = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        else{
            socket.socketActive = false;
            GrabZone2.GetComponent<Renderer>().enabled = false;
            rb.constraints = RigidbodyConstraints.None;
        }

    }
}
